using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection;
using ThePensionsRegulator.Umbraco.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Controllers;
using Umbraco.Extensions;

namespace GovUk.Frontend.Umbraco.Validation
{
    [PluginController("GOVUK")]
    public class ModelPropertyController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public IEnumerable<string> ForDocumentType(string alias)
        {
            var filepath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
           
            List<Type> controllers = new();

            var files = Directory.GetFiles(filepath, "*.dll").ToList();

            files.RemoveAll(x => x.Contains(@"\System.") || x.Contains(@"\Microsoft.")); // Don't load dlls where there won't be any custom code. This list is not exhaustive
            
            foreach(var file in files) 
            { 
                Assembly assembly = Assembly.LoadFrom(file);
                List<Type> controllersToAdd = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(RenderController))).ToList();
                controllers.AddRange(controllersToAdd);
            }
            var controllerType = controllers?.FirstOrDefault(x => x.Name.ToUpperInvariant() == $"{alias.ToUpperInvariant()}CONTROLLER");
            if (controllerType != null)
            {
                var actionMethods = controllerType.GetMethods().Where(x => x.Name == "Index");
                foreach (var method in actionMethods)
                {
                    var modelType = (method?.GetCustomAttributes(typeof(ModelTypeAttribute), false).SingleOrDefault() as ModelTypeAttribute)?.ModelType;
                    if (modelType != null)
                    {
                        var propNames = new List<string>();
                        const int maxDepth = 5;

                        // Track visited types on the current recursion path to avoid infinite loops.
                        var pathStack = new Stack<Type>();

                        CollectProperties(modelType, string.Empty, 0, maxDepth, pathStack, propNames);

                        return propNames;
                    }
                }
            
            }

            return Array.Empty<string>();
        }

        public void CollectProperties(Type type, string prefix, int depth, int maxDepth, Stack<Type> pathStack, List<string> propNames)
        {
            if (type == null) return;
            if (depth >= maxDepth) return;
            if (pathStack.Contains(type)) return; // avoid cycles

            pathStack.Push(type);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propType = property.PropertyType;

                // Skip Umbraco published content and blocklist model types
                if (propType.IsSubclassOf(typeof(PublishedContentModel)) || propType.IsAssignableTo(typeof(OverridableBlockListModel)) || propType.IsAssignableTo(typeof(OverridableBlockGridModel)))
                {
                    continue;
                }

                var fullName = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";

                // Primitive-ish types and string: add the property and stop recursing
                if (propType.IsPrimitive || propType.IsEnum || propType == typeof(string) || propType == typeof(decimal))
                {
                    propNames.Add(fullName);
                    continue;
                }

                // Handle enumerable types (arrays, IList<T>, IEnumerable<T>, etc.)
                if (typeof(IEnumerable).IsAssignableFrom(propType) && propType != typeof(string))
                {
                    var elementType = GetEnumerableElementType(propType);
                    if (elementType == null || elementType == typeof(string) || elementType.IsPrimitive || elementType.IsEnum)
                    {
                        // no further exploration possible - surface the collection property
                        propNames.Add(fullName);
                    }
                    else
                    {
                        // recurse into element type
                        CollectProperties(elementType, fullName, depth + 1, maxDepth, pathStack, propNames);
                    }

                    continue;
                }

                // Interfaces and user-defined classes: recurse into them
                if (propType.IsInterface || (propType.IsClass && !propType.FullName!.StartsWith("System.", StringComparison.Ordinal)))
                {
                    CollectProperties(propType, fullName, depth + 1, maxDepth, pathStack, propNames);
                    continue;
                }

                // Fallback: add the property name
                propNames.Add(fullName);
            }

            pathStack.Pop();
        }

        public Type? GetEnumerableElementType(Type enumerableType)
        {
            if (enumerableType.IsArray)
            {
                return enumerableType.GetElementType();
            }

            if (enumerableType.IsGenericType)
            {
                var args = enumerableType.GetGenericArguments();
                if (args.Length == 1) return args[0];
            }

            // Look for IEnumerable<T> on implemented interfaces
            var ienum = enumerableType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                .Select(i => i.GetGenericArguments().FirstOrDefault())
                .FirstOrDefault();

            return ienum;
        }
    }
} 