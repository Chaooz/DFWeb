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

using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Repository.Database;
using DarkFactorCoreNet.Api;
using DarkFactorCoreNet.ConfigModel;
using DarkFactorCoreNet.Provider;

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
            services.AddMvc();

            // register the swagger generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

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
            services.Configure<DatabaseConfig>(Configuration.GetSection("DatabaseConfigModel"));
            services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));

            services.AddSingleton(typeof(IEmailConfiguration), typeof(EmailConfiguration));

            services.AddScoped(typeof(IMenuProvider), typeof(MenuProvider));
            services.AddScoped(typeof(IPageProvider), typeof(PageProvider));
            services.AddScoped(typeof(IEditPageProvider), typeof(EditPageProvider));
            services.AddScoped(typeof(ILoginRepository), typeof(LoginRepository));

            services.AddScoped(typeof(ILoginProvider), typeof(LoginProvider));
            services.AddScoped(typeof(IUserSessionProvider), typeof(UserSessionProvider));
            services.AddScoped(typeof(IEmailProvider), typeof(EmailProvider));

            services.AddSingleton(typeof(IMenuRepository), typeof(MenuRepository));
            services.AddSingleton(typeof(IPageRepository), typeof(PageRepository));
            services.AddSingleton(typeof(IDFDatabase), typeof(DFDataBase));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            //app.UseHttpsRedirection();


            // Enable Swagger middleware 
            app.UseSwagger(); 

            // specify the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            }); 

            app.UseSession();  
            app.UseMvc();
        }
    }
}
