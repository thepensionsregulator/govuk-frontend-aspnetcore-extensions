using System;

namespace ThePensionsRegulator.Frontend.Models
{
    public class SessionTimeoutSettings
    {
        public TimeSpan RemainingTime { get; set; }
        public TimeSpan ExtensionLength { get; set; }
        public TimeSpan WarningBeforeExpiry { get; set; }
        public string? ExpiryUrl { get; set; }
    }
}
