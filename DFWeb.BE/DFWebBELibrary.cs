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
using AccountClientModule.Client;

// dotnet new classlib -n DFWeb.BE -f net7.0

namespace DFWeb.BE
{
    public class DFWebBELibrary
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            //services.Configure<DatabaseConfig>(Configuration.GetSection("DatabaseConfigModel"));
            //services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));

            // Create Logger + service
            DFServices.Create(services);
            new DFServices(services)
                .SetupLogger()
                .SetupMySql()
                .LogToConsole(DFLogLevel.INFO)
                ;

            AccountClient.SetupService(services);
        }
    }
}
