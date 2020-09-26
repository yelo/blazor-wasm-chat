using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace RetroChat.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseUrls("https://monolith.lan:5001");
                    webBuilder.UseStartup<Startup>();
                });
    }
}