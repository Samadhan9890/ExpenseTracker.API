//*** System Define Classes and Namespace ***//
using Microsoft.EntityFrameworkCore;
using Serilog;

//*** User Define Classes and Namespace ***//
using ExpenseTracker.Services.Data;
using ExpenseTracker.Services.Middleware;
using ExpenseTracker.Services.StartUpServices;
using ExpenseTracker.Services.AutoMapper;
using ExpenseTracker.Services.Models.DTOs;
using ExpenseTracker.Services.Utilities.ConstantHelper;
using Microsoft.Extensions.Azure;
using ExpenseTracker.Services.Models;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<ResponseDto>();
builder.Services.AddDbContext<AppDBContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection")));
builder.AddAppAuthentication();
builder.Services.JwtServices();
builder.Services.RegisterServices();
builder.Services.AddUserServices();
builder.Services.AddMasterServices();
builder.Services.AuthenticationServices();
builder.Services.AddMenuService();
builder.Services.AddReportService();
builder.Services.AddAllRequiredServices();
builder.Services.AddStorageProcessingServices();
builder.Services.AddFeatureManagement();
builder.Host.UseSerilog((context, configuration) =>     
                configuration.ReadFrom.Configuration(context.Configuration));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<FileUploadOperation>();
});
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("MSSQLConnection"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddAzureClients(client =>
{
	client.AddBlobServiceClient(builder.Configuration.GetConnectionString("AzStorageConStr"));
});

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: ConstantHelper.cors_react,
					  policy =>
					  {
						  policy.WithOrigins(builder.Configuration.GetValue<string>("CORS").Split(";")).AllowAnyHeader().AllowAnyMethod();						  
					  });
});

//builder.Services.Configure<CommunicationConfigOptions>(builder => builder.con)

builder.Services.Configure<CommunicationConfigOptions>(
				builder.Configuration.GetSection("CommunicationConfig"));

if(builder.Environment.EnvironmentName != "Development")
{
	builder.Services.AddApplicationInsightsTelemetry();
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
});
app.MapHealthChecks("/health");

app.UseCors(ConstantHelper.cors_react);
app.UseSerilogRequestLogging();
app.UseExceptionMiddleware();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
ApplyMigration();
app.Run();


//Apply migrations if any
void ApplyMigration()
{
	using (var scope = app.Services.CreateScope())
	{
		var _db = scope.ServiceProvider.GetRequiredService<AppDBContext>();

		if (_db.Database.GetPendingMigrations().Count() > 0)
		{
			_db.Database.Migrate();
		}
	}
}