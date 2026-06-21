using ExpenseTracker.Services.Contracts;
using ExpenseTracker.Services.Contracts.IContracts;
using ExpenseTracker.Services.Contracts.IContracts.BusinessUnitContracts;
using ExpenseTracker.Services.Contracts.IContracts.JWTContracts;
using ExpenseTracker.Services.Contracts.RequestResponseDto;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Repository;
using ExpenseTracker.Services.Repository.Interface;
using ExpenseTracker.Services.Services;
using ExpenseTracker.Services.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using NC.StorageProcessor.Interfaces;
using NC.StorageProcessor.Implementations;
using Microsoft.Extensions.Azure;

namespace ExpenseTracker.Services.StartUpServices
{
    public static class RegisterUserCreatedServices
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme { 
                    BearerFormat = "JWT",
                    Name = "JWt Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Require JWT Tokens",
                    Reference = new OpenApiReference { 
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NCApi", Version = "v1"});
                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {jwtSecurityScheme, Array.Empty<string>() }
                });
            });
            return services;
        }
        public static void JwtServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtUtils, JwtUtils>();
        }
        public static void AddUserServices(this IServiceCollection services) {
            services.AddScoped<IUserFactory, Users>();
        }
        public static void AuthenticationServices(this IServiceCollection services)
        {
            services.AddScoped<IResponseDto, ResponseDto>();
            services.AddScoped<IAuthenticationFactory, AuthenticationFactory>();
        }

        public static void AddMasterServices(this IServiceCollection services)
        {
            services.AddScoped<IBusinessUnit, BusinessUnit>();
			services.AddScoped<IMasterRepository, MasterRepository>();
			services.AddScoped<IBankMasterService, BankMasterServices>();
            services.AddScoped<IRoleMasterService, RoleMasterServices>();
            services.AddScoped<ILocationMasterService, LocationMasterService>();
            services.AddScoped<IDepartmentMasterService, DepartmentMasterService>();
        }

		public static void AddMenuService(this IServiceCollection services)
		{
			services.AddScoped<IMenuServices, MenuServices>();
			services.AddScoped<IMenuRepository, MenuRepository>();
		}

        public static void AddReportService(this IServiceCollection services)
        {
            services.AddScoped<IReportMasterService, ReportMasterServices>();
            services.AddScoped<IReportMasterRepository, ReportMasterRepository>();
        }

        public static void AddAllRequiredServices(this IServiceCollection services)
        {
            services.AddScoped<IPlanMasterService,PlanMasterService>();
			services.AddScoped<IClientMasterService, ClientMasterService>();
			services.AddScoped<IClientMasterRepository, ClientMasterRepository>();
			services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
			services.AddScoped<ISubscriptionService, SubscriptionService>();
			services.AddScoped<IDashboardService, DashboardService>();
			services.AddScoped<IDashboardRepository, DashboardRepository>();
			services.AddScoped<IBusinessDevTeamService, BusinessDevTeamService>();
			services.AddScoped<IBusinessDevTeamRepository, BusinessDevTeamRepository>();
			services.AddScoped<IPaymentProofService, PaymentProofService>();
			services.AddScoped<IPaymentProofRepository, PaymentProofRepository>();
            services.AddScoped<IMessageService, WhatsappMessageService>();
            services.AddScoped<IAuditTrailService, AuditTrailService>();
            services.AddScoped<IAuditTrailRepository, AuditTrailRepository>();
            services.AddScoped<IInvestmentRepository, InvestmentRepository>();
            services.AddScoped<IInvestmentService, InvestmentService>();
            services.AddScoped<IReferralService, ReferralService>();
            services.AddScoped<IReferralsRepository, ReferralsRepository>();
            services.AddScoped<IExpensesService, ExpensesService>();
            services.AddScoped<IExpensesRepository, ExpensesRepository>();

        }

        public static void AddStorageProcessingServices(this IServiceCollection services)
		{
			services.AddScoped<IDocumentProcessor, AzStorageDocumentProcessor>();
		}



	}
}
