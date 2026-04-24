using ThePensionsRegulator.GovUk.Frontend.Security;
using Umbraco.Cms.Infrastructure.Migrations.Install;

namespace GovUk.Frontend.Umbraco.ExampleApp.Middleware
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

        public async Task InvokeAsync(HttpContext context, INonceProvider nonceProvider, DatabaseBuilder databaseBuilder)
        {
            string path = context.Request.Path;
            if (databaseBuilder.IsDatabaseConfigured && path.StartsWith("/umbraco", StringComparison.OrdinalIgnoreCase) == false)
            {
                var connectSrcForLocalhost = _webHostEnvironment.IsDevelopment() ? "'self' ws://localhost:* http://localhost:*" : string.Empty; // Allows Visual Studio Browser Link for hot reload

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
                        $"script-src 'self' 'nonce-{nonce}' {scriptSrcForYouTube};" +
                        $"style-src 'self' {styleSrcForAblePlayer};" +
                        $"img-src 'self' {imgSrcForYouTube};" +
                        $"frame-src {frameSrcForYouTube}; " +
                        $"connect-src 'self' {connectSrcForLocalhost}");
            }

            await _next(context);
        }
    }
}