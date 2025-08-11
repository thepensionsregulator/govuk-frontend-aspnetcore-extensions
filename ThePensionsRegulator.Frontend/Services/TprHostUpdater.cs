using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace ThePensionsRegulator.Frontend.Services
{
    /// <summary>
    /// Updates the host for a TPR service so that, for example, links to service.tpr.local can be transformed to service.thepensionsregulator.gov.uk in production
    /// </summary>
    public class TprHostUpdater(IOptions<TprFrontendOptions> _tprOptions) : IContextAwareHostUpdater
    {
        public string UpdateHost(string destinationUrl, string requestHost)
        {
            if (string.IsNullOrEmpty(destinationUrl))
            {
                throw new ArgumentException($"'{nameof(destinationUrl)}' cannot be null or empty.", nameof(destinationUrl));
            }

            if (string.IsNullOrEmpty(requestHost))
            {
                throw new ArgumentException($"'{nameof(requestHost)}' cannot be null or empty.", nameof(requestHost));
            }

            var destination = new Uri(destinationUrl, UriKind.RelativeOrAbsolute);

            if (!destination.IsAbsoluteUri || !IsTprHost(destination.Host) || IsTprAutomaticEnrolmentHost(destination.Host) || !IsTprHost(requestHost) || !IsAllowedDestinationHost(destination.Host))
            {
                return destinationUrl;
            }

            var newDestination = new UriBuilder(destination);
            newDestination.Host = TprService(destination.Host) + TprDomain(requestHost);
            return newDestination.Uri.ToString();
        }

        private bool IsAllowedDestinationHost(string host)
        {
            var hostAllowList = _tprOptions?.Value?.UpdateDestinationHostnames;
            if (hostAllowList is null) { return true; }

            var segments = host.ToLowerInvariant().Split(".");
            if (segments.Length == 0) { return true; }

            return hostAllowList.Contains(segments[0]);
        }

        private string TprService(string host)
        {
            var segments = host.ToLowerInvariant().Split(".");
            if (IsTprLocal(segments))
            {
                return string.Join('.', segments.Take(segments.Length - 2));
            }
            else if (IsTprProd(segments))
            {
                return string.Join('.', segments.Take(segments.Length - 3));
            }
            else
            {
                return string.Join('.', segments.Take(segments.Length - 4));
            }
        }

        private string TprDomain(string host)
        {
            var segments = host.ToLowerInvariant().Split(".");
            if (IsTprLocal(segments))
            {
                return ".tpr.local";
            }
            else if (IsTprProd(segments))
            {
                return ".thepensionsregulator.gov.uk";
            }
            else
            {
                return "." + string.Join('.', segments.Skip(segments.Length - 4).Take(4));
            }
        }

        private static bool IsTprHost(string host)
        {
            var segments = host.ToLowerInvariant().Split(".");
            return IsTprLocal(segments) || IsTprNonProd(segments) || IsTprProd(segments);
        }

        private static bool IsTprAutomaticEnrolmentHost(string host)
        {
            var segments = host.ToLowerInvariant().Split(".");
            return segments.Length > 4 && segments[^4] == "ae" && segments[^3] == "tpr" && segments[^2] == "gov" && segments[^1] == "uk";
        }

        private static bool IsTprProd(string[] segments)
        {
            return segments.Length > 3 && segments[^3] == "thepensionsregulator" && segments[^2] == "gov" && segments[^1] == "uk";
        }

        private static bool IsTprNonProd(string[] segments)
        {
            return segments.Length > 4 && segments[^3] == "tpr" && segments[^2] == "gov" && segments[^1] == "uk";
        }

        private static bool IsTprLocal(string[] segments)
        {
            return segments.Length > 2 && segments[^2] == "tpr" && segments[^1] == "local";
        }
    }
}
