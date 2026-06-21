using AutoMapper;
using ExpenseTracker.Services.Contracts.IContracts.BusinessUnitContracts;
using ExpenseTracker.Services.Contracts.RequestResponseDto;
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExpenseTracker.Services.Contracts
{
    public class BusinessUnit : IBusinessUnit
    {
        private readonly ILogger<BusinessUnit> _logger;
        private readonly AppDBContext _dbContext;
        private readonly ResponseDto _responseDto;
        private readonly IMapper _mapper;
        public BusinessUnit(ResponseDto responseDto, AppDBContext appDBContext, 
                            ILogger<BusinessUnit> logger, IMapper mapper)
        {
            _dbContext = appDBContext;
            _responseDto = responseDto;
            _logger = logger;
            _mapper = mapper;
        }


        public async Task<ResponseDto> DeleteBU(int buId)
        {
            _logger.LogInformation($"Delete Business Unit for Id {buId}");
            _responseDto.Result = await _dbContext.TblBusinessUnits.Where(x=>x.BusinessUnitID == buId)
                                                                   .ExecuteDeleteAsync();
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Record Deleted for Business Unit Id {buId}");
            return _responseDto;
        }

        public async Task<ResponseDto> GetAllBU()
        {
            _logger.LogInformation($"Get All Business Unit");
            _responseDto.Result = await _dbContext.TblBusinessUnits.ToListAsync();
            _logger.LogInformation($"Retunrining All Business Unit");
            return _responseDto;
        }

        public async Task<ResponseDto> GetBU(int buId)
        {
            _logger.LogInformation($"Get Business Unit for ID {buId}");
            _responseDto.Result = await _dbContext.TblBusinessUnits.Where(x=>x.BusinessUnitID == buId)
                                                                    .FirstOrDefaultAsync();
            _logger.LogInformation($"Retunrining Business Unit Details for ID {buId}");
            return _responseDto;
        }

        public async Task<ResponseDto> CreateBusinessUnit(BusinessUnitRequestDto businessUnitRequestDto)
        {
            _logger.LogInformation($"Validation Started for Business Unit.");
            /* Apply Securoty Check */
            _logger.LogInformation($"Validation Completed for Business Unit.");
            _logger.LogInformation($"Business Unit Creation Started.");
            TblBusinessUnit businessUnit = _mapper.Map<TblBusinessUnit>( businessUnitRequestDto );
            _responseDto.Result = await _dbContext.TblBusinessUnits.AddAsync(businessUnit);
             await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"Business Unit Created Successfully.");
            return _responseDto;
        }

        public async Task<ResponseDto> UpdateBU(BusinessUnitRequestDto businessUnitRequestDto)
        {
            _logger.LogInformation($"Update Business Unit with parameters ");
            TblBusinessUnit tblBusinessUnit = _mapper.Map<TblBusinessUnit>(businessUnitRequestDto);
            _responseDto.Result = _dbContext.Update(tblBusinessUnit);
            _logger.LogInformation($"Business Unit Updated Successfully.");
            return _responseDto;
        }
    }
}
