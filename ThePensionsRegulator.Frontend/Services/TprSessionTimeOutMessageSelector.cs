using System;
using System.Collections.Generic;
using ThePensionsRegulator.Frontend.Models;

namespace ThePensionsRegulator.Frontend.Services
{
    public static class TprSessionTimeOutMessageSelector
    {
        public static TprSessionTimeOutModel? SelectByKey(
            IEnumerable<TprSessionTimeOutModel> messages,
            string? messageKey)
        {
            ArgumentNullException.ThrowIfNull(messages);

            TprSessionTimeOutModel? first = null;
            foreach (var message in messages)
            {
                if (message is null) continue;
                first ??= message;

                if (!string.IsNullOrWhiteSpace(messageKey)
                    && string.Equals(message.MessageKey, messageKey, StringComparison.OrdinalIgnoreCase))
                {
                    return message;
                }
            }

            return string.IsNullOrWhiteSpace(messageKey) ? first : null;
        }
    }
}
