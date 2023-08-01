using System.Collections;
using System.Text.RegularExpressions;

namespace ThePensionsRegulator.Umbraco
{
	/// <summary>
	/// Exposes a property of an <see cref="IOverridablePublishedElement"/> as a list of tokens.
	/// </summary>
	public class TokenList : IList<string>
	{
		private readonly IOverridablePublishedElement _publishedElement;
		private readonly string _propertyName;
		private readonly List<string> _tokens = new();
		private bool _tokensCurrent = false;
		private Regex _regex = new Regex(@"\s+", RegexOptions.Compiled);
		private readonly string _separator;

		/// <summary>
		/// Creates a new <see cref="TokenList"/>
		/// </summary>
		/// <param name="publishedElement">The Umbraco content or element containing the property with the tokenised value.</param>
		/// <param name="propertyName">The name of the Umbraco property where the tokens are stored.</param>
		/// <param name="separator">The string with which tokens are separated in the property value.</param>
		/// <exception cref="ArgumentException">Thrown is <c>propertyName</c> is <c>null</c> or an empty string.</exception>
		/// <exception cref="ArgumentNullException">Thrown if <c>publishedElement</c> is <c>null</c>.</exception>
		public TokenList(IOverridablePublishedElement publishedElement, string propertyName, string separator = " ")
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new ArgumentException($"'{nameof(propertyName)}' cannot be null or empty.", nameof(propertyName));
			}

			_publishedElement = publishedElement ?? throw new ArgumentNullException(nameof(publishedElement));
			_propertyName = propertyName;
			_separator = separator;
		}

		private void Tokenise()
		{
			var raw = _publishedElement.Value<string>(_propertyName);
			_tokens.Clear();
			if (!string.IsNullOrEmpty(raw))
			{
				raw = _regex.Replace(raw, _separator).Trim();
				_tokens.AddRange(raw.Split(_separator));
			}
			_tokensCurrent = true;
		}

		/// <summary>
		/// Determines the index of a specific token in the list.
		/// </summary>
		/// <param name="index">The token to locate in the list.</param>
		/// <returns>The index of the token if found in the list; otherwise, -1.</returns>
		public string this[int index]
		{
			get
			{
				if (!_tokensCurrent) { Tokenise(); }
				return _tokens[index];

			}
			set
			{
				if (!_tokensCurrent) { Tokenise(); }
				_tokens[index] = value;
				_publishedElement.OverrideValue(_propertyName, string.Join(_separator, _tokens.ToArray()));
			}
		}

		/// <summary>
		/// Gets the number of tokens contained in the list.
		/// </summary>
		/// <returns>The number of tokens contained in the list.</returns>
		public int Count
		{
			get
			{
				if (!_tokensCurrent) { Tokenise(); }
				return _tokens.Count;
			}
		}

		/// <summary>
		/// Gets a value indicating whether the list is read-only.
		/// </summary>
		/// <returns>false</returns>
		public bool IsReadOnly => false;

		/// <summary>
		/// Adds a token to the list.
		/// </summary>
		/// <param name="token">The token to add to the list</param>
		public void Add(string token)
		{
			if (!_tokensCurrent) { Tokenise(); }
			if (!_tokens.Contains(token))
			{
				_tokens.Add(token);
				_publishedElement.OverrideValue(_propertyName, string.Join(_separator, _tokens.ToArray()));
			}
		}

		/// <summary>
		/// Removes all tokens from the list.
		/// </summary>
		public void Clear()
		{
			_tokens.Clear();
			_publishedElement.OverrideValue(_propertyName, string.Empty);
		}

		/// <summary>
		/// Determines whether the list contains a specific token.
		/// </summary>
		/// <param name="token">The token to locate in the list.</param>
		/// <returns>true if the token is found in the list; otherwise, false.</returns>
		public bool Contains(string token)
		{
			if (!_tokensCurrent) { Tokenise(); }
			return _tokens.Contains(token);
		}


		/// <summary>
		/// Copies the tokens to a System.Array, starting at a particular System.Array index.
		/// </summary>
		/// <param name="array">The one-dimensional System.Array that is the destination of the tokens copied. The System.Array must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		/// <exception cref="ArgumentNullException">array is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
		/// <exception cref="ArgumentException">The number of tokens is greater than the available space from arrayIndex to the end of the destination array.</exception>
		public void CopyTo(string[] array, int arrayIndex)
		{
			if (!_tokensCurrent) { Tokenise(); }
			_tokens.CopyTo(array, arrayIndex);
		}

		/// <summary>
		/// Returns an enumerator that iterates through the tokens.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the tokens.</returns>
		public IEnumerator<string> GetEnumerator()
		{
			if (!_tokensCurrent) { Tokenise(); }
			return _tokens.GetEnumerator();
		}

		/// <summary>
		/// Determines the index of a specific token in the list.
		/// </summary>
		/// <param name="token">The token to locate in the list.</param>
		/// <returns>The index of token if found in the list; otherwise, -1.</returns>
		public int IndexOf(string token)
		{
			if (!_tokensCurrent) { Tokenise(); }
			return _tokens.IndexOf(token);
		}

		/// <summary>
		/// Inserts a token to the list at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which the token should be inserted.</param>
		/// <param name="token">The token to insert into the list.</param>
		public void Insert(int index, string token)
		{
			if (!_tokensCurrent) { Tokenise(); }
			_tokens.Insert(index, token);
			_publishedElement.OverrideValue(_propertyName, string.Join(_separator, _tokens.ToArray()));
		}

		/// <summary>
		/// Removes the first occurrence of a specific token from the list.
		/// </summary>
		/// <param name="token">The token to remove from the list.</param>
		/// <returns>true if the token was successfully removed; otherwise, false. This method also returns false if the token is not found.</returns>
		public bool Remove(string token)
		{
			if (!_tokensCurrent) { Tokenise(); }
			var result = _tokens.Remove(token);
			if (result) { _publishedElement.OverrideValue(_propertyName, string.Join(_separator, _tokens.ToArray())); }
			return result;
		}

		/// <summary>
		/// Removes the list item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		public void RemoveAt(int index)
		{
			if (!_tokensCurrent) { Tokenise(); }
			_tokens.RemoveAt(index);
			_publishedElement.OverrideValue(_propertyName, string.Join(_separator, _tokens.ToArray()));
		}

		/// <summary>
		/// Returns an enumerator that iterates through the tokens.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the tokens.</returns>

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Returns the list of tokens separated by the separator specified in the constructor.
		/// </summary>
		/// <returns>The list of tokens separated by the separator specified in the constructor.</returns>
		public override string ToString()
		{
			if (!_tokensCurrent) { Tokenise(); }
			return string.Join(_separator, _tokens.ToArray());
		}
	}
}
