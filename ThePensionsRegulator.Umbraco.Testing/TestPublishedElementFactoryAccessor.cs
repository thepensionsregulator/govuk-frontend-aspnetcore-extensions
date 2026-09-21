using ThePensionsRegulator.Umbraco.Core;

namespace ThePensionsRegulator.Umbraco.Testing
{
    /// <summary>
    /// A test implementation of the <see cref="IOverridablePublishedElementFactoryAccessor"/> interface that returns a <see cref="TestPublishedElementFactory"/> instance.
    /// </summary>
    public class TestPublishedElementFactoryAccessor : IOverridablePublishedElementFactoryAccessor
    {
        public IOverridablePublishedElementFactory Get() => new TestPublishedElementFactory();
    }
}
