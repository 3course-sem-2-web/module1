using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace webapi.Swashbuckle
{
    public static class CustomSwaggerUIExtensions
    {
        public static void AddUIButtonBearerTokenAuthToSwagger(this SwaggerGenOptions c)
        {
            var newLine = string.Concat(Enumerable.Repeat(Environment.NewLine, 2));
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                BearerFormat = "JWT",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Description = $"JWT Authorization header using the Bearer scheme. {newLine} Enter 'Bearer' [space] and then your token in the text input below.{newLine}Example: \"Bearer 1safsfsdfdfd\"",
            });

            /*
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
            */

            c.OperationFilter<AuthResponsesOperationFilter>();
        }
    }
}
