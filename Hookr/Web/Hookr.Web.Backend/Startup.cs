using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hookr.Web.Backend.Filters.Response;
using Hookr.Web.Backend.Operations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hookr.Web.Backend
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(x => x.Filters.Add(new ResponseFilter()))
                .AddJsonOptions(options => { options.JsonSerializerOptions.WriteIndented = true; });
            services
                .AddOperations();
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
                if (env.IsDevelopment())
                {
                    endpoints.MapGet("/", context => context.Response.WriteAsync("Bruh, here we go again"));
                }

                endpoints.MapControllers();
            });
        }
    }
}