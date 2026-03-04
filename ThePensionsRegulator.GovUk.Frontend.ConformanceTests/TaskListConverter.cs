using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using ThePensionsRegulator.GovUk.Frontend.ConformanceTests.OptionsJson;

namespace ThePensionsRegulator.GovUk.Frontend.ConformanceTests
{
    /// <summary>
    /// A custom converter is required because the <c>items</c> collection can contain an object, boolean <c>false</c> or empty string, and .NET needs help to deserialise that.
    /// </summary>
    internal class TaskListConverter : CustomCreationConverter<TaskList>
    {
        public override TaskList Create(Type objectType)
        {
            throw new NotImplementedException();
        }

        public TaskList Create(JObject jObject)
        {
            var taskList = new TaskList();

            var items = jObject.Property("items")?.ToArray()?.First();
            if (items is not null)
            {
                foreach (var item in items)
                {
                    var task = (item.Type != JTokenType.Boolean) ? JsonConvert.DeserializeObject<TaskListTask>(item.ToString()) : null;
                    taskList.Items.Add(task);
                }
            }

            return taskList;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream 
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject 
            var target = Create(jObject);

            // Populate the object properties 
            jObject.Remove("items");
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }

}
