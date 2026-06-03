using System;

namespace GovUk.Frontend.AspNetCore.Extensions.Validation
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ModelTypeAttribute : Attribute
    {
        public ModelTypeAttribute(Type modelType)
        {
            ModelType = modelType;
        }

        public Type ModelType { get; }
    }
}
