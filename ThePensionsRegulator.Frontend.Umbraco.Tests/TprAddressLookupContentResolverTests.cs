using System.Reflection;
using Moq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using ThePensionsRegulator.Umbraco.Core;
using Umbraco.Cms.Core.Dictionary;

namespace ThePensionsRegulator.Frontend.Umbraco.Tests
{
    public class TprAddressLookupContentResolverTests
    {
        private const string BlockValue = "From block";
        private const string DictionaryValue = "From dictionary";

        public static TheoryData<int> FieldIndexes()
        {
            var data = new TheoryData<int>();
            for (var i = 0; i < TprAddressLookupContentResolver.Fields.Count; i++)
            {
                data.Add(i);
            }
            return data;
        }

        [Fact]
        public void Every_content_property_is_mapped_exactly_once()
        {
            var stringProperties = typeof(TprAddressLookupContent).GetProperties().Where(p => p.PropertyType == typeof(string)).ToList();

            var mappedProperties = TprAddressLookupContentResolver.Fields.Select(f => FindPropertyFor(f).Name).ToList();

            Assert.Equal(stringProperties.Select(p => p.Name).OrderBy(x => x), mappedProperties.OrderBy(x => x));
        }

        [Fact]
        public void Property_aliases_and_dictionary_keys_are_unique()
        {
            var fields = TprAddressLookupContentResolver.Fields;

            Assert.Equal(fields.Count, fields.Select(f => f.PropertyAlias).Distinct(StringComparer.OrdinalIgnoreCase).Count());
            Assert.Equal(fields.Count, fields.Select(f => f.DictionaryKey).Distinct().Count());
        }

        [Fact]
        public void Library_defaults_are_used_when_block_and_dictionary_are_empty()
        {
            var dictionary = new Mock<ICultureDictionary>();
            dictionary.Setup(x => x[It.IsAny<string>()]).Returns(string.Empty);

            var result = TprAddressLookupContentResolver.Resolve(null, dictionary.Object);

            AssertAllPropertiesEqual(new TprAddressLookupContent(), result);
        }

        [Theory]
        [MemberData(nameof(FieldIndexes))]
        public void Dictionary_value_overrides_library_default(int fieldIndex)
        {
            var field = TprAddressLookupContentResolver.Fields[fieldIndex];
            var dictionary = CreateDictionary(field.DictionaryKey, DictionaryValue);

            var result = TprAddressLookupContentResolver.Resolve(CreateBlock(field.PropertyAlias, null), dictionary);

            Assert.Equal(DictionaryValue, FindPropertyFor(field).GetValue(result));
        }

        [Theory]
        [MemberData(nameof(FieldIndexes))]
        public void Block_value_overrides_dictionary_value(int fieldIndex)
        {
            var field = TprAddressLookupContentResolver.Fields[fieldIndex];
            var dictionary = CreateDictionary(field.DictionaryKey, DictionaryValue);

            var result = TprAddressLookupContentResolver.Resolve(CreateBlock(field.PropertyAlias, BlockValue), dictionary);

            Assert.Equal(BlockValue, FindPropertyFor(field).GetValue(result));
        }

        [Theory]
        [MemberData(nameof(FieldIndexes))]
        public void Whitespace_block_value_falls_back_to_dictionary(int fieldIndex)
        {
            var field = TprAddressLookupContentResolver.Fields[fieldIndex];
            var dictionary = CreateDictionary(field.DictionaryKey, DictionaryValue);

            var result = TprAddressLookupContentResolver.Resolve(CreateBlock(field.PropertyAlias, "  "), dictionary);

            Assert.Equal(DictionaryValue, FindPropertyFor(field).GetValue(result));
        }

        [Theory]
        [MemberData(nameof(FieldIndexes))]
        public void Whitespace_dictionary_value_falls_back_to_library_default(int fieldIndex)
        {
            var field = TprAddressLookupContentResolver.Fields[fieldIndex];
            var dictionary = CreateDictionary(field.DictionaryKey, "  ");
            var property = FindPropertyFor(field);

            var result = TprAddressLookupContentResolver.Resolve(CreateBlock(field.PropertyAlias, string.Empty), dictionary);

            Assert.Equal(property.GetValue(new TprAddressLookupContent()), property.GetValue(result));
        }

        private static IOverridablePublishedElement CreateBlock(string alias, string? value)
        {
            var block = new Mock<IOverridablePublishedElement>();
            block.Setup(x => x.Value<string>(alias, null, null, default, default)).Returns(value);
            return block.Object;
        }

        private static ICultureDictionary CreateDictionary(string key, string value)
        {
            var dictionary = new Mock<ICultureDictionary>();
            dictionary.Setup(x => x[It.IsAny<string>()]).Returns(string.Empty);
            dictionary.Setup(x => x[key]).Returns(value);
            return dictionary.Object;
        }

        private static PropertyInfo FindPropertyFor(TprAddressLookupContentResolver.Field field)
        {
            const string sentinel = "__sentinel__";
            var content = new TprAddressLookupContent();
            field.Apply(content, sentinel);
            return typeof(TprAddressLookupContent).GetProperties().Single(p => Equals(p.GetValue(content), sentinel));
        }

        private static void AssertAllPropertiesEqual(TprAddressLookupContent expected, TprAddressLookupContent actual)
        {
            foreach (var property in typeof(TprAddressLookupContent).GetProperties())
            {
                Assert.Equal(property.GetValue(expected), property.GetValue(actual));
            }
        }
    }
}
