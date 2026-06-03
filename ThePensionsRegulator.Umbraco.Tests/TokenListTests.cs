using Moq;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Tests
{
	public class TokenListTests
	{
		private const string PROPERTY_ALIAS = "alias";

		private (TokenList TokenList, Mock<IPublishedElement> PublishedElement) CreateReadOnlyTokenList()
		{
			var publishedElement = UmbracoContentFactory.CreateContent<IPublishedElement>();
			publishedElement.SetupUmbracoTextboxPropertyValue(PROPERTY_ALIAS, "  example-a example-b    example-c  ");
			return (new TokenList(publishedElement.Object, PROPERTY_ALIAS), publishedElement);
		}

		private (TokenList TokenList, Mock<IOverridablePublishedElement> Settings) CreateOverridableTokenList()
		{
			var publishedElement = UmbracoContentFactory.CreateContent<IOverridablePublishedElement>();
			publishedElement.SetupUmbracoTextboxPropertyValue(PROPERTY_ALIAS, "  example-a example-b    example-c  ");
			return (new TokenList(publishedElement.Object, PROPERTY_ALIAS), publishedElement);
		}

		[Fact]
		public void Null_publishedElement_returns_empty_TokenList()
		{
			var list = new TokenList(null, PROPERTY_ALIAS);

			Assert.Empty(list);
			Assert.Equal(string.Empty, list.ToString());
		}

		[Fact]
		public void Class_list_is_tokenised()
		{
			var tokenList = CreateOverridableTokenList();

			Assert.Equal(3, tokenList.TokenList.Count);
			Assert.Equal("example-a", tokenList.TokenList[0]);
			Assert.Equal("example-b", tokenList.TokenList[1]);
			Assert.Equal("example-c", tokenList.TokenList[2]);
		}

		[Theory]
		[InlineData("example-b", true)]
		[InlineData("example-d", false)]
		[InlineData("example", false)]
		public void Contains_checks_whole_tokens_only(string checkFor, bool expected)
		{
			var tokenList = CreateOverridableTokenList();

			var result = tokenList.TokenList.Contains(checkFor);

			Assert.Equal(expected, result);
		}

		[Fact]
		public void Can_copy_to_array()
		{
			var tokenList = CreateOverridableTokenList();

			var result = new string[tokenList.TokenList.Count];
			tokenList.TokenList.CopyTo(result, 0);

			Assert.Equal("example-a", result[0]);
			Assert.Equal("example-b", result[1]);
			Assert.Equal("example-c", result[2]);
		}

		[Fact]
		public void Can_find_index_of_token()
		{
			var tokenList = CreateOverridableTokenList();

			var result = tokenList.TokenList.IndexOf("example-b");

			Assert.Equal(1, result);
		}

		[Fact]
		public void Can_add_token()
		{
			var tokenList = CreateOverridableTokenList();

			tokenList.TokenList.Add("example-d");

			tokenList.Settings.Verify(x => x.OverrideValue(PROPERTY_ALIAS, "example-a example-b example-c example-d"), Times.Once);
		}

		[Fact]
		public void Add_token_throws_if_read_only()
		{
			var tokenList = CreateReadOnlyTokenList();

			Assert.Throws<NotSupportedException>(() =>
			{
				tokenList.TokenList.Add("example-d");
			});
		}

		[Fact]
		public void Can_insert_at_index()
		{
			var tokenList = CreateOverridableTokenList();

			tokenList.TokenList.Insert(1, "example-d");

			tokenList.Settings.Verify(x => x.OverrideValue(PROPERTY_ALIAS, "example-a example-d example-b example-c"), Times.Once);
		}

		[Fact]
		public void Insert_at_index_throws_if_read_only()
		{
			var tokenList = CreateReadOnlyTokenList();

			Assert.Throws<NotSupportedException>(() =>
			{
				tokenList.TokenList.Insert(1, "example-d");
			});
		}

		[Fact]
		public void Can_remove_token()
		{
			var tokenList = CreateOverridableTokenList();

			tokenList.TokenList.Remove("example-b");

			tokenList.Settings.Verify(x => x.OverrideValue(PROPERTY_ALIAS, "example-a example-c"), Times.Once);
		}

		[Fact]
		public void Remove_token_throws_if_read_only()
		{
			var tokenList = CreateReadOnlyTokenList();

			Assert.Throws<NotSupportedException>(() =>
			{
				tokenList.TokenList.Remove("example-b");
			});
		}


		[Fact]
		public void Can_remove_at_index()
		{
			var tokenList = CreateOverridableTokenList();

			tokenList.TokenList.RemoveAt(1);

			tokenList.Settings.Verify(x => x.OverrideValue(PROPERTY_ALIAS, "example-a example-c"), Times.Once);
		}

		[Fact]
		public void Remove_at_index_throws_if_read_only()
		{
			var tokenList = CreateReadOnlyTokenList();

			Assert.Throws<NotSupportedException>(() =>
			{
				tokenList.TokenList.RemoveAt(1);
			});
		}

		[Fact]
		public void Can_clear_tokens()
		{
			var tokenList = CreateOverridableTokenList();

			tokenList.TokenList.Clear();

			tokenList.Settings.Verify(x => x.OverrideValue(PROPERTY_ALIAS, string.Empty), Times.Once);
		}

		[Fact]
		public void Clear_tokens_throws_if_read_only()
		{
			var tokenList = CreateReadOnlyTokenList();

			Assert.Throws<NotSupportedException>(() =>
			{
				tokenList.TokenList.Clear();
			});
		}

		[Fact]
		public void ToString_returns_tokens()
		{
			var tokenList = CreateOverridableTokenList();

			var result = tokenList.TokenList.ToString();

			Assert.Equal("example-a example-b example-c", result);
		}
	}
}
