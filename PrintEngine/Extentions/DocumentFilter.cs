using Microsoft.OpenApi.Models;
using PrintEngine.Core.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.ComponentModel;
using System.Reflection;

namespace PrintEngine.Extentions
{
    /// <summary>
    /// RemoveSchemasFilter
    /// </summary>
    public class DocumentFilter
        : IDocumentFilter
    {
        private static readonly (string, string, Type)[] VisibleSchemas = 
        {
            ("PrintResult", "Результат операции", typeof(PrintResult)),
            ("PrintData", "Данные печатной формы", typeof(PrintData)), 
            ("SignData", "Данные ЭЦП", typeof(SignData)),
            ("TemplateResult", "Данные о реализованных ПФ", typeof(TemplateResult)),
        };
        /// <summary>
        /// Apply(OpenApiDocument swaggerDoc)
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (var key in swaggerDoc.Components.Schemas.Keys
                .Where(k => VisibleSchemas.Any(v => v.Item1 == k)))
            {
                var s = context.SchemaRepository.Schemas[key];
                var vs = VisibleSchemas.First(v => v.Item1 == key);
                s.Description = vs.Item2;

                var properties = vs.Item3.GetProperties()
                    .ToDictionary(p => p.Name.ToLower(), p => p.GetCustomAttributes()
                        .FirstOrDefault(a => a is DescriptionAttribute));

                foreach (var p in s.Properties)
                {
                    if (properties.TryGetValue(p.Key.ToLower(), out var description))
                    {
                        if (description is DescriptionAttribute da)
                            p.Value.Description = da.Description;
                    }
                }
            }


        }
        
    }
}
