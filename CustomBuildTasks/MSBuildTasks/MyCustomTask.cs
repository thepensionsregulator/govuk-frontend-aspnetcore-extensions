using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.IO;
using System.Xml;

namespace CustomBuildTasks.MSBuildTasks
{
    public class MyCustomTask : Task
    {
        public override bool Execute()
        {
            //System.Diagnostics.Debugger.Launch();

            foreach (var type in uSyncTypes)
            {
                var files = Directory.GetFiles($"{uSyncFilesPath}\\{type}");

                var outputDirectiory = BuildOutputDirectory(type);
                Directory.CreateDirectory(outputDirectiory);
                foreach (var file in files)
                {
                    var fileContent = File.ReadAllText(file);
                    var hash = fileContent.GetHashCode();

                    var outputFilePath = BuildOutputFileName(file, outputDirectiory, type);

                    if (!FileExistsAndIdentical(hash, outputFilePath))
                    {
                        File.Copy(file, outputFilePath, true);
                    }
                }
            }

            return true;
        }

        [Required]
        public string uSyncFilesPath { get; set; }

        private string defaultOutputPath = "GUIDFileNames\\";

        public string outputFilePath { get; set; }

        [Required]
        public string[] uSyncTypes { get; set; }

        private string BuildOutputFileName(string originalFileName, string outputPath, string uSyncType)
        {
            var guid = GetGuid(originalFileName, uSyncType);

            return $"{outputPath}\\{guid}.config";
        }

        private string BuildOutputDirectory(string uSyncType)
        {
            var outputDirectory = outputFilePath;
            if (string.IsNullOrEmpty(outputDirectory))
            {
                outputDirectory = $"{uSyncFilesPath}\\{defaultOutputPath}";
            }

            outputDirectory += $"\\{uSyncType}";

            return outputDirectory;
        }

        private bool FileExistsAndIdentical(int originalFileHash, string outputFilePath)
        {
            if (File.Exists(outputFilePath))
            {
                var existingFileContent = File.ReadAllText(outputFilePath);
                var existingFileContentHash = existingFileContent.GetHashCode();

                return existingFileContentHash == originalFileHash;
            }
            else
            {
                return false;
            }
        }

        private string GetGuid(string fileName, string uSyncType)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(fileName);
            var node = xmlDoc.SelectSingleNode(uSyncType.Remove(uSyncType.Length-1)); // Remove the 's' that makes the type plural, ContentTypes become ContentType

            return node.Attributes["Key"].Value;
        }
    }
}
