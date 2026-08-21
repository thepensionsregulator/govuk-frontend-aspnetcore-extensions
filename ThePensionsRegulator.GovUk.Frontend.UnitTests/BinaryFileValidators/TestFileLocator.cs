namespace ThePensionsRegulator.GovUk.Frontend.UnitTests.BinaryFileValidators
{
    internal static class TestFileLocator
    {
        public static string TestFileDirectory = @"BinaryFileValidators\TestFiles" + Path.DirectorySeparatorChar;
        public static string EmptyExcelFile => Path.Combine([TestFileDirectory, "EmptyExcelFile.xlsx"]);
        public static string EmptyPowerPoint => Path.Combine([TestFileDirectory, "EmptyPowerPoint.pptx"]);
        public static string WordSavedAsPdf => Path.Combine(TestFileDirectory, "WordSavedAsPdf.pdf");
    }
}