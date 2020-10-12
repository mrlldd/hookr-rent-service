using Hookr.Core.Config;
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
        private readonly ICoreApplicationConfig coreApplicationConfig;

        public Startup(IConfiguration configurationRoot)
        {
            this.configurationRoot = configurationRoot;
            coreApplicationConfig = new CoreApplicationConfig();
            configurationRoot.Bind(coreApplicationConfig);
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(x => x.Filters.Add(new ResponseFilter()))
                .AddJsonOptions(options => { options.JsonSerializerOptions.WriteIndented = true; });
                
            services
                .AddConfig(coreApplicationConfig)
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