using AutoMapper;
using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Utilities;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExpenseTracker.Services.Repository
{
    public class MasterRepository : IMasterRepository
    {
        private readonly AppDBContext _appDBContext;
        private readonly IMapper _mapper;
        public MasterRepository(AppDBContext appDBContext, IMapper mapper)
        {
            _appDBContext = appDBContext;
            _mapper = mapper;
        }

        #region Bank Master
        public async Task<TblBankMaster> AddBankMasterAsync(TblBankMaster bank)
        {

            if (_appDBContext.BankMasters.Any(b => b.BankCode == bank.BankCode))
            {
                throw new Exception("Bank masyter already exist of this bank code");
            }
            await _appDBContext.BankMasters.AddAsync(bank);
            await _appDBContext.SaveChangesAsync();

            return bank;
        }

        public async Task DeleteBankMasterAsync(int bankId)
        {
            await _appDBContext.BankMasters.Where(b => b.Id == bankId).ExecuteDeleteAsync();
        }

        public async Task<IList<TblBankMaster>> GetAllBankMastersAsync()
        {
            List<TblBankMaster> bankMasters = await _appDBContext.BankMasters.ToListAsync();

            return bankMasters;
        }

        public async Task<TblBankMaster> GetBankMasterByIdAsync(int bankId)
        {
            TblBankMaster bankMaster = await _appDBContext.BankMasters.AsNoTracking().Where(b => b.Id == bankId)
                                              .FirstAsync();

            return bankMaster;

        }

        public async Task UpdateBankMaster(BankMasterDto bank)
        {
            TblBankMaster existingBankMaster = await _appDBContext.BankMasters.Where(b => b.Id == bank.Id)
                                              .FirstAsync();
            _mapper.Map(bank, existingBankMaster);

            _appDBContext.BankMasters.Update(existingBankMaster).Property(p => p.Id).IsModified = false;
            _appDBContext.SaveChanges();
        }
        #endregion

        #region Role Master
        public async Task<IList<TblRole>> GetAllRoles()
        {
            List<TblRole> roleMasters = await _appDBContext.RoleMasters.AsNoTracking().ToListAsync();

            return roleMasters;
        }

        public async Task<TblRole> GetRoleById(int roleId)
        {
            TblRole role = await _appDBContext.RoleMasters.Where(x => x.RoleId == roleId).FirstAsync();
            if (role == null)
            {
                throw new Exception("Role Not Found");
            }
            return role;
        }

        public async Task<TblRole> AddRole(TblRole role)
        {
            await _appDBContext.RoleMasters.AddAsync(role);
            await _appDBContext.SaveChangesAsync();
            return role;
        }

        public async Task<TblRole> UpdateRole(TblRole role)
        {
            var _rolerMaster = await _appDBContext.RoleMasters.FindAsync(role?.RoleId);
            if (_rolerMaster == null)
            {
                throw new Exception("Role Not Found");
            }
            var updateRole = new EntityPropertyUpdaterHelperWithEF<TblRole>(_appDBContext);
            updateRole.UpdateEntityModifiedProperties(_rolerMaster, role);
            await _appDBContext.SaveChangesAsync();
            return _rolerMaster;
        }

        public async Task DeleteRole(int roleId)
        {
            await _appDBContext.RoleMasters.Where(x => x.RoleId == roleId).ExecuteDeleteAsync();
        }
        #endregion


        #region Location Master
        public async Task<IList<TblLocation>> GetAllLocations()
        {
            List<TblLocation> location = await _appDBContext.LocationMasters.AsNoTracking().ToListAsync();
            return location;
        }

        #endregion

        #region Department Master
        public async Task<IList<TblDepartment>> GetAllDepartments()
        {
            List<TblDepartment> department = await _appDBContext.DepartmentMasters.AsNoTracking().ToListAsync();
            return department;
        }

		#endregion


		#region Plan Master
		public async Task<PlanMaster> AddPlanMasterAsync(PlanMaster plan)
		{
			if (_appDBContext.PlanMasters.Any(p => p.PlanCode == plan.PlanCode))
			{
				throw new Exception("Plan master already exists with this plan code");
			}

			await _appDBContext.PlanMasters.AddAsync(plan);
			await _appDBContext.SaveChangesAsync();

			return plan;
		}

		public async Task DeletePlanMasterAsync(int planId)
		{
            PlanMaster plan = await _appDBContext.PlanMasters.Where(p => p.PlanId == planId).FirstOrDefaultAsync();

            plan.EndDate = DateTime.Now;
            plan.Status = false;

			_appDBContext.PlanMasters.Update(plan).Property(p => p.PlanId).IsModified = false;
			await _appDBContext.SaveChangesAsync();
		}

		public async Task<IList<PlanMaster>> GetAllPlanMastersAsync()
		{
			List<PlanMaster> planMasters = await _appDBContext.PlanMasters.AsNoTracking().ToListAsync();
			return planMasters;
		}

		public async Task<PlanMaster> GetPlanMasterByIdAsync(int planId)
		{
			PlanMaster planMaster = await _appDBContext.PlanMasters.AsNoTracking().Where(p => p.PlanId == planId)
										   .FirstOrDefaultAsync();
			return planMaster;
		}

		public async Task UpdatePlanMaster(PlanMasterDto plan)
		{
			PlanMaster existingPlanMaster = await _appDBContext.PlanMasters.Where(p => p.PlanId == plan.PlanId)
												.FirstOrDefaultAsync();

			_mapper.Map(plan, existingPlanMaster);

			_appDBContext.PlanMasters.Update(existingPlanMaster).Property(p => p.PlanId).IsModified = false;
			await _appDBContext.SaveChangesAsync();
		}


		#endregion

		#region sub Plan Master

		public async Task<IList<SubPlansMasterResponseDto>> GetAllSubPlanMastersByPlanIdAsync(int planId)
		{
            List<SubPlansMaster> subPlans = await _appDBContext.SubPlansMasters.AsNoTracking().Where(p => p.PlanId == planId).ToListAsync();
             List<SubPlansMasterResponseDto> suplanRespDto = _mapper.Map<List<SubPlansMasterResponseDto>>(subPlans);
            return suplanRespDto;
		}

		public async Task AddSubPlanMasterAsync(SubPlansMasterRequestDto subPlan)
		{
            var subplanEntity = _mapper.Map<SubPlansMaster>(subPlan);
            await _appDBContext.SubPlansMasters.AddAsync(subplanEntity);
            await _appDBContext.SaveChangesAsync();
		}		

		public async Task UpdateSubPlanMaster(SubPlansMasterRequestDto subPlan)
		{
			var subplanEntity = _mapper.Map<SubPlansMaster>(subPlan);
            var existingEntity = _appDBContext.SubPlansMasters.Where(p=> p.SubPlansId == subPlan.SubPlansId).FirstOrDefault();
            _mapper.Map(subplanEntity, existingEntity);
            _appDBContext.SubPlansMasters.Update(existingEntity).Property(p => p.SubPlansId).IsModified = false;
			_appDBContext.SubPlansMasters.Update(existingEntity).Property(p => p.CreatedDate).IsModified = false;
			await _appDBContext.SaveChangesAsync(true);
		}


        #endregion

        public async Task<IList<SplPlanMaster>> GetAllSplPlanMastersAsync()
        {
            List<SplPlanMaster> planMasters = await _appDBContext.SplPlanMaster.AsNoTracking().ToListAsync();
            return planMasters;
        }

        public async Task<SplPlanMaster> GetSplPlanMasterByIdAsync(int planId)
        {
            SplPlanMaster planMaster = await _appDBContext.SplPlanMaster.AsNoTracking().Where(p => p.PlanId == planId)
                                           .FirstOrDefaultAsync();
            return planMaster;
        }

        public async Task<SplPlanMaster> AddSplPlanMasterAsync(SplPlanMaster plan)
        {
            if (_appDBContext.SplPlanMaster.Any(p => p.PlanCode == plan.PlanCode))
            {
                throw new Exception("Plan master already exists with this plan code");
            }

            await _appDBContext.SplPlanMaster.AddAsync(plan);
            await _appDBContext.SaveChangesAsync();

            return plan;
        }

        public async Task DeleteSplPlanMasterAsync(int planId)
        {
            SplPlanMaster plan = await _appDBContext.SplPlanMaster.Where(p => p.PlanId == planId).FirstOrDefaultAsync();

            //plan.EndDate = DateTime.Now;
            plan.Status = false;

            _appDBContext.SplPlanMaster.Update(plan).Property(p => p.PlanId).IsModified = false;
            await _appDBContext.SaveChangesAsync();
        }

        public async Task UpdateSplPlanMaster(SplPlanMasterDto plan)
        {
            SplPlanMaster existingPlanMaster = await _appDBContext.SplPlanMaster.Where(p => p.PlanId == plan.PlanId)
                                                .FirstOrDefaultAsync();

            _mapper.Map(plan, existingPlanMaster);

            _appDBContext.SplPlanMaster.Update(existingPlanMaster).Property(p => p.PlanId).IsModified = false;
            _appDBContext.SplPlanMaster.Update(existingPlanMaster).Property(p => p.CreatedDate).IsModified = false;

            await _appDBContext.SaveChangesAsync();
        }

    }
}
