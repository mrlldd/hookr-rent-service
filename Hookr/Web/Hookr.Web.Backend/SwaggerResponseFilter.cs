using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Hookr.Web.Backend.Filters.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Hookr.Web.Backend
{
    public class SwaggerResponseFilter : IOperationFilter
    {
        private const string ContentType = "application/json";

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var responses = operation.Responses;
            responses.Clear();

            responses.Add("200", new OpenApiResponse
            {
                Description = "Successful response",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        ContentType, new OpenApiMediaType
                        {
                            Schema = context.SchemaGenerator.GenerateSchema(GetCorrectReturnType(),
                                context.SchemaRepository)
                        }
                    }
                }
            });

            responses.Add("500", new OpenApiResponse
            {
                Description = "Error response",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        ContentType, new OpenApiMediaType
                        {
                            Schema = context.SchemaGenerator.GenerateSchema(typeof(Error),
                                context.SchemaRepository)
                        }
                    }
                }
            });

            if (context.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>() == null)
            {
                responses.Add("401", new OpenApiResponse
                {
                    Description = "Not authorized"
                });
            }

            Type GetCorrectReturnType()
            {
                static Type GenericResponse(Type type) => typeof(Success<>).MakeGenericType(type);
                var methodInfo = context.MethodInfo;
                var returnType = methodInfo.ReturnType;
                return returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>)
                    ? GenericResponse(returnType.GetGenericArguments()[0])
                    : returnType != typeof(Task)
                        ? GenericResponse(returnType)
                        : typeof(Success);
            }
        }
    }
}