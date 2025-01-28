using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace WorldsBelly.API.Utilities.Mappers.ActionFilter
{
    public class AuthenticationRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();
            var key = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearer"
                }
            };
            var security = operation.Security;
            var securityRequirement = new OpenApiSecurityRequirement { [key] = (IList<string>)new List<string>() };
            security.Add(securityRequirement);
        }
    }
}
