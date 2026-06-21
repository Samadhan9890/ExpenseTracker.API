using AutoMapper;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Models.DTOs.ClientMaster;
using ExpenseTracker.Services.Models.DTOs.Subscriptions;
using ExpenseTracker.Services.Models.EntityModels;
using ExpenseTracker.Services.Models.ServiceModels;
using System.Runtime.InteropServices;

namespace ExpenseTracker.Services.AutoMapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<TblUser, UserDto>();
			CreateMap<UserDto, TblUser>()
				.ForMember(x => x.InternalUserId, opt => opt.Ignore())
				.ForMember(x => x.EntryDate, opt => opt.Ignore())
				.ForMember(x => x.EntryId, opt => opt.Ignore())
			   .ForMember(x => x.UserPassword, opt => opt.Ignore());
			CreateMap<TblUser, UserClaims>().ReverseMap();
			CreateMap<TblMenu, SP_MENUROLE_ACCESS_Result>().ReverseMap();
			CreateMap<TblMenuAccess, MenuAccess>().ReverseMap();
			CreateMap<TblBusinessUnit, BusinessUnitRequestDto>().ReverseMap();
			CreateMap<TblBankMaster, BankMasterDto>();
            CreateMap<TblAuditTrail, AuditTrailDto>().ReverseMap();
            CreateMap<BankMasterDto, TblBankMaster>()
				.ForMember(x => x.Id, opt => opt.Ignore())
				.ForMember(x => x.EntryDate, opt => opt.Ignore())
				.ForMember(x => x.EntryBy, opt => opt.Ignore());
			CreateMap<TblRole, RoleDto>().ReverseMap();
			CreateMap<TblCustomReportMaster, CustomReportMasterDto>().ReverseMap();
			CreateMap<TblLocation, LocationDTO>().ReverseMap();
			CreateMap<TblDepartment, DepartmentDto>().ReverseMap();

			//plan master
			CreateMap<PlanMaster, PlanMasterDto>();
			CreateMap<PlanMasterDto, PlanMaster>().ForMember(x => x.CreatedDate, opt => opt.Ignore());
			CreateMap<PlanMaster, PlanMasterDto>();
			CreateMap<PlanMasterDto, PlanMaster>().ForMember(x => x.CreatedDate, opt => opt.Ignore());

			//client master
			CreateMap<ClientMaster, ClientMasterResponseDto>();
			CreateMap<ClientMasterRequestDto, ClientMaster>()
				.ForMember(x => x.CreatedDate, opt => opt.Ignore())
				.ForMember(x => x.AadharAttachmentPath, opt => opt.Ignore())
				.ForMember(x => x.ProfileImageAttachmentPath, opt => opt.Ignore())
				.ForMember(x => x.PanAttachmentPath, opt => opt.Ignore());

			//subscription master
			CreateMap<ClientMaster,ClientWithSubscriptionDto>();
            CreateMap<ClientMaster, ClientWithInvestmentDto>();
            CreateMap<TblSubscription, SubscriptionResponseDto>();

			CreateMap<AddSubscriptionDto, TblSubscription>().ReverseMap();
			CreateMap<PaymentScheduleDto, TblPaymentSchedule>().ReverseMap();

			CreateMap<TblPaymentSchedule, PaymentScheduleDto>().ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.ClientMaster != null ? src.ClientMaster.Name : null));
			CreateMap<SubPlansMasterRequestDto, SubPlansMaster>();
			CreateMap<SubPlansMaster, SubPlansMasterResponseDto>();
			CreateMap<SubPlansMaster, SubPlansMaster>();

            CreateMap<BusinessDevTeam, BusinessDevTeamDTO>().ReverseMap();
			CreateMap<ClientBankingDetail, ClientBankingDetailsDto>().ReverseMap();

            CreateMap<SplPlanMaster,SplPlanMasterDto>().ReverseMap();
            CreateMap<Investment, InvestmentsDto>().ReverseMap();
            CreateMap<InvestmentReceivedDetails, InvestmentReceivedDetailDto>().ReverseMap();
            CreateMap<ClientBankingDetail, ClientBankingDetailsDto>().ReverseMap();
        }

    }
}
