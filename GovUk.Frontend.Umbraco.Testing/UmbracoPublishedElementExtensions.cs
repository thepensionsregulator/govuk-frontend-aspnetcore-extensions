using Moq;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using ThePensionsRegulator.Umbraco;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace GovUk.Frontend.Umbraco.Testing
{
    public static class UmbracoPublishedElementExtensions
    {
        /// <summary>
        /// Verifies that a specific invocation matching a given expression was performed on the mock. Use in conjunction with the default <see cref="MockBehaviour.Loose" />.
        /// </summary>
        /// <param name="expression">The expression to verify.</param>
        /// <param name="times">The number of times a method is expected to be called.</param>
        /// <exception cref="ArgumentException" />
        /// <exception cref="MockException" />
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>
        public static Mock<T> Verify<T>(this T publishedElement, Expression<Action<T>> expression, Func<Times> times) where T : class, IPublishedElement
        {
            var mock = Mock.Get(publishedElement);
            mock.Verify(expression, times);
            return mock;
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>
        public static Mock<TModel> SetupUmbracoPropertyValue<TModel, TProperty>(this TModel publishedElement, string alias, TProperty value) where TModel : class, IPublishedElement
        {
            return Mock.Get(publishedElement).SetupUmbracoPropertyValue(alias, value);
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>
        public static Mock<TModel> SetupUmbracoPropertyValue<TModel, TProperty>(this Mock<TModel> publishedElement, string alias, TProperty value) where TModel : class, IPublishedElement
        {
            return SetupUmbracoPropertyValue(publishedElement, alias, value, (alias, value) => UmbracoPropertyFactory.CreateProperty(alias, null, value));
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> using a textbox data type, and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>

        public static Mock<T> SetupUmbracoTextboxPropertyValue<T>(this T publishedElement, string alias, string? value) where T : class, IPublishedElement
        {
            return Mock.Get(publishedElement).SetupUmbracoTextboxPropertyValue(alias, value);
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> using a textbox data type, and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>
        public static Mock<T> SetupUmbracoTextboxPropertyValue<T>(this Mock<T> publishedElement, string alias, string? value) where T : class, IPublishedElement
        {
            return SetupUmbracoPropertyValue(publishedElement, alias, value, UmbracoPropertyFactory.CreateTextboxProperty);
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> using an integer data type, and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>

        public static Mock<T> SetupUmbracoIntegerPropertyValue<T>(this T publishedElement, string alias, int? value) where T : class, IPublishedElement
        {
            return Mock.Get(publishedElement).SetupUmbracoIntegerPropertyValue(alias, value);
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> using an integer data type, and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>
        public static Mock<T> SetupUmbracoIntegerPropertyValue<T>(this Mock<T> publishedElement, string alias, int? value) where T : class, IPublishedElement
        {
            return SetupUmbracoPropertyValue(publishedElement, alias, value, UmbracoPropertyFactory.CreateIntegerProperty);
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> using a boolean data type, and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>

        public static Mock<T> SetupUmbracoBooleanPropertyValue<T>(this T publishedElement, string alias, bool? value) where T : class, IPublishedElement
        {
            return Mock.Get(publishedElement).SetupUmbracoBooleanPropertyValue(alias, value);
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> using a boolean data type, and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>

        public static Mock<T> SetupUmbracoBooleanPropertyValue<T>(this Mock<T> publishedElement, string alias, bool? value) where T : class, IPublishedElement
        {
            return SetupUmbracoPropertyValue(publishedElement, alias, value, UmbracoPropertyFactory.CreateBooleanProperty);
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> using a multi-URL picker data type configured to pick a maximum of one URL, and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>

        public static Mock<T> SetupUmbracoMultiUrlPickerPropertyValue<T>(this T publishedElement, string alias, Link? value) where T : class, IPublishedElement
        {
            return Mock.Get(publishedElement).SetupUmbracoMultiUrlPickerPropertyValue(alias, value);
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> using a multi-URL picker data type configured to pick a maximum of one URL, and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The value to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>
        public static Mock<T> SetupUmbracoMultiUrlPickerPropertyValue<T>(this Mock<T> publishedElement, string alias, Link? value) where T : class, IPublishedElement
        {
            return SetupUmbracoPropertyValue(publishedElement, alias, value, UmbracoPropertyFactory.CreateMultiUrlPickerProperty);
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> using a block list data type, and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The block list to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>
        public static Mock<TModel> SetupUmbracoBlockListPropertyValue<TModel, TBlockList>(this TModel publishedElement, string alias, TBlockList? value)
           where TModel : class, IPublishedElement
           where TBlockList : class, IEnumerable<BlockListItem>
        {
            return Mock.Get(publishedElement).SetupUmbracoBlockListPropertyValue(alias, value);
        }

        /// <summary>
        /// Mock an Umbraco property on an <see cref="IPublishedElement"/> using a block list data type, and set its value.
        /// </summary>
        /// <param name="alias">The alias of the Umbraco property to mock.</param>
        /// <param name="value">The block list to assign to the mocked Umbraco property.</param>
        /// <returns>The <see cref="Mock&lt;IPublishedElement&gt;"/> this method was called on.</returns>
        public static Mock<TModel> SetupUmbracoBlockListPropertyValue<TModel, TBlockList>(this Mock<TModel> publishedElement, string alias, TBlockList? value)
        where TModel : class, IPublishedElement
        where TBlockList : class, IEnumerable<BlockListItem>
        {
            publishedElement.SetupUmbracoProperty(UmbracoPropertyFactory.CreateBlockListProperty(alias, value));
            var overridablePublishedElement = publishedElement as Mock<IOverridablePublishedElement>;
            if (overridablePublishedElement != null)
            {
                overridablePublishedElement.Setup(x => x.Value<TBlockList>(It.Is<string>(x => string.Equals(alias, x, StringComparison.OrdinalIgnoreCase)), null, null, default, null)).Returns(value);
                overridablePublishedElement.Setup(x => x.Value<IEnumerable<BlockListItem>>(It.Is<string>(x => string.Equals(alias, x, StringComparison.OrdinalIgnoreCase)), null, null, default, null)).Returns(value);

                var readOnlyValue = value as ReadOnlyCollection<BlockListItem>;
                if (readOnlyValue != null)
                {
                    overridablePublishedElement.Setup(x => x.Value<ReadOnlyCollection<BlockListItem>>(It.Is<string>(x => string.Equals(alias, x, StringComparison.OrdinalIgnoreCase)), null, null, default, null)).Returns(readOnlyValue);
                }
            }
            return publishedElement;
        }

        private static Mock<TModel> SetupUmbracoPropertyValue<TModel, TProperty>(Mock<TModel> publishedElement, string alias, TProperty? value, Func<string, TProperty?, IPublishedProperty> createPropertyWithPropertyType)
            where TModel : class, IPublishedElement
        {
            publishedElement.SetupUmbracoProperty(createPropertyWithPropertyType(alias, value));
            var overridablePublishedElement = publishedElement as Mock<IOverridablePublishedElement>;
            if (overridablePublishedElement != null)
            {
                overridablePublishedElement.Setup(x => x.Value<TProperty?>(It.Is<string>(x => string.Equals(alias, x, StringComparison.OrdinalIgnoreCase)), null, null, default, default)).Returns(value);
            }
            return publishedElement;
        }

        private static Mock<T> SetupUmbracoProperty<T>(this Mock<T> publishedElement, IPublishedProperty property) where T : class, IPublishedElement
        {
            var properties = publishedElement.Object.Properties as IList<IPublishedProperty>;
            if (properties == null)
            {
                properties = new List<IPublishedProperty>();
                publishedElement.Setup(x => x.Properties).Returns(properties);
            }
            properties.Add(property);
            publishedElement.Setup(x => x.GetProperty(It.Is<string>(x => string.Equals(property.Alias, x, StringComparison.OrdinalIgnoreCase)))).Returns(property);
            return publishedElement;
        }
    }
}
