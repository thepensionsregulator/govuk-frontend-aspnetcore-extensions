using AngleSharp.Diffing;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using System.Text;

namespace GovUk.Frontend.AspNetCore.Extensions.ConformanceTests
{
    internal static class Assert
    {
        internal static void HtmlEqual(
            string expected,
            string actual)
        {
            var diffs = DiffBuilder.Compare(expected).WithTest(actual).Build()
                .Where(diff => !ExcludeFalsePositive(diff))
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

                NUnit.Framework.Assert.Fail(sb.ToString());
            }
        }

        private static bool ExcludeFalsePositive(IDiff diff)
        {
            var matchedRule = AllowAdditionalHtmlClasses(diff);
            if (matchedRule) { return true; }

            matchedRule = AllowAdditionalHtmlIds(diff);
            if (matchedRule) { return true; }

            return false;
        }

        /// <summary>
        /// We must include required ids, but adding extra ones can be useful and should not fail.
        /// </summary>
        /// <param name="diff"></param>
        /// <returns><c>true</c> to exclude the diff as a false positive; <c>false</c> otherwise.</returns>
        private static bool AllowAdditionalHtmlIds(IDiff diff)
        {
            if (diff is UnexpectedAttrDiff attrDiff && attrDiff.Test.Attribute.Name == "id")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// We must include the required HTML classes, but adding extra ones can be useful and should not fail.
        /// </summary>
        /// <param name="diff"></param>
        /// <returns><c>true</c> to exclude the diff as a false positive; <c>false</c> otherwise.</returns>
        private static bool AllowAdditionalHtmlClasses(IDiff diff)
        {
            if (diff is AttrDiff attrDiff && attrDiff.Test.Attribute.Name == "class")
            {
                var controlValueParts = attrDiff.Control.Attribute.Value
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var testValueParts = attrDiff.Test.Attribute.Value
                    .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var className in controlValueParts)
                {
                    if (!testValueParts.Contains(className)) { return false; }
                }
                return true;
            }
            return false;
        }
    }
}
