using GovUk.Frontend.AspNetCore.Extensions.ConformanceTests.OptionsJson;
using Newtonsoft.Json.Linq;

namespace GovUk.Frontend.AspNetCore.Extensions.ConformanceTests
{
    public class ComponentFixtureData
    {
        public static IEnumerable<object[]> GetTaskListData() => GetData("task-list.json", typeof(TaskList));

        public static IEnumerable<object[]> GetData(string fixturesFilename, Type optionsType)
        {
            var fixturesFile = Path.Combine("Fixtures", fixturesFilename);

            if (!File.Exists(fixturesFile))
            {
                throw new FileNotFoundException(
                    $"Could not find fixtures file at: '{fixturesFile}'.",
                    fixturesFile);
            }

            var fixturesJson = File.ReadAllText(fixturesFile);
            var fixtures = JObject.Parse(fixturesJson).SelectToken("fixtures");

            if (fixtures is null)
            {
                throw new InvalidOperationException($"Couldn't find fixtures in '{fixturesFile}'.");
            }

            var testCaseDataType = typeof(ComponentTestCaseData<>).MakeGenericType(optionsType);

            foreach (var fixture in fixtures)
            {
                var name = fixture["name"]!.ToString();
                var options = fixture["options"]!.ToObject(optionsType);
                var html = fixture["html"]!.ToString();

                var testCaseData = Activator.CreateInstance(testCaseDataType, name, options, html)!;

                yield return new object[]
                {
                    testCaseData
                };
            }
        }
    }
}
