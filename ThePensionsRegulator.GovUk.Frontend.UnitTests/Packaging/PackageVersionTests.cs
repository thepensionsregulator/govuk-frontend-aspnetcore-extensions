using System.Diagnostics;

namespace ThePensionsRegulator.GovUk.Frontend.UnitTests.Packaging
{
    public class PackageVersionTests
    {
        static string RepoRoot => FindRepoRoot();

        [Fact]
        public void PreparePackage_GeneratesVersionPartial_And_UpdatesGovukFrontend()
        {
            var version = "1.2.3-test";

            // Copying the project to a temporary folder so the test doesn't modify the repository
            var tempRoot = Path.Combine(Path.GetTempPath(), "tpr-pack-test-" + System.Guid.NewGuid().ToString("N"));
            var sourceProjectRoot = Path.Combine(RepoRoot, "ThePensionsRegulator.GovUk.Frontend");
            var tempProjectRoot = Path.Combine(tempRoot, "ThePensionsRegulator.GovUk.Frontend");
            CopyDirectory(sourceProjectRoot, tempProjectRoot);

            var tempCsproj = Path.Combine(tempProjectRoot, "ThePensionsRegulator.GovUk.Frontend.csproj");

            try
            {
                var success = RunMsBuildTarget(tempCsproj, "GovUkFrontend_PreparePackage", $"-p:Configuration=Release -p:Version={version}", tempProjectRoot);
                Assert.True(success, "PreparePackage target failed");

                var tempPartial = Path.Combine(tempProjectRoot, "Styles", "_tpr-version.scss");
                var tempGovuk = Path.Combine(tempProjectRoot, "Styles", "govuk-frontend.scss");

                Assert.True(File.Exists(tempPartial), $"Expected generated partial at {tempPartial}");
                var partial = File.ReadAllText(tempPartial);
                Assert.Contains("--govuk-frontend-version", partial);
                Assert.Contains(version, partial);

                Assert.True(File.Exists(tempGovuk), $"Expected govuk-frontend.scss at {tempGovuk}");
                var govuk = File.ReadAllText(tempGovuk);
                Assert.Contains("_tpr-version", govuk);

                var restored = RunMsBuildTarget(tempCsproj, "GovUkFrontend_RestoreAfterPack", "-p:Configuration=Release", tempProjectRoot);
                Assert.True(restored, "RestoreAfterPack target failed");

                Assert.False(File.Exists(tempPartial), "Generated partial should be removed after restore");
            }
            finally
            {
                try { Directory.Delete(tempRoot, true); } catch { }
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

        static void CopyDirectory(string sourceDir, string targetDir)
        {
            var dir = new DirectoryInfo(sourceDir);
            if (!dir.Exists) throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
            Directory.CreateDirectory(targetDir);

            foreach (var file in dir.GetFiles())
            {
                var targetFilePath = Path.Combine(targetDir, file.Name);
                file.CopyTo(targetFilePath, true);
            }

            foreach (var subDir in dir.GetDirectories())
            {
                var newTargetDir = Path.Combine(targetDir, subDir.Name);
                CopyDirectory(subDir.FullName, newTargetDir);
            }
        }

        static string FindRepoRoot()
        {
            var dir = Path.GetDirectoryName(typeof(PackageVersionTests).Assembly.Location) ?? Directory.GetCurrentDirectory();
            while (dir != null)
            {
                var candidate = Path.Combine(dir, "GovUk.Frontend.slnx");
                if (File.Exists(candidate)) return dir;
                dir = Path.GetDirectoryName(dir);
            }
            throw new FileNotFoundException("Could not locate repository root (GovUk.Frontend.slnx)");
        }
    }
}
