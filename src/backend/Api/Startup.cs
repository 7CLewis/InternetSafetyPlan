using InternetSafetyPlan.Api.Middleware;
using InternetSafetyPlan.Application.Base;
using InternetSafetyPlan.Infrastructure.Base;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace InternetSafetyPlan.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        ConfigureDatabaseServices(services);
        ConfigureControllerServices(services);
        ConfigureAuthenticationServices(services);
        ConfigureAuthorizationServices(services);
        ConfigureHttpServices(services);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseAuthentication();

        app.UseRouting();

        app.UseAuthorization();

        app.UseCors();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private void ConfigureAuthenticationServices(IServiceCollection services)
    {
        var authenticationConfiguration = _configuration.GetSection("Authentication");

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authenticationConfiguration.GetValue<string>("Authority");
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = true,
                    ValidAudience = authenticationConfiguration.GetValue<string>("Audience"),
                    ValidateIssuer = true,
                    ValidIssuer = authenticationConfiguration.GetValue<string>("Authority"),
                    ValidateLifetime = true,
                    ClockSkew = new TimeSpan(0, 0, 30)
                };
                options.RequireHttpsMetadata = false;
            });
    }

    private static void ConfigureAuthorizationServices(IServiceCollection services)
    {
        services.AddAuthorization();
    }

    private static void ConfigureControllerServices(IServiceCollection services)
    {
        var apiAssembly = typeof(AssemblyReference).Assembly;
        services
            .AddControllers()
            .AddNewtonsoftJson()
            .AddApplicationPart(apiAssembly);

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter bearer token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
        });

        var applicationAssembly = typeof(Application.AssemblyReference).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));

        services.AddTransient<ExceptionHandlingMiddleware>();

        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingPipelineBehavior<,>));

        services.RegisterMapsterConfiguration();
    }
    private static void ConfigureHttpServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private void ConfigureDatabaseServices(IServiceCollection services)
    {
        services.AddDbContextPool<DatabaseContext>(
            options => options.UseSqlServer(_configuration.GetConnectionString("InternetSafetyPlan"))
        );

        services.AddScoped<IDatabaseContext, DatabaseContext>();
    }
}
