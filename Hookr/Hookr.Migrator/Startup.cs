using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hookr.Core.Config;
using Hookr.Core.Config.Telegram;
using Hookr.Core.Repository.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hookr.Migrator
{
    public class Startup
    {
        private readonly ICoreApplicationConfig applicationConfig;
        public Startup(IConfiguration configuration)
        {
            var config = new CoreApplicationConfig();
            configuration.Bind(config);
            applicationConfig = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDbContext<HookrContext>(
                    builder => builder
                        .UseHookrCoreConfig(applicationConfig.Database)
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            HookrContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapFallback(x =>
                {
                    x.Response.Redirect("/");
                    return Task.CompletedTask;
                });
                endpoints
                    .MapGet("/", context => context.Response.WriteAsync("Nope, that's not a web application"));
            });
        }
    }
}