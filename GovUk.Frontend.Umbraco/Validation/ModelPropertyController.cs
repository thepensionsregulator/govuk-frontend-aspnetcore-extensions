using GovUk.Frontend.AspNetCore.Extensions.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
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

            files.RemoveAll(x => x.Contains($"{Path.DirectorySeparatorChar}System.") || x.Contains($"{Path.DirectorySeparatorChar}Microsoft.")); // Don't load dlls where there won't be any custom code. This list is not exhaustive
            
            foreach(var file in files) 
            { 
                Assembly assembly = Assembly.LoadFrom(file);
                Type[] types;
                try { types = assembly.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { types = ex.Types.OfType<Type>().ToArray(); }

                List<Type> controllersToAdd = types
                    .Where(x => x != typeof(RenderController) && x.IsAssignableTo(typeof(RenderController)))
                    .ToList();
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

        internal void CollectProperties(Type type, string prefix, int depth, int maxDepth, Stack<Type> pathStack, List<string> propNames)
        {
            if (type == null) return;
            if (depth >= maxDepth) return;
            if (pathStack.Contains(type)) return; // avoid cycles

            pathStack.Push(type);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop => prop.CanRead))
            {
                var propType = property.PropertyType;

                // Skip Umbraco published content and blocklist model types
                if (propType.IsAssignableTo(typeof(PublishedContentModel)) || propType.IsAssignableTo(typeof(OverridableBlockListModel)) || propType.IsAssignableTo(typeof(OverridableBlockGridModel)))
                {
                    continue;
                }

                var fullName = string.IsNullOrEmpty(prefix) ? property.Name : $"{prefix}.{property.Name}";

                // Primitive-ish types and string: add the property and stop recursing
                if (propType.IsPrimitive || propType.IsEnum || propType == typeof(string) || propType == typeof(decimal) ||
                    propType == typeof(DateTime) || propType == typeof(DateOnly) || propType == typeof(DateTimeOffset) ||
                    propType == typeof(Guid) || IsNullableTerminalType(propType))
                {
                    propNames.Add(fullName);
                    continue;
                }

                // Handle tuples: expand tuple element names
                if (IsTupleType(propType))
                {
                    var tupleElementNames = GetTupleElementNames(propType, property);
                    foreach (var elementName in tupleElementNames)
                    {
                        propNames.Add($"{fullName}.{elementName}");
                    }
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
                    else if (IsTupleType(elementType))
                    {
                        // Element type is a tuple: expand tuple elements
                        var tupleElementNames = GetTupleElementNames(elementType);
                        foreach (var elementName in tupleElementNames)
                        {
                            propNames.Add($"{fullName}.{elementName}");
                        }
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

        internal bool IsNullableTerminalType(Type type)
        {
            if (!type.IsGenericType) return false;

            var genericDefinition = type.GetGenericTypeDefinition();
            if (genericDefinition != typeof(Nullable<>)) return false;

            var underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType!.IsPrimitive ||
                   underlyingType == typeof(decimal) ||
                   underlyingType == typeof(DateTime) ||
                   underlyingType == typeof(DateOnly) ||
                   underlyingType == typeof(DateTimeOffset) ||
                   underlyingType == typeof(Guid);
        }

        internal Type? GetEnumerableElementType(Type enumerableType)
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

        internal bool IsTupleType(Type type)
        {
            if (type == null) return false;

            // Unwrap Nullable<T> to check the underlying type
            var typeToCheck = type;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                typeToCheck = Nullable.GetUnderlyingType(type)!;
            }

            // Check for ValueTuple (preferred in modern .NET)
            if (typeToCheck.IsGenericType)
            {
                var genericDefinition = typeToCheck.GetGenericTypeDefinition();
                if (genericDefinition == typeof(ValueTuple) ||
                    genericDefinition == typeof((object, object)) ||
                    genericDefinition.Name.StartsWith("ValueTuple`"))
                {
                    return true;
                }
            }

            // Check for System.Tuple (older .NET)
            return typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == typeof(Tuple<>);
        }

        internal IEnumerable<string> GetTupleElementNames(Type tupleType, PropertyInfo? property = null)
        {
            if (tupleType == null || !IsTupleType(tupleType))
            {
                return Enumerable.Empty<string>();
            }

            // Unwrap Nullable<T> to get the actual tuple type
            var actualTupleType = tupleType;
            if (tupleType.IsGenericType && tupleType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                actualTupleType = Nullable.GetUnderlyingType(tupleType)!;
            }

            var genericArgs = actualTupleType.GetGenericArguments();

            // Try to get custom tuple element names from TupleElementNamesAttribute on the property
            if (property != null)
            {
                var propertyAttr = property.GetCustomAttribute<TupleElementNamesAttribute>();
                if (propertyAttr?.TransformNames != null && propertyAttr.TransformNames.Count > 0)
                {
                    return propertyAttr.TransformNames;
                }
            }

            // Try to get custom tuple element names from TupleElementNamesAttribute on the type itself
            var tupleAttr = actualTupleType.GetCustomAttribute<TupleElementNamesAttribute>();
            if (tupleAttr?.TransformNames != null && tupleAttr.TransformNames.Count > 0)
            {
                // Filter to only return names for actual tuple elements (skip any synthetic names)
                return tupleAttr.TransformNames.Take(genericArgs.Length);
            }

            // Fallback: generate default names (Item1, Item2, etc.)
            return Enumerable.Range(1, genericArgs.Length).Select(i => $"Item{i}");
        }
    }
} 