using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.Source.Database;
using DarkFactorCoreNet.Controllers;

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

/* 
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DefaultConnection"))
            );
            */

            services.AddScoped(typeof(IMenuCollector), typeof(MenuCollector));
            services.AddScoped(typeof(IPageCollector), typeof(PageCollector));

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
            app.UseMvc();

        }
    }
}
