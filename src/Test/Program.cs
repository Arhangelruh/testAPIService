using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var isService = false;

            if (Debugger.IsAttached == false && args.Contains("--service"))
            {
                isService = true;
            }

            if (isService)
            {
                var pathToContentRoot = Directory.GetCurrentDirectory();

                string configurationFile = "appsettings.json";
                string portNumber = "5004";

                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                pathToContentRoot = Path.GetDirectoryName(pathToExe);

                string appJsonFilePath = Path.Combine(pathToContentRoot, configurationFile);

                if (File.Exists(appJsonFilePath))
                {
                    StreamReader sr = new StreamReader(appJsonFilePath);
                    string jsonData = sr.ReadToEnd();
                    JObject jObject = JObject.Parse(jsonData);

                    if (jObject["ServicePort"] != null)
                    {
                        portNumber = jObject["ServicePort"].ToString();
                    }
                }

                var host = Host.CreateDefaultBuilder(args)
                   .UseContentRoot(pathToContentRoot)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseStartup<Startup>()
                       .UseUrls("http://localhost:" + portNumber);
                   })
                   .UseWindowsService()
                   .Build();
                host.Run();

            }
            else
            {
                CreateHostBuilder(args).Build().Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseWindowsService();
    }
}
