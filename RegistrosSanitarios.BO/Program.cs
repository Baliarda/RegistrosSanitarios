using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace RegistrosSanitarios.BO
{
   
    public class Program
    {
        public static void Main(string[] args)
        {

            var webHost = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                  {
                      var env = hostingContext.HostingEnvironment;
                      //config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                      config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                      config.AddEnvironmentVariables();                      

                      Log.Logger = new LoggerConfiguration()
                          .ReadFrom.Configuration(config.Build())                      
                          .CreateLogger();
                  })
              .ConfigureLogging((hostingContext, logging) =>
              {
                  logging.AddSerilog(dispose: true);
              })
              .UseStartup<Startup>()
              .Build();

            webHost.Run();                   
            
        }

       
    }

}
