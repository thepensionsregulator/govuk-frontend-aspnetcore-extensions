using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.Services
{
    public static class TprSessionTimeOutMessageSelector
    {
        public static TprSessionTimeOutModel? SelectByKey(IEnumerable<TprSessionTimeOutModel> messages, string? messageKey)
        {
            ArgumentNullException.ThrowIfNull(messages);

            TprSessionTimeOutModel? fallback = null;

            foreach (var message in messages)
            {
                if (message is null) { continue; }

                fallback ??= message;

                if (!string.IsNullOrWhiteSpace(messageKey) && string.Equals(message.MessageKey, messageKey, StringComparison.OrdinalIgnoreCase))
                {
                    return message;
                }
            }

            return fallback;
        }
    }
}
