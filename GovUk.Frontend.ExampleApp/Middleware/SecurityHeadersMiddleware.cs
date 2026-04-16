using GovUk.Frontend.AspNetCore.Extensions.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GovUk.Frontend.ExampleApp.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SecurityHeadersMiddleware(RequestDelegate next, IWebHostEnvironment webHostEnvironment)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task InvokeAsync(HttpContext context, INonceProvider nonceProvider)
        {
            var connectSrcForVisualStudioBrowserLink = _webHostEnvironment.IsDevelopment() ? "'self' ws://localhost:* http://localhost:*" : string.Empty;

            const string scriptSrcForAblePlayer = "https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js https://cdn.jsdelivr.net/npm/js-cookie@3.0.1/dist/js.cookie.min.js";
            const string styleSrcForAblePlayer = "'sha384-xBuQ/xzmlsLoJpyjoggmTEz8OWUFM0/RC5BsqQBDX2v5cMvDHcMakNTNrHIW2I5f' 'sha384-ETDm/j6COkRSUfVFsGNM5WYE4WjyRgfDhy4Pf4Fsc8eNw/eYEMqYZWuxTzMX6FBa'";
            const string scriptSrcForYouTube = "https://youtube.com https://www.youtube.com https://www.youtube-nocookie.com";
            const string imgSrcForYouTube = "https://i.ytimg.com";
            const string frameSrcForYouTube = "https://youtube.com https://www.youtube.com https://www.youtube-nocookie.com";
            const string pictureInPictureForYouTube = "\"https://www.youtube.com\"  \"https://www.youtube-nocookie.com\"";
            const string fullscreenSrcYouTube = "\"https://www.youtube.com\"  \"https://www.youtube-nocookie.com\"";

            var nonce = nonceProvider.GetNonce();
            context.Response.Headers.Append("Permissions-Policy", $"accelerometer=(),autoplay=(),camera=(),cross-origin-isolated=(),display-capture=(),encrypted-media=(),fullscreen=(self {fullscreenSrcYouTube}),geolocation=(),gyroscope=(),magnetometer=(),microphone=(),midi=(),payment=(),picture-in-picture=(self {pictureInPictureForYouTube}),publickey-credentials-get=(),screen-wake-lock=(),sync-xhr=(),usb=(),web-share=(),xr-spatial-tracking=()");
            context.Response.Headers.Append("Content-Security-Policy",
                    "default-src 'self';" +
                    "require-trusted-types-for 'script';" +
                    $"script-src 'self' 'nonce-{nonce}' {scriptSrcForAblePlayer} {scriptSrcForYouTube};" +
                    $"style-src 'self' {styleSrcForAblePlayer};" +
                    $"img-src 'self' {imgSrcForYouTube};" +
                    $"frame-src {frameSrcForYouTube}; " +
                    $"connect-src 'self' {connectSrcForVisualStudioBrowserLink}");

            await _next(context);
        }
    }
}