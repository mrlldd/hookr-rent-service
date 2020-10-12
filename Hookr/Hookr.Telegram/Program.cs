using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Hookr.Telegram
{
    public class Program
    {
        public static void Main(string[] args)
            => Host
                .CreateDefaultBuilder(args)
                .UseSerilog((context, configuration) =>
                {
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        configuration
                            .MinimumLevel.Debug();
                    }

                    configuration
                        .ReadFrom
                        .Configuration(context.Configuration)
                        .WriteTo
                        .Console();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build()
                .Run();
    }
}