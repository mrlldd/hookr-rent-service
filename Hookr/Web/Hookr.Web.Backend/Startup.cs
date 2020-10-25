using System;
using System.Text.Json;
using Hookr.Core;
using Hookr.Core.Config;
using Hookr.Core.Repository.Context;
using Hookr.Core.Utilities.Extensions;
using Hookr.Web.Backend.Config;
using Hookr.Web.Backend.Filters.Response;
using Hookr.Web.Backend.Middleware;
using Hookr.Web.Backend.Operations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Hookr.Web.Backend
{
    public class Startup
    {
        private readonly ICoreApplicationConfig coreApplicationConfig;
        private readonly IJwtConfig jwtConfig;

        public Startup(IConfiguration configurationRoot)
        {
            coreApplicationConfig = new CoreApplicationConfig()
                .SideEffect(configurationRoot.Bind);
            jwtConfig = new JwtConfig()
                .SideEffect(configurationRoot.GetSection("Jwt").Bind);
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = jwtConfig.Audience,
                        ValidateAudience = true,
                        ValidIssuer = jwtConfig.Issuer,
                        ValidateIssuer = true,
                        IssuerSigningKey = new SymmetricSecurityKey(jwtConfig.Key.Utf8Bytes()),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services
                .AddHttpContextAccessor()
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.IgnoreNullValues = true;
                });
            services
                .AddHookrCore(typeof(Startup).Assembly, coreApplicationConfig);
            services
                .AddSingleton(jwtConfig)
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
            app.UseAuthentication();
            app.UseMiddleware<JwtReaderMiddleware>();
            app.UseAuthorization();
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