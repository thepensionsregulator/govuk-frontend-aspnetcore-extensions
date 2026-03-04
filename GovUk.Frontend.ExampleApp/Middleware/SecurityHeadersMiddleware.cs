using GovUk.Frontend.AspNetCore.Extensions.Security;

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
            var connectSrcForHotReload = _webHostEnvironment.IsDevelopment() ? "'self' ws://localhost:* http://localhost:64300" : string.Empty;
            var styleSrcForAblePlayer = "'sha384-xBuQ/xzmlsLoJpyjoggmTEz8OWUFM0/RC5BsqQBDX2v5cMvDHcMakNTNrHIW2I5f' 'sha384-ETDm/j6COkRSUfVFsGNM5WYE4WjyRgfDhy4Pf4Fsc8eNw/eYEMqYZWuxTzMX6FBa'";
            var nonce = nonceProvider.GetNonce();
            context.Response.Headers.Append("Content-Security-Policy",
                    "default-src 'self'; " +
                    $"script-src 'self' 'nonce-{nonce}' youtube.com www.youtube.com www.youtube-nocookie.com;" +
                    $"style-src 'self' {styleSrcForAblePlayer};" +
                    "img-src 'self' https://i.ytimg.com; " +
                    "frame-src youtube.com www.youtube.com www.youtube-nocookie.com; " +
                    $"connect-src {connectSrcForHotReload}");

            await _next(context);
        }
    }
}