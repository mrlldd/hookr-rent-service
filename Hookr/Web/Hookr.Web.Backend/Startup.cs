using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Web.Backend.Config;
using Hookr.Web.Backend.Filters.Response;
using Hookr.Web.Backend.Operations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hookr.Web.Backend
{
    public class Startup
    {
        private readonly IConfiguration configurationRoot;
        private readonly IApplicationConfig applicationConfig;

        public Startup(IConfiguration configurationRoot)
        {
            this.configurationRoot = configurationRoot;
            applicationConfig = new ApplicationConfig();
            configurationRoot.Bind(applicationConfig);
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddConfig(applicationConfig);
            services
                .AddControllers(x => x.Filters.Add(new ResponseFilter()))
                .AddJsonOptions(options => { options.JsonSerializerOptions.WriteIndented = true; });
            services
                .AddOperations();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                if (env.IsDevelopment())
                {
                    endpoints.MapGet("/", context => context.Response.WriteAsync("Bruh, here we go again"));
                }

                endpoints.MapControllers();
            });
        }
    }
}