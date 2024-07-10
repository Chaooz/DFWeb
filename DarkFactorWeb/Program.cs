using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using DFCommonLib.Logger;
using DFCommonLib.Utils;
using DFCommonLib.Config;
using DFCommonLib.DataAccess;

using DarkFactorCoreNet.Repository;
using DarkFactorCoreNet.ConfigModel;
using DarkFactorCoreNet.Provider;

using AccountClientModule.Client;

namespace DarkFactorCoreNet
{
    public class Program
    {
        public static string AppName = "DarkFactor Web";
        public static string AppVersion = "1.0.0";

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                //services.Configure<DatabaseConfig>(Configuration.GetSection("DatabaseConfigModel"));
                //services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));
                services.AddTransient<IConfigurationHelper, ConfigurationHelper<WebConfig> >();

                // Create Logger + service
                DFServices.Create(services);
                new DFServices(services)
                    .SetupLogger()
                    .SetupMySql()
                    .LogToConsole(DFLogLevel.INFO)
                    ;

                services.AddScoped<IDbConnectionFactory, LocalMysqlConnectionFactory>();

                services.AddTransient<IStartupDatabasePatcher, DFWebDatabasePatcher>();

                services.AddSingleton(typeof(IEmailConfiguration), typeof(EmailConfiguration));

                services.AddScoped(typeof(IMenuProvider), typeof(MenuProvider));
                services.AddScoped(typeof(IPageProvider), typeof(PageProvider));
                services.AddScoped(typeof(IEditPageProvider), typeof(EditPageProvider));
                services.AddScoped(typeof(ILoginRepository), typeof(LoginRepository));

                services.AddScoped(typeof(ILoginProvider), typeof(LoginProvider));
                services.AddScoped(typeof(IUserSessionProvider), typeof(UserSessionProvider));
                services.AddScoped(typeof(IEmailProvider), typeof(EmailProvider));
                services.AddScoped(typeof(IImageProvider), typeof(ImageProvider));

                services.AddScoped(typeof(IMenuRepository), typeof(MenuRepository));
                services.AddScoped(typeof(IPageRepository), typeof(PageRepository));
                services.AddScoped(typeof(IEditPageRepository), typeof(EditPageRepository));
                services.AddScoped(typeof(IImageRepository), typeof(ImageRepository));

                services.AddScoped(typeof(ICookieProvider), typeof(CookieProvider));

                AccountClient.SetupService(services);

            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
