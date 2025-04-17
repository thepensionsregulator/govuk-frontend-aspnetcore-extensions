using GovUk.Frontend.AspNetCore.Extensions.ConformanceTests.OptionsJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GovUk.Frontend.AspNetCore.Extensions.ConformanceTests
{
    public class ComponentFixtureData
    {
        public static IEnumerable<object[]> GetTaskListData() => GetData<TaskList>("task-list.json", [new TaskListConverter()]);

        public static IEnumerable<object[]> GetData<T>(string fixturesFilename, JsonConverter[]? optionsConverters)
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

            var testCaseDataType = typeof(ComponentTestCaseData<>).MakeGenericType(typeof(T));

            foreach (var fixture in fixtures)
            {
                var name = fixture["name"]!.ToString();
                var options = JsonConvert.DeserializeObject<T>(fixture["options"]!.ToString(), optionsConverters ?? []);
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
