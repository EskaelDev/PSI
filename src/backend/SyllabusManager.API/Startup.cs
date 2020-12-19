using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Data.ProviderContexts;
using System;
using Microsoft.EntityFrameworkCore;

namespace SyllabusManager.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // cors
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SyllabusManager.API", Version = "v1" });
            });

            services.AddIdentity<SyllabusManagerUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<SyllabusManagerDbContext>()
                .AddDefaultTokenProviders();

            switch (Configuration.GetValue<string>("DatabaseProvider"))
            {
                case "SqlServer":
                    services.AddDbContext<SyllabusManagerDbContext, SqlServerSyllabusManagerDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SMDatabase")));
                    break;
                case "Oracle":
                    services.AddDbContext<SyllabusManagerDbContext, OracleSyllabusManagerDbContext>(options => options.UseOracle(Configuration.GetConnectionString("SMDatabase")));
                    break;
                case "Postgres":
                    services.AddDbContext<SyllabusManagerDbContext, PostgresSyllabusManagerDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("SMDatabase")));
                    break;
                default:
                    throw new Exception("No valid database provider! Available options: SqlServer, Oracle, Postgres.");
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SyllabusManager.API v1"));
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
