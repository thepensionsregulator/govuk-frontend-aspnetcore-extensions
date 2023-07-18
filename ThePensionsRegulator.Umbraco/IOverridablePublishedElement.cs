using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco
{
    /// <summary>
    /// A wrapper for <see cref="IPublishedElement"/> which allows property values to be overridden.
    /// </summary>
    public interface IOverridablePublishedElement : IPublishedElement
    {
        void OverrideValue(string alias, object value);
        T? Value<T>(string alias, string? culture = null, string? segment = null, Fallback fallback = default, T? defaultValue = default);
    }
}