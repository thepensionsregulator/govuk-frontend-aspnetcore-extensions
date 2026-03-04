using GovUk.Frontend.AspNetCore.Extensions.Validation;
using System.Reflection;
using ThePensionsRegulator.Umbraco.Core.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;

namespace ThePensionsRegulator.GovUk.Frontend.Umbraco.PropertyEditors.ModelPropertyPicker
{
    /// <summary>
    /// Gets C# property names associated with a document type by looking for the ModelTypeAttribute 
    /// on the Index action method of a controller with the same name as the document type alias.
    /// </summary>
    public class ModelTypeAttributeModelPropertyProvider : IModelPropertyProvider
    {
        /// <inheritdoc/>
        public IEnumerable<string> GetPropertyNames(string documentTypeAlias)
        {
            var filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            List<Type> controllers = new();

            var files = Directory.GetFiles(filepath, "*.dll").ToList();

            files.RemoveAll(x => x.Contains(@"\System.") || x.Contains(@"\Microsoft.")); // Don't load dlls where there won't be any custom code. This list is not exhaustive

            foreach (var file in files)
            {
                Assembly assembly = Assembly.LoadFrom(file);
                List<Type> controllersToAdd = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(RenderController))).ToList();
                controllers.AddRange(controllersToAdd);
            }
            var controllerType = controllers?.FirstOrDefault(x => x.Name.ToUpperInvariant() == $"{documentTypeAlias.ToUpperInvariant()}CONTROLLER");
            if (controllerType != null)
            {
                var actionMethods = controllerType.GetMethods().Where(x => x.Name == "Index");
                foreach (var method in actionMethods)
                {
                    var modelType = (method?.GetCustomAttributes(typeof(ModelTypeAttribute), false).SingleOrDefault() as ModelTypeAttribute)?.ModelType;
                    if (modelType != null)
                    {
                        return modelType.GetProperties().Where(x =>
                            !x.PropertyType.IsSubclassOf(typeof(PublishedContentModel)) &&
                            !x.PropertyType.IsAssignableTo(typeof(OverridableBlockListModel))
                            ).Select(x => x.Name);
                    }
                }
            }

            return Array.Empty<string>();
        }
    }
}