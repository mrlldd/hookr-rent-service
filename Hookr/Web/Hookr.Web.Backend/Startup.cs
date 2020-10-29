using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hookr.Core;
using Hookr.Core.Config;
using Hookr.Core.Repository.Context;
using Hookr.Core.Utilities.Extensions;
using Hookr.Web.Backend.Config;
using Hookr.Web.Backend.Filters.Response;
using Hookr.Web.Backend.Middleware;
using Hookr.Web.Backend.Operations;
using Hookr.Web.Backend.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
                .AddUtilities()
                .AddSwaggerGen(options =>
                    {
                        options.SwaggerDoc("v1", new OpenApiInfo
                        {
                            Version = "v1",
                            Title = "Hookr Web API "
                        });
                        options.CustomSchemaIds(type =>
                        {
                            string GetTypeName(Type x)
                            {
                                if (x.IsNested)
                                {
                                    return $"{x.DeclaringType?.Name}.{x.Name}";
                                }

                                if (!x.IsGenericType)
                                {
                                    return string.Join(".", x.Namespace, x.Name);
                                }

                                var genericTypeName = x.GetGenericTypeDefinition().Name.Replace("`1", string.Empty);
                                var genericArguments = string.Join(", ", x.GetGenericArguments().Select(GetTypeName));
                                return $"{genericTypeName}<{genericArguments}>";
                            }

                            return GetTypeName(type);
                        });
                        options.OperationFilter<SwaggerResponseFilter>();
                    })
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
                        .AddControllers()
                        .AddJsonOptions(options =>
                        {
                            options.JsonSerializerOptions.WriteIndented = true;
                            options.JsonSerializerOptions.IgnoreNullValues = true;
                            options.JsonSerializerOptions.Converters.Add(
                                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hookr Web API"));

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseMiddleware<JwtReaderMiddleware>();
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