using Hookr.Telegram.Filters;
using Hookr.Telegram.Operations;
using Hookr.Telegram.Repository;
using Hookr.Telegram.Repository.Context;
using Hookr.Telegram.Utilities;
using Hookr.Telegram.Utilities.App.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Hookr.Telegram
{
    public class Startup
    {
        private readonly IConfigurationRoot configurationRoot;
        private readonly IHostEnvironment hostEnvironment;

        public Startup(IConfiguration configuration, IHostEnvironment hostEnvironment)
        {
            configurationRoot = new ConfigurationBuilder()
                .AddConfiguration(configuration)
                .AddEnvironmentVariables()
                .Build();
            this.hostEnvironment = hostEnvironment;
        }

        //private const string AzureTableStorage = "AzureTableStorage";
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var log = new LoggerConfiguration()
                .WriteTo
                .Console()
                .CreateLogger();
            Log.Logger = log;
            var appSettings = new AppSettings();
            configurationRoot.Bind(appSettings);

            services.AddSingleton<IAppSettings>(appSettings);

            services
                .AddScoped<GrabbingCurrentTelegramUpdateFilterAttribute>()
                .AddControllers()
                .AddNewtonsoftJson();
            services
                .AddHttpClient()
                .AddMemoryCache()
                .AddDbContext<HookrContext>(
                    builder => builder.UseSqlServer(appSettings.Database.ConnectionString)
                )
                .AddOperations()
                .AddRepositories()
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