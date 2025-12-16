using System;
using System.IO;
using System.Linq;
using System.Xml;
using Umbraco.Cms.Core.Events;
using uSync.BackOffice;

namespace GovUk.Frontend.Umbraco.ExampleApp.NotificationHandlers
{
    public class uSyncNotificationHandler : INotificationHandler<uSyncExportedItemNotification>
    {
        public void Handle(uSyncExportedItemNotification notification)
        {
            var xml = notification.Item;
            var type = xml.Name.LocalName;

            if (!type.Equals("ContentType") && !type.Equals("DataType"))
            {
                return;
            }

            var itemId = xml.Attributes().FirstOrDefault(x => x.Name.LocalName == "Key");
            var alias = xml.Attributes().FirstOrDefault(x => x.Name.LocalName == "Alias");
            if (itemId is null || alias is null || string.IsNullOrEmpty(itemId.Value) || string.IsNullOrEmpty(alias.Value))
            {
                return;
            }

            if (!alias.Value.Contains("Gov.Uk", StringComparison.OrdinalIgnoreCase) && !alias.Value.Contains("TPR", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            var directoryPath = Path.Combine("usync/v9/guidFileNames", type);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            XmlDocument document = new();
            document.LoadXml(xml.ToString());
            document.Save(Path.Combine(directoryPath, $"{itemId.Value}.config"));
        }
    }
}
