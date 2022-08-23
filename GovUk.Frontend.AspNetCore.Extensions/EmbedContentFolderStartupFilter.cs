using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.FileProviders;
using System;

namespace GovUk.Frontend.AspNetCore.Extensions
{
    public class EmbedContentFolderStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            return app =>
            {
                app.UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new ManifestEmbeddedFileProvider(
                      typeof(EmbedContentFolderStartupFilter).Assembly,
                      root: "wwwroot")
                });

                next(app);
            };
        }
    }
}
