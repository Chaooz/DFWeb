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


using DFCommonLib.Utils;
using DFCommonLib.DataAccess;
using DFCommonLib.Logger;

[assembly: ApiController]
namespace DarkFactorCoreNet
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            IDFLogger<Startup> logger = new DFLogger<Startup>();
            logger.Startup(Program.AppName, Program.AppVersion);

            // Run database script
            IStartupDatabasePatcher startupRepository = DFServices.GetService<IStartupDatabasePatcher>();
            startupRepository.WaitForConnection();
            startupRepository.RunPatcher();

            // Setup logger to database
            //DFServices.GetService
//                    .LogToMySQL(DFLogLevel.WARNING)

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
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseSession();  
            app.UseMvc();
        }
    }
}
