using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Hookr.Web.Backend
{
    public class Program
    {
        public static Task Main(string[] args) 
            => Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseSerilog((context, configuration) => 
                    configuration
                        .WriteTo.Console())
                .Build()
                .RunAsync();
    }
}