using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomBuildTasks.MSBuildTasks
{
    static class Extensions
    {
        public static bool Error(this TaskLoggingHelper log, string code, string message, bool result = false)
        {
            log.LogError(null, code, null, null, 0, 0, 0, 0, message);
            return result;
        }

        public static bool Warn(this TaskLoggingHelper log, string code, string message, bool result = false)
        {
            log.LogWarning(null, code, null, null, 0, 0, 0, 0, message);
            return result;
        }

        public static IEnumerable<ITaskItem> AsItems(this JToken json) => json switch
        {
            JObject complex => new ITaskItem[] { complex.AsItem() },
            JArray array => array.SelectMany(AsItems),
            { Type: JTokenType.Null } => Array.Empty<ITaskItem>(),
            _ => new ITaskItem[] { new TaskItem(json.AsString()) },
        };

        public static string AsString(this JToken token)
        {
            if (token.Type == JTokenType.String)
                return token.ToString();

            using var writer = new StringWriter();
            using var json = new JsonTextWriter(writer) { Formatting = Formatting.Indented };
            token.WriteTo(json);
            json.Flush();

            return writer.ToString();
        }

        static ITaskItem AsItem(this JObject json)
        {
            var item = new TaskItem(json.AsString());
            // Top-level properties turned into metadata for convenience.
            foreach (var prop in json.Properties())
                item.SetMetadata(prop.Name, prop.Value.AsString());

            item.SetMetadata("_", json.AsString());

            return item;
        }
    }
}
