/* This code is made by Anneshu Nag, Student ID- 2210994760*/
using Microsoft.EntityFrameworkCore.Design;
using HandlebarsDotNet;
public class ScaffoldingDesignTimeServices : IDesignTimeServices
{
    public void ConfigureDesignTimeServices(IServiceCollection services)
    {
        services.AddHandlebarsScaffolding();
        
        var handlebars = Handlebars.Create();

        /* Learnt how to add Hanldebars Helpers from: https://github.com/ErikEJ/EFCorePowerTools/issues/166*/
        
        // Register Handlebars helpers
        // The PascalCase Helper is not really necessary as property names are already coming in PascalCase only but I have added it and used it too just to show how to use helpers 
        services.AddHandlebarsHelpers(
            ("PascalCase-for-property-name", (writer, context, parameters) =>
                writer.Write($"{context["property-name"]}".Replace("_", "").Replace(" ", ""))
            ),
        
        // CamelCase is required for constructor
            ("camelCase-for-constructor-args", (writer, context, parameters) =>
            {
                var camelCase = char.ToLowerInvariant($"{context["property-name"]}"[0]) + $"{context["property-name"]}".Substring(1);
                writer.Write(camelCase);
            })
        );
    }
}