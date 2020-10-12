using System.Reflection;
using Hookr.Core;
using Hookr.Core.Config;
using Hookr.Core.Config.Telegram;
using Hookr.Core.Filters;
using Hookr.Core.Repository;
using Hookr.Core.Repository.Context;
using Hookr.Telegram.Config;
using Hookr.Telegram.Config.Telegram;
using Hookr.Telegram.Filters;
using Hookr.Telegram.Operations;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Utilities;
using Hookr.Telegram.Utilities.Telegram.Bot;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Hookr.Telegram
{
    public class Startup
    {
        private readonly IApplicationConfig applicationConfig;
        private readonly IExtendedTelegramConfig extendedTelegramConfig;

        public Startup(IConfiguration configuration)
        {
            var config = new ApplicationConfig();
            var telegramConfig = new ExtendedTelegramConfig();
            configuration.Bind(config);
            configuration.GetSection(nameof(CoreApplicationConfig.Telegram)).Bind(telegramConfig);
            applicationConfig = config;
            extendedTelegramConfig = telegramConfig;
        }

        //private const string AzureTableStorage = "AzureTableStorage";
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddScoped<GrabbingCurrentTelegramUpdateFilterAttribute>()
                .AddControllers()
                .AddNewtonsoftJson();
            services
                .AddHookrCore(typeof(Startup).Assembly)
                .AddConfig(applicationConfig)
                .AddSingleton(extendedTelegramConfig)
                .AddHttpClient()
                .AddMemoryCache()
                .AddDbContext<HookrContext>(
                    builder => builder
                        .UseHookrCoreConfig(applicationConfig.Database)
                )
                .AddOperations()
                .AddScoped<ITelegramHookrRepository, TelegramHookrRepository>()
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
                    return context.Response.WriteAsync("Bruh, here we go again.");
#endif
#if !DEBUG
                    context.Response.Redirect("https://t.me/HookrBot");
                    return Task.CompletedTask;
#endif
                });
                endpoints.MapControllers();
            });
        }
    }
}