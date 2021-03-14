using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace DemoMasiv.Config.Api.Class
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if(operation.Parameters == null) { operation.Parameters = new List<OpenApiParameter>(); }
            operation.Parameters.Add(new OpenApiParameter 
            {
                Name = "UserId",
                In = ParameterLocation.Header,
                Description = "Id de usuario Autenticado",
                Required = false
            });
        }
    }
}
