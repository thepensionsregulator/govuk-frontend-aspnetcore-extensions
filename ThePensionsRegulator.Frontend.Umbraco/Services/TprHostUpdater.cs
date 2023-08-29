using System;
using System.Linq;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    /// <summary>
    /// Updates the host for a TPR service so that, for example, links to service.tpr.local can be transformed to service.thepensionsregulator.gov.uk in production
    /// </summary>
    public class TprHostUpdater : IContextAwareHostUpdater
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

            if (!destination.IsAbsoluteUri || !IsTprHost(destination.Host) || !IsTprHost(requestHost))
            {
                return destinationUrl;
            }

            var newDestination = new UriBuilder(destination);
            newDestination.Host = TprService(destination.Host) + TprDomain(requestHost);
            return newDestination.Uri.ToString();
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

        private static bool IsTprProd(string[] segments)
        {
            return segments.Length > 3 && segments[segments.Length - 3] == "thepensionsregulator" && segments[segments.Length - 2] == "gov" && segments[segments.Length - 1] == "uk";
        }

        private static bool IsTprNonProd(string[] segments)
        {
            return segments.Length > 4 && segments[segments.Length - 3] == "tpr" && segments[segments.Length - 2] == "gov" && segments[segments.Length - 1] == "uk";
        }

        private static bool IsTprLocal(string[] segments)
        {
            return segments.Length > 2 && segments[segments.Length - 2] == "tpr" && segments[segments.Length - 1] == "local";
        }
    }
}
