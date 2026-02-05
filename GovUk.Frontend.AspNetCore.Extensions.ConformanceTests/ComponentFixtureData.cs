using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace GovUk.Frontend.AspNetCore.Extensions.ConformanceTests
{
    public class ComponentFixtureData<T> : DataAttribute
    {
        private readonly string _fixtureFilename;
        private readonly Type _optionsType;
        private readonly string? _only;
        private readonly HashSet<string> _exclude;

        public ComponentFixtureData(
            string fixtureFileName,
            string? only = null,
            params string[] exclude)
        {
            _fixtureFilename = fixtureFileName ?? throw new ArgumentNullException(nameof(fixtureFileName));
            _optionsType = typeof(T);
            _only = only;
            _exclude = [.. exclude ?? Array.Empty<string>()];

            if (!_fixtureFilename.EndsWith(".json"))
            {
                _fixtureFilename += ".json";
            }
        }

        public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod, DisposalTracker disposalTracker)
        {
            return new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(Impl().ToArray());

            IEnumerable<ITheoryDataRow> Impl()
            {
                var fixturesFile = Path.Combine("Fixtures", _fixtureFilename);

                if (!File.Exists(fixturesFile))
                {
                    throw new FileNotFoundException(
                        $"Could not find fixtures file at: '{fixturesFile}'.",
                        fixturesFile);
                }

                var fixturesJson = File.ReadAllText(fixturesFile);
                var fixtures = JObject.Parse(fixturesJson).SelectToken("fixtures") ?? throw new InvalidOperationException($"Couldn't find fixtures in '{fixturesFile}'.");
                var testCaseDataType = typeof(ComponentTestCaseData<>).MakeGenericType(_optionsType);

                foreach (var fixture in fixtures)
                {
                    var name = fixture["name"]?.ToString() ?? string.Empty;

                    if (_exclude.Contains(name) || (_only is not null && name != _only))
                    {
                        continue;
                    }

                    var options = JsonConvert.DeserializeObject<T>(fixture["options"]!.ToString(), new TaskListConverter());
                    var html = fixture["html"]?.ToString();

                    var testCaseData = Activator.CreateInstance(testCaseDataType, name, options, html);

                    yield return new TheoryDataRow(testCaseData);
                }
            }
        }

        public override bool SupportsDiscoveryEnumeration() => true;
    }

}
