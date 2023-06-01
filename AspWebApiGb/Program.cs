using System.Net;
using System.Text;
using AspWebApiGb.Models.Dto;
using AspWebApiGb.Models.Requests;
using AspWebApiGb.Models.Validators;
using AspWebApiGb.Services;
using AspWebApiGb.Services.GRPC;
using AspWebApiGb.Services.Impl;
using EmployeeServiceData;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

var logger = LogManager
    .Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

logger.Debug("Init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.WebHost.ConfigureKestrel(options =>
    {
        options.Listen(IPAddress.Any, 5001, listenOptions =>
        {
            listenOptions.Protocols = HttpProtocols.Http2;
        });
    });
    
    #region Configure Repositories

    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
    builder.Services.AddScoped<IEmployeeTypeRepository, EmployeeTypeRepository>();
    builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

    #endregion

    #region Configure EF DBContext Service (EmployeeDatabase Database)

    builder.Services.AddDbContext<EmployeeServiceDbContext>(
        options =>
        {
            string connectionString = 
                "server=localhost;port=3306;database=EmployeeDatabase;user id=root;password=Poi132poi_;";

            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        });

    #endregion

    #region Configure gRPC
    
    builder.Services.AddGrpc();

    #endregion
    
    #region Configure Auth

    builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(AuthService.SecretKey)
                    ),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        });
    
    builder.Services.AddSingleton<IAuthService, AuthService>();

    #endregion
    
    #region Configure Logging

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    #endregion

    #region Configure Fluent Validator

    builder.Services.AddScoped<IValidator<AuthRequest>, AuthRequestValidator>();
    builder.Services.AddScoped<IValidator<AccountDto>, AccountDtoValidator>();
    builder.Services.AddScoped<IValidator<string>, PasswordValidator>();
    builder.Services.AddScoped<AbstractValidator<EmployeeDto>, EmployeeValidator>();

    #endregion
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo()
        {
            Title = "Employee Service", Version = "v1"
        });
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
        {
            Description = "JWT Auth header using the Bearer scheme (Example: 'Bearer E3Fn1mFoSf')",
            Name = HeaderNames.Authorization,
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        c.AddSecurityRequirement(new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                },
                Array.Empty<string>()
            }
        });
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseRouting();
    
    app.UseAuthentication();
    
    app.UseAuthorization();
    
    app.MapControllers();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapGrpcService<DictionaryService>();
        endpoints.MapGrpcService<DepartmentService>();
    });
    
    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}