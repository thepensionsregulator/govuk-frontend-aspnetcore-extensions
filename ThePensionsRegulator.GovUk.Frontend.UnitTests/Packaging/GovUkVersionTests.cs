using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ThePensionsRegulator.GovUk.Frontend.UnitTests.Packaging
{
    public class GovUkVersionTests
    {
        static string ProjectRoot => FindProjectRoot();

        [Fact]
        public void Build_updates_govuk_frontend_version_in_generated_CSS()
        {
            var csproj = Path.Combine(ProjectRoot, "ThePensionsRegulator.GovUk.Frontend.csproj");
            var partialPath = Path.Combine(ProjectRoot, "Styles", "_govuk-version.generated.scss");
            var cssPath = Path.Combine(ProjectRoot, "wwwroot", "ThePensionsRegulator.GovUk.Frontend", "css", "govuk-frontend.css");
            var originalPartial = File.Exists(partialPath) ? File.ReadAllText(partialPath) : null;
            var originalCss = File.Exists(cssPath) ? File.ReadAllText(cssPath) : null;

            try
            {
                var success = RunMsBuildTarget(csproj, "GovUkFrontend_BuildSass", "-p:Configuration=Release", ProjectRoot);
                Assert.True(success, "GovUkFrontend_BuildSass target failed");

                Assert.True(File.Exists(partialPath), $"Expected generated partial at {partialPath}");
                Assert.True(File.Exists(cssPath), $"Expected generated CSS at {cssPath}");

                var partial = File.ReadAllText(partialPath);
                var css = File.ReadAllText(cssPath);

                Assert.Contains("--govuk-frontend-version", partial);

                var expectedVersion = GetGovUkFrontendVersionFromPartial(partial);
                Assert.NotNull(expectedVersion);
                Assert.Contains($"--govuk-frontend-version: \"{expectedVersion}\"", css);
            }
            finally
            {
                RestoreFile(partialPath, originalPartial);
                RestoreFile(cssPath, originalCss);
            }
        }

        static bool RunMsBuildTarget(string csproj, string target, string properties, string workingDirectory)
        {
            if (!File.Exists(csproj)) throw new FileNotFoundException("Project file not found", csproj);

            var start = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"msbuild \"{csproj}\" -t:{target} {properties}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = workingDirectory
            };

            using var p = Process.Start(start);
            if (p == null) return false;
            p.WaitForExit(120_000);
            var outText = p.StandardOutput.ReadToEnd();
            var errText = p.StandardError.ReadToEnd();
            if (p.ExitCode != 0)
            {
                var msg = $"msbuild exit {p.ExitCode}\nSTDOUT:\n{outText}\nSTDERR:\n{errText}";
                throw new Exception(msg);
            }

            return true;
        }

        static string? GetGovUkFrontendVersionFromPartial(string partial)
        {
            const string marker = "--govuk-frontend-version: \"";
            var start = partial.IndexOf(marker, StringComparison.Ordinal);
            if (start < 0) return null;

            start += marker.Length;
            var end = partial.IndexOf('"', start);
            return end >= 0 ? partial[start..end] : null;
        }

        static void RestoreFile(string path, string? originalContent)
        {
            if (originalContent is null)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                return;
            }

            File.WriteAllText(path, originalContent);
        }

        static string FindProjectRoot([CallerFilePath] string testFilePath = "")
        {
            var testDirectory = Path.GetDirectoryName(testFilePath);
            if (string.IsNullOrEmpty(testDirectory))
            {
                throw new InvalidOperationException("Could not determine the test file path.");
            }

            var repoRoot = Path.GetFullPath(Path.Combine(testDirectory, "..", ".."));
            return Path.GetFullPath(Path.Combine(repoRoot, "ThePensionsRegulator.GovUk.Frontend"));
        }
    }
}
