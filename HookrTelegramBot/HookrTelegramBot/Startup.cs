using HookrTelegramBot.Operations;
using HookrTelegramBot.Utilities;
using HookrTelegramBot.Utilities.Telegram.Selectors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Telegram.Bot;

namespace HookrTelegramBot
{
    public class Startup
    {
        private const string AzureTableStorage = "AzureTableStorage";
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers();
            services
                .AddOperations()
                .AddUtilities();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                    {
#if DEBUG
                        return context.Response.WriteAsync("Bruh");
#endif
#if !DEBUG
                        context.Response.Redirect("https://t.me/HookrBot");
                        return Task.CompletedTask;
#endif
                    });
                endpoints.MapControllers();
            });
            
            var log = new LoggerConfiguration()
                .WriteTo
                .Console()
                .CreateLogger();
            Log.Logger = log;
        }
    }
}