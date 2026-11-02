using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ThePensionsRegulator.Frontend.Tests.Packaging
{
    public class TprVersionTests
    {
        static string ProjectRoot => FindProjectRoot();

        [Fact]
        public void Build_updates_govuk_frontend_version_in_generated_CSS()
        {
            var csproj = Path.Combine(ProjectRoot, "ThePensionsRegulator.Frontend.csproj");
            var cssPath = Path.Combine(ProjectRoot, "wwwroot", "ThePensionsRegulator.Frontend", "css", "tpr.css");
            var originalCss = File.Exists(cssPath) ? File.ReadAllText(cssPath) : null;

            try
            {
                var success = RunMsBuildTarget(csproj, "TprFrontend_BuildSass", "-p:Configuration=Release", ProjectRoot);
                Assert.True(success, "TprFrontend_BuildSass target failed");

                Assert.True(File.Exists(cssPath), $"Expected generated CSS at {cssPath}");

                var css = File.ReadAllText(cssPath);

                const string cssMarker = "--govuk-frontend-version:";
                Assert.Contains(cssMarker, css);
                Assert.Equal(1, CountOccurrences(css, cssMarker));
            }
            finally
            {
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

        static int CountOccurrences(string text, string value)
        {
            var count = 0;
            var index = 0;

            while ((index = text.IndexOf(value, index, StringComparison.Ordinal)) >= 0)
            {
                count++;
                index += value.Length;
            }

            return count;
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
            return Path.GetFullPath(Path.Combine(repoRoot, "ThePensionsRegulator.Frontend"));
        }
    }
}
