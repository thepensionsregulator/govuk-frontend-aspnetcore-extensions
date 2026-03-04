namespace ThePensionsRegulator.GovUk.Frontend.Validation
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
