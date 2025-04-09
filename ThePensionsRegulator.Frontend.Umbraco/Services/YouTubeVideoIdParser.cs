using System;
using System.Text.RegularExpressions;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public class YouTubeVideoIdParser : IYouTubeVideoIdParser
    {
        public bool TryParseUrl(Uri urlToParse, out string? videoId)
        {
            if (urlToParse is null)
            {
                videoId = null;
                return false;
            }

            return TryParseUrl(urlToParse.ToString(), out videoId);
        }

        public bool TryParseUrl(string urlToParse, out string? videoId)
        {
            if (string.IsNullOrWhiteSpace(urlToParse))
            {
                videoId = null;
                return false;
            }

            var match = Regex.Match(urlToParse, @"(\/embed\/|\/watch\?v=|youtu.be\/)(?<VideoId>[A-Z0-9_]+)", RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                videoId = null;
                return false;
            }

            videoId = match.Groups["VideoId"].Value;
            return true;
        }
    }
}
