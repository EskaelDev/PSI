using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SyllabusManager.Data;
using SyllabusManager.Data.ProviderContexts;
using SyllabusManager.Logic.Interfaces;
using SyllabusManager.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.API.Helpers
{
    public static class InjectionHelper
    {
        public static void Inject(IServiceCollection services, IConfiguration configuration)
        {
            SetDB(services, configuration);
            SetSettings(services, configuration);
            SetServicesDI(services);

        }

        private static void SetDB(IServiceCollection services, IConfiguration configuration)
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

        private static void SetSettings(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<Logic.Settings.AuthorizationOptions>(configuration.GetSection("AuthOptions"));
        }

        private static void SetServicesDI(IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IUserService, UserService>();
        }
    }
}
