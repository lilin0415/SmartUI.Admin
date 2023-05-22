using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace YiSha.Admin.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder= WebHost.CreateDefaultBuilder(args)
                     
                       .UseStartup<Startup>()


                       .ConfigureLogging(logging =>
                       {
                           logging.ClearProviders();
                           logging.SetMinimumLevel(LogLevel.Trace);
                       }).UseNLog();

            var config = new ConfigurationBuilder()
                .SetBasePath(builder.GetSetting(WebHostDefaults.ContentRootKey))
                .AddJsonFile("appsettings.json").Build();


            builder.UseConfiguration(config);

            var urls = config.GetSection("Urls")?.Value;
            if (!string.IsNullOrEmpty(urls))
            {
                builder.UseUrls(urls);
            }

            return builder;
        }

         
    }
}
