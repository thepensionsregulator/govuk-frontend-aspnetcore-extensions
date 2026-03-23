using GovUk.Frontend.AspNetCore.Extensions.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
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
            var connectSrcForHotReload = _webHostEnvironment.IsDevelopment() ? "'self' ws://localhost:* http://localhost:64300" : string.Empty;
            var scriptSrcForAblePlayer = "https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js https://cdn.jsdelivr.net/npm/js-cookie@3.0.1/dist/js.cookie.min.js";
            var styleSrcForAblePlayer = "'sha384-xBuQ/xzmlsLoJpyjoggmTEz8OWUFM0/RC5BsqQBDX2v5cMvDHcMakNTNrHIW2I5f' 'sha384-ETDm/j6COkRSUfVFsGNM5WYE4WjyRgfDhy4Pf4Fsc8eNw/eYEMqYZWuxTzMX6FBa'";
            var nonce = nonceProvider.GetNonce();
            context.Response.Headers.Append("Content-Security-Policy",
                    "default-src 'self'; " +
                    $"script-src 'self' 'nonce-{nonce}' {scriptSrcForAblePlayer} youtube.com www.youtube.com www.youtube-nocookie.com;" +
                    $"style-src 'self' {styleSrcForAblePlayer};" +
                    "img-src 'self' https://i.ytimg.com; " +
                    "frame-src youtube.com www.youtube.com www.youtube-nocookie.com; " +
                    $"connect-src 'self' https://api.os.uk; {connectSrcForHotReload}");

            await _next(context);
        }
    }
}