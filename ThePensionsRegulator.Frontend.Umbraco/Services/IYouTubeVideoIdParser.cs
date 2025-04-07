using System;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public interface IYouTubeVideoIdParser
    {
        bool TryParseUrl(string urlToParse, out string? videoId);
        bool TryParseUrl(Uri urlToParse, out string? videoId);
    }
}