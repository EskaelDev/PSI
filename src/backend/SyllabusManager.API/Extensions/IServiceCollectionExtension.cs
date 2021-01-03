using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Data.ProviderContexts;
using SyllabusManager.Logic.Interfaces;
using SyllabusManager.Logic.Models;
using SyllabusManager.Logic.Services;
using System;
using System.Threading.Tasks;

namespace SyllabusManager.API.Extensions
{
    public static class IServiceCollectionExtension
    {
        public static async Task SetRolesAndAccounts(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            UserManager<SyllabusManagerUser> userManager = serviceProvider.GetRequiredService<UserManager<SyllabusManagerUser>>();

            foreach (string roleName in UsersRoles.All)
            {
                bool roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            SyllabusManagerUser admin = new SyllabusManagerUser()
            {
                Name = "admin",
                UserName = "admin",
                Email = "admin@pwr.pl",
            };

            const string adminPassword = "S4#SAX@2WqS?mkr&";
            SyllabusManagerUser user = await userManager.FindByEmailAsync(admin.Email);

            if (user == null)
            {
                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, UsersRoles.Admin);
                }
            }
        }

        public static void SetDB(this IServiceCollection services, IConfiguration configuration)
        {
            switch (configuration.GetValue<string>("DatabaseProvider"))
            {
                case "SqlServer":
                    services.AddDbContext<SyllabusManagerDbContext, SqlServerSyllabusManagerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SMDatabase")));
                    break;
                case "Oracle":
                    services.AddDbContext<SyllabusManagerDbContext, OracleSyllabusManagerDbContext>(options => options.UseOracle(configuration.GetConnectionString("SMDatabase")));
                    break;
                case "Postgres":
                    services.AddDbContext<SyllabusManagerDbContext, PostgresSyllabusManagerDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("SMDatabase")));
                    break;
                default:
                    throw new Exception("No valid database provider! Available options: SqlServer, Oracle, Postgres.");
            }
        }

        public static void SetSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Logic.Settings.AuthorizationOptions>(configuration.GetSection("AuthOptions"));
        }

        public static void SetServicesDI(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFieldOfStudyService, FieldOfStudyService>();
            services.AddTransient<ILearningOutcomeService, LearningOutcomeService>();
        }

        public static void SetAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
               .AddJwtBearer(options =>
               {
                   options.SaveToken = true;
                   options.RequireHttpsMetadata = false;
                   options.TokenValidationParameters = new TokenValidationParameters()
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       IssuerSigningKey = new SymmetricSecurityKey(
                           System.Text.Encoding.UTF8.GetBytes(configuration["AuthOptions:Secret"]))
                   };
               });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }
    }
}
