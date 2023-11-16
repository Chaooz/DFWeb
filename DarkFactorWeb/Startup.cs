using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger; 
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Hosting;


using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Repository.Database;
using DarkFactorCoreNet.Api;
using DarkFactorCoreNet.ConfigModel;
using DarkFactorCoreNet.Provider;
using DFCommonLib.Utils;
using DFCommonLib.Config;
using DFCommonLib.DataAccess;

[assembly: ApiController]
namespace DarkFactorCoreNet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc( options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddSwaggerGen();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddOptions();
            services.AddHttpContextAccessor();
            //services.Configure<DatabaseConfig>(Configuration.GetSection("DatabaseConfigModel"));
            //services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));

            //DFServices.Create(services);
            services.AddTransient<IConfigurationHelper, ConfigurationHelper<Customer> >();

            services.AddScoped<IDbConnectionFactory, LocalMysqlConnectionFactory>();
            services.AddScoped<IDBPatcher, MySQLDBPatcher>();
            //services.BuildServiceProvider();

            //new DFServices(services)
            //        .SetupMySql();

            services.AddSingleton(typeof(IEmailConfiguration), typeof(EmailConfiguration));

            services.AddScoped(typeof(IMenuProvider), typeof(MenuProvider));
            services.AddScoped(typeof(IPageProvider), typeof(PageProvider));
            services.AddScoped(typeof(IEditPageProvider), typeof(EditPageProvider));
            services.AddScoped(typeof(ILoginRepository), typeof(LoginRepository));

            services.AddScoped(typeof(ILoginProvider), typeof(LoginProvider));
            services.AddScoped(typeof(IUserSessionProvider), typeof(UserSessionProvider));
            services.AddScoped(typeof(IEmailProvider), typeof(EmailProvider));

            services.AddScoped(typeof(IMenuRepository), typeof(MenuRepository));
            services.AddScoped(typeof(IPageRepository), typeof(PageRepository));
            services.AddScoped(typeof(IDFDatabase), typeof(DFDataBase));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            //app.UseHttpsRedirection();
            //app.UseRouting();

            app.UseStaticFiles();


            // Enable Swagger middleware 
            app.UseSwagger(); 

            // specify the Swagger JSON endpoint.
            /*app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            */

            app.UseSession();  
            app.UseMvc();
        }
    }
}
