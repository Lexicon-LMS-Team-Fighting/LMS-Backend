using LMS.API.Filters;
using LMS.Infractructure.Data;
using LMS.Infractructure.Repositories;
using LMS.Presentation;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace LMS.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            //ToDo: Restrict access to your BlazorApp only!
            options.AddDefaultPolicy(policy =>
            {
                //..
                //..
                //..
            });

            //Can be used during development
            options.AddPolicy("AllowAll", p =>
               p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
        });
    }

    public static void ConfigureOpenApi(this IServiceCollection services) =>
       services.AddEndpointsApiExplorer()
               .AddSwaggerGen(setup =>
               {
                   setup.EnableAnnotations();

                   setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                   {
                       In = ParameterLocation.Header,
                       Description = "Place to add JWT with Bearer",
                       Name = "Authorization",
                       Type = SecuritySchemeType.Http,
                       Scheme = "Bearer"
                   });

                   setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                   {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                   });
               });

    public static void ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(opt =>
        {
            opt.ReturnHttpNotAcceptable = true;
            opt.Filters.Add(new ProducesAttribute("application/json"));
            opt.Filters.Add<ValidationFilter>();
        })
            .AddNewtonsoftJson()
            .AddApplicationPart(typeof(AssemblyReference).Assembly);
    }

    public static void ConfigureValidationFilter(this IServiceCollection services)
    {
        // Register ValidationFilter dependencies and suppress default model state invalid filter
        // so that our custom ValidationFilter handles validation errors consistently.
        services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
    }

    public static void ConfigureSql(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();

		// ToDo: TestService injection is here for testing purposes only, should be removed when no longer needed.
		services.AddScoped<ITestService, TestService>();
        services.AddScoped(provider => new Lazy<ITestService>(() => provider.GetRequiredService<ITestService>()));

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped(provider => new Lazy<IAuthService>(() => provider.GetRequiredService<IAuthService>()));

		services.AddScoped<IUserService, UserService>();
		services.AddScoped(provider => new Lazy<IUserService>(() => provider.GetRequiredService<IUserService>()));
		
        services.AddScoped<ICourseService, CourseService>();
		services.AddScoped(provider => new Lazy<ICourseService>(() => provider.GetRequiredService<ICourseService>()));

        services.AddScoped<IModuleService, ModuleService>();
        services.AddScoped(provider => new Lazy<IModuleService>(() => provider.GetRequiredService<IModuleService>()));

        services.AddScoped<ILMSActivityService, LMSActivityService>();
        services.AddScoped(provider => new Lazy<ILMSActivityService>(() => provider.GetRequiredService<ILMSActivityService>()));

        services.AddScoped<IActivityTypeService, ActivityTypeService>();
        services.AddScoped(provider => new Lazy<IActivityTypeService>(() => provider.GetRequiredService<IActivityTypeService>()));
    }
}
