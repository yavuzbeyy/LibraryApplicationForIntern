using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Katmanli.Core.SharedLibrary
{
    public static class Logging
    {
        public static string loggingPath = "C:\\Users\\yavuz\\OneDrive\\Desktop\\VakifbankStaj\\Kütüphane Uygulaması\\Katmanli.API\\wwwroot\\Logs\\";
        public static Action<HostBuilderContext, LoggerConfiguration> ConfigureLogging => (builderContext,
            loggerConfiguration) =>
        {
            var environment = builderContext.HostingEnvironment;

            loggerConfiguration
            .ReadFrom.Configuration(builderContext.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Environment", environment.EnvironmentName)
            .Enrich.WithProperty("AppName", environment.ApplicationName)
            .WriteTo.File(loggingPath + "logLibrary.txt");
        };
    }
}
