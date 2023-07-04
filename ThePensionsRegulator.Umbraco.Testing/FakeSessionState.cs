using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ThePensionsRegulator.Umbraco.Testing
{
    /// <summary>
    /// HTTP Session State implemented using a <see cref="Dictionary{TKey, TValue}"/>. For use in tests via <see cref="UmbracoTestContext"/>.
    /// </summary>
    internal class FakeSessionState : ISession
    {
        private readonly Dictionary<string, object> _sessionStorage = new();
        public string Id => throw new NotImplementedException();
        public bool IsAvailable => throw new NotImplementedException();
        public IEnumerable<string> Keys => _sessionStorage.Keys;
        public void Clear()
        {
            _sessionStorage.Clear();
        }
        public Task CommitAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public Task LoadAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public void Remove(string key)
        {
            _sessionStorage.Remove(key);
        }
        public void Set(string key, byte[] value)
        {
            _sessionStorage[key] = Encoding.UTF8.GetString(value);
        }
        public bool TryGetValue(string key, [NotNullWhen(true)] out byte[]? value)
        {
            if (_sessionStorage[key] != null)
            {
                var valueAsString = _sessionStorage[key].ToString();
                if (valueAsString is not null)
                {
                    value = Encoding.ASCII.GetBytes(valueAsString);
                    return true;
                }
            }
            value = null;
            return false;
        }
    }
}
