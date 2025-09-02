using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace ThePensionsRegulator.Frontend.Umbraco.Services
{
    public class TprHeaderMenuBlockListGenerator : ITprHeaderMenuBlockListGenerator
    {
        private readonly IContentService _contentService;
        private readonly IContentTypeService _contentTypeService;
        private readonly ITprGlobalNavigationSerivce _tprGlobalNavigationSerivce;

        public TprHeaderMenuBlockListGenerator(IContentService contentService, IContentTypeService contentTypeService, ITprGlobalNavigationSerivce tprGlobalNavigationSerivce)
        {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _tprGlobalNavigationSerivce = tprGlobalNavigationSerivce;
        }

        public void GenerateTprHeaderMenuBlockList(int rootId, int settingsId)
        {

            var settings = _contentService.GetById(settingsId);
            if (settings == null)
            {
                throw new ArgumentException("Settings node Id cannot be null");
            }

            var contentTypes = _contentTypeService.GetAll();
            var parentItemType = contentTypes.Where(x => x.Alias == "tprHeaderMenuParentItem").FirstOrDefault();
            var chidlItemType = contentTypes.Where(x => x.Alias == "tprHeaderMenuChildItem").FirstOrDefault();

            var exsitingBlockListData = GetExsisitngBlockListData(settings, settings.ContentType.Alias);

            var finalBlockList = new BlockList();
            var finalContentData = new List<Dictionary<string, object>>(exsitingBlockListData.contentData);
            var finalLayoutUdis = new List<Dictionary<string, object>>(exsitingBlockListData.layout);
            var finalSettingsData = new List<Dictionary<string, object>>(exsitingBlockListData.settingsData);


            var newContentData = new List<Dictionary<string, object>>();
            var newLayourUdis = new List<Dictionary<string, object>>();

            var menuItems = _tprGlobalNavigationSerivce.GetMenuItems(rootId);

            foreach (var menuItem in menuItems)
            {
                GuidUdi contentUdi = new GuidUdi("element", Guid.NewGuid());

                var mainItemData = new Dictionary<string, object>
                {
                    { "contentTypeKey", parentItemType.Key.ToString() },
                    { "linkText", menuItem.LinkText },
                    { "udi", contentUdi.ToString() }
                };

                if (!string.IsNullOrEmpty(menuItem.LinkDestination))
                {
                    mainItemData.Add("linkDestination", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>{ {"url", menuItem.LinkDestination } }
                    });
                }
                else
                {
                    mainItemData.Add("linkDestination", "");
                }

                newContentData.Add(mainItemData);

                if (menuItem.HeaderMenuChildItems != null)
                {

                    var childMenuBlock = GenerateChildMenuBlockList(menuItem.HeaderMenuChildItems, chidlItemType);
                    newContentData.Add(new Dictionary<string, object> { { "tprHeaderMenuChildItems", childMenuBlock } });
                }


                newLayourUdis.Add(new Dictionary<string, object> { { "contentUdi", contentUdi.ToString() } });
            }

            finalContentData.InsertRange(0, newContentData);
            finalLayoutUdis.InsertRange(0, newLayourUdis);

            finalBlockList.layout = new BlockListUdi(newLayourUdis);
            finalBlockList.contentData = finalContentData;
            finalBlockList.settingsData = finalSettingsData;

            settings?.SetValue("tprHeaderMenu", JsonConvert.SerializeObject(finalBlockList));

            if (settings != null)
            {
                _contentService.SaveAndPublish(settings);
            }

        }

        private ExsistingBlockList GetExsisitngBlockListData(IContent settingsNode, string propertyAlias)
        {
            var exsistingValue = settingsNode.GetValue(propertyAlias)?.ToString();

            if (string.IsNullOrWhiteSpace(exsistingValue))
            {
                return new ExsistingBlockList
                {
                    layout = new List<Dictionary<string, object>>(),
                    contentData = new List<Dictionary<string, object>>(),
                    settingsData = new List<Dictionary<string, object>>()

                };
            }

            try
            {
                var exsisitngJson = JObject.Parse(exsistingValue);

                var layoutUdis = new List<Dictionary<string, object>>();
                if (exsisitngJson["layout"]?["Umbraco.BlockList"] is JArray layoutArray)
                {
                    foreach (var item in layoutArray)
                    {
                        layoutUdis.Add(item.ToObject<Dictionary<string, object>>());
                    }
                }

                var contentData = new List<Dictionary<string, object>>();
                if (exsisitngJson["contentData"]?["Umbraco.BlockList"] is JArray contentArray)
                {
                    foreach (var item in contentArray)
                    {
                        contentData.Add(item.ToObject<Dictionary<string, object>>());
                    }
                }

                var settingsData = new List<Dictionary<string, object>>();
                if (exsisitngJson["settingsData"] is JArray settingsArray)
                {
                    foreach (var item in settingsArray)
                    {
                        settingsData.Add(item.ToObject<Dictionary<string, object>>());
                    }
                }

                return new ExsistingBlockList
                {
                    layout = layoutUdis,
                    contentData = contentData,
                    settingsData = settingsData
                };

            }
            catch (JsonException)
            {
                return new ExsistingBlockList
                {
                    layout = new List<Dictionary<string, object>>(),
                    contentData = new List<Dictionary<string, object>>(),
                    settingsData = new List<Dictionary<string, object>>()
                };
            }
        }

        public BlockList GenerateChildMenuBlockList(List<TprHeaderMenuChildItem> childItems, IContentType contentType)
        {
            var childBlockList = new BlockList();
            var childContentData = new List<Dictionary<string, object>>();
            var childLayoutUdis = new List<Dictionary<string, object>>();

            foreach (var item in childItems)
            {
                GuidUdi childItemUdi = new GuidUdi("element", Guid.NewGuid());

                var childItemData = new Dictionary<string, object>
                {
                    {"contentTypeKey", contentType.Key.ToString() },
                    {"udi", childItemUdi.ToString() },
                    {"linkText", item.LinkText },
                    //{"linkDestination", item.LinkDestination}
                };

                if (!string.IsNullOrEmpty(item.LinkDestination))
                {
                    childItemData.Add("linkDestination", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>{ {"url", item.LinkDestination } }
                    });
                }
                else
                {
                    childItemData.Add("linkDestination", "");
                }

                childContentData.Add(childItemData);
                childLayoutUdis.Add(new Dictionary<string, object>
                {
                    {"contentUdi", childItemUdi.ToString()},
                });

            }

            childBlockList.layout = new BlockListUdi(childLayoutUdis);
            childBlockList.contentData = childContentData;
            childBlockList.settingsData = new List<Dictionary<string, object>>();

            return childBlockList;

        }
    }


    public class BlockList
    {
        public BlockListUdi? layout { get; set; }
        public List<Dictionary<string, object>>? contentData { get; set; }
        public List<Dictionary<string, object>>? settingsData { get; set; }
    }

    public class ExsistingBlockList
    {
        public List<Dictionary<string, object>>? layout { get; set; }
        public List<Dictionary<string, object>>? contentData { get; set; }
        public List<Dictionary<string, object>>? settingsData { get; set; }
    }

    public class BlockListUdi
    {
        [JsonProperty("Umbraco.BlockList")]
        public List<Dictionary<string, object>> _contentUdi { get; set; }

        public BlockListUdi(List<Dictionary<string, object>> contentUdi)
        {

            _contentUdi = contentUdi;
        }
    }

    //public class TprHeaderChildMenuBlockList
    //{
    //    public BlockListUdi? layout { get; set; }
    //    public List<Dictionary<string, object>>? contentData { get; set; }
    //    public List<Dictionary<string, string>>? settingsData { get; set; }
    //}

    public interface ITprHeaderMenuBlockListGenerator
    {

        public void GenerateTprHeaderMenuBlockList(int rootId, int settingsId);

        public BlockList GenerateChildMenuBlockList(List<TprHeaderMenuChildItem> childItems, IContentType contentType);
    }


}

