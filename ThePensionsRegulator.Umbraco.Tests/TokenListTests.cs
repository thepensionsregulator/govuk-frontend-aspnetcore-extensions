using Moq;
using ThePensionsRegulator.Umbraco.Testing;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace ThePensionsRegulator.Umbraco.Tests
{
	[TestFixture]
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

		[Test]
		public void Null_publishedElement_returns_empty_TokenList()
		{
			var list = new TokenList(null, PROPERTY_ALIAS);

			Assert.That(list.Count, Is.EqualTo(0));
			Assert.That(list.ToString(), Is.EqualTo(string.Empty));
		}

		[Test]
		public void Class_list_is_tokenised()
		{
			var tokenList = CreateOverridableTokenList();

			Assert.That(tokenList.TokenList.Count, Is.EqualTo(3));
			Assert.That(tokenList.TokenList[0], Is.EqualTo("example-a"));
			Assert.That(tokenList.TokenList[1], Is.EqualTo("example-b"));
			Assert.That(tokenList.TokenList[2], Is.EqualTo("example-c"));
		}

		[TestCase("example-b", true)]
		[TestCase("example-d", false)]
		[TestCase("example", false)]
		public void Contains_checks_whole_tokens_only(string checkFor, bool expected)
		{
			var tokenList = CreateOverridableTokenList();

			var result = tokenList.TokenList.Contains(checkFor);

			Assert.That(result, Is.EqualTo(expected));
		}

		public void Can_copy_to_array()
		{
			var tokenList = CreateOverridableTokenList();

			var result = new string[tokenList.TokenList.Count];
			tokenList.TokenList.CopyTo(result, 0);

			Assert.That(result[0], Is.EqualTo("example-a"));
			Assert.That(result[1], Is.EqualTo("example-b"));
			Assert.That(result[2], Is.EqualTo("example-c"));
		}

		[Test]
		public void Can_find_by_index()
		{
			var tokenList = CreateOverridableTokenList();

			var result = tokenList.TokenList.IndexOf("example-b");

			Assert.That(result, Is.EqualTo(1));
		}

		[Test]
		public void Can_add_token()
		{
			var tokenList = CreateOverridableTokenList();

			tokenList.TokenList.Add("example-d");

			tokenList.Settings.Verify(x => x.OverrideValue(PROPERTY_ALIAS, "example-a example-b example-c example-d"), Times.Once);
		}

		[Test]
		public void Add_token_throws_if_read_only()
		{
			var tokenList = CreateReadOnlyTokenList();

			Assert.Throws<NotSupportedException>(delegate
			{
				tokenList.TokenList.Add("example-d");
			});
		}

		[Test]
		public void Can_insert_at_index()
		{
			var tokenList = CreateOverridableTokenList();

			tokenList.TokenList.Insert(1, "example-d");

			tokenList.Settings.Verify(x => x.OverrideValue(PROPERTY_ALIAS, "example-a example-d example-b example-c"), Times.Once);
		}

		[Test]
		public void Insert_at_index_throws_if_read_only()
		{
			var tokenList = CreateReadOnlyTokenList();

			Assert.Throws<NotSupportedException>(delegate
			{
				tokenList.TokenList.Insert(1, "example-d");
			});
		}

		[Test]
		public void Can_remove_token()
		{
			var tokenList = CreateOverridableTokenList();

			tokenList.TokenList.Remove("example-b");

			tokenList.Settings.Verify(x => x.OverrideValue(PROPERTY_ALIAS, "example-a example-c"), Times.Once);
		}

		[Test]
		public void Remove_token_throws_if_read_only()
		{
			var tokenList = CreateReadOnlyTokenList();

			Assert.Throws<NotSupportedException>(delegate
			{
				tokenList.TokenList.Remove("example-b");
			});
		}


		[Test]
		public void Can_remove_at_index()
		{
			var tokenList = CreateOverridableTokenList();

			tokenList.TokenList.RemoveAt(1);

			tokenList.Settings.Verify(x => x.OverrideValue(PROPERTY_ALIAS, "example-a example-c"), Times.Once);
		}

		[Test]
		public void Remove_at_index_throws_if_read_only()
		{
			var tokenList = CreateReadOnlyTokenList();

			Assert.Throws<NotSupportedException>(delegate
			{
				tokenList.TokenList.RemoveAt(1);
			});
		}

		[Test]
		public void Can_clear_tokens()
		{
			var tokenList = CreateOverridableTokenList();

			tokenList.TokenList.Clear();

			tokenList.Settings.Verify(x => x.OverrideValue(PROPERTY_ALIAS, string.Empty), Times.Once);
		}

		[Test]
		public void Clear_tokens_throws_if_read_only()
		{
			var tokenList = CreateReadOnlyTokenList();

			Assert.Throws<NotSupportedException>(delegate
			{
				tokenList.TokenList.Clear();
			});
		}

		[Test]
		public void ToString_returns_tokens()
		{
			var tokenList = CreateOverridableTokenList();

			var result = tokenList.TokenList.ToString();

			Assert.That(result, Is.EqualTo("example-a example-b example-c"));
		}
	}
}
