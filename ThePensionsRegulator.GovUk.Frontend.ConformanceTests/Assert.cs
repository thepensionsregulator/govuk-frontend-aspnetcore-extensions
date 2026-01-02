using AngleSharp.Diffing;
using System.Text;
using Xunit.Sdk;

namespace ThePensionsRegulator.GovUk.Frontend.ConformanceTests
{
    internal static class Assert
    {
        internal static void HtmlEqual(
            string expected,
            string actual)
        {
            var diffs = DiffBuilder.Compare(expected).WithTest(actual).Build()
                .ToArray();

            if (diffs.Length != 0)
            {
                var sb = new StringBuilder();
                sb.AppendLine($"{nameof(Assert)}.{nameof(HtmlEqual)}() failure");
                sb.AppendLine();

                foreach (var diff in diffs)
                {
                    DiffConverter.Append(diff, sb);
                }

                throw new XunitException(sb.ToString());
            }
        }
    }
}
