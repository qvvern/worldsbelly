﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
namespace WorldsBelly.API.Utilities.Mappers.ActionFilter
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            //var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
            //var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

            //if (isAuthorized && !allowAnonymous)
            //{
            //    // other things to do if 
            //}

            if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "Language-Id",
                    In = ParameterLocation.Header,
                    Description = "Chosen Languages",
                    Required = false,
                    Schema = new OpenApiSchema
                    {
                        Type = "int",
                        Default = new OpenApiString("20")
                    }
                });
        }
    }
}