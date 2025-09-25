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
        private readonly ITprGlobalNavigationService _tprGlobalNavigationSerivce;

        public TprHeaderMenuBlockListGenerator(IContentService contentService, IContentTypeService contentTypeService, ITprGlobalNavigationService tprGlobalNavigationSerivce)
        {
            _contentService = contentService;
            _contentTypeService = contentTypeService;
            _tprGlobalNavigationSerivce = tprGlobalNavigationSerivce;
        }

        public void GenerateTprHeaderMenuBlockList(Guid rootKey, Guid settingsKey)
        {

            var settings = _contentService.GetById(settingsKey);
            if (settings == null)
            {
                throw new ArgumentException("Settings node Id cannot be null");
            }

            var contentTypes = _contentTypeService.GetAll();
            var parentItemType = contentTypes.Where(x => x.Alias == "tprHeaderMenuParentItems").FirstOrDefault();
            var chidlItemType = contentTypes.Where(x => x.Alias == "tprHeaderMenuChildItem").FirstOrDefault();

            var exsitingBlockListData = GetExsisitngBlockListData(settings, "tprHeaderMenu");


            var finalContentData = new List<Dictionary<string, object>>(exsitingBlockListData.contentData);
            var finalLayoutUdis = new List<Dictionary<string, object>>(exsitingBlockListData.layout);
            var finalSettingsData = new List<Dictionary<string, object>>(exsitingBlockListData.settingsData);


            var newContentData = new List<Dictionary<string, object>>();
            var newLayourUdis = new List<Dictionary<string, object>>();

            var menuItems = _tprGlobalNavigationSerivce.GetMenuItems(rootKey);

            var validUdis = new HashSet<string>();

            foreach (var menuItem in menuItems)
            {
                var stableUdi = GenerateStableUdi(menuItem.ContentKey);
                var exsistingBlock = finalContentData.FirstOrDefault(x => x.TryGetValue("udi", out var udi) && udi.ToString() == stableUdi.ToString());

                if (exsistingBlock != null)
                {
                    exsistingBlock["linkText"] = menuItem.LinkText;
                    exsistingBlock["linkUrl"] = menuItem.LinkUrl;
                }
                else
                {
                    var newBlock = new Dictionary<string, object>
                    {
                        { "contentTypeKey", parentItemType.Key.ToString() },
                        {"linkText", menuItem.LinkText },
                        {"udi", stableUdi.ToString() }

                    };

                    if (!string.IsNullOrEmpty(menuItem.LinkUrl))
                    {
                        newBlock.Add("linkUrl", new List<Dictionary<string, object>>()
                        {
                            new Dictionary<string, object> { { "url", menuItem.LinkUrl } } });
                    }


                    if (menuItem.HeaderMenuChildItems != null)
                    {

                        var childMenuBlock = GenerateChildMenuBlockList(menuItem.HeaderMenuChildItems, chidlItemType);
                        newBlock.Add("tprHeaderMenuChildItems", childMenuBlock);
                    }

                    finalContentData.Add(newBlock);
                    finalLayoutUdis.Add(new Dictionary<string, object> { { "contentUdi", stableUdi.ToString() } });
                }
                validUdis.Add(stableUdi.ToString());
            }

            finalContentData.RemoveAll(block => block.TryGetValue("udi", out var udi) && !validUdis.Contains(udi));
            finalLayoutUdis.RemoveAll(layout => layout.TryGetValue("contentUdi", out var udi) && !validUdis.Contains(udi));

            var finalBlockList = new BlockList
            {
                layout = new BlockListUdi(finalLayoutUdis),
                contentData = finalContentData,
                settingsData = finalSettingsData
            };

            settings?.SetValue("tprHeaderMenu", JsonConvert.SerializeObject(finalBlockList));

            if (settings != null)
            {
                _contentService.SaveAndPublish(settings);
            }
        }

        private GuidUdi GenerateStableUdi(Guid contentKey)
        {
            return new GuidUdi("element", contentKey);
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
                if (exsisitngJson["contentData"] is JArray contentArray)
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
                var stableUdi = GenerateStableUdi(item.ContentKey);

                var childItemData = new Dictionary<string, object>
                {
                    {"contentTypeKey", contentType.Key.ToString() },
                    {"udi", stableUdi.ToString()},
                    {"linkText", item.LinkText },
                };

                if (!string.IsNullOrEmpty(item.LinkUrl))
                {
                    childItemData.Add("linkUrl", new List<Dictionary<string, object>>
                    {
                        new Dictionary<string, object>{ {"url", item.LinkUrl } }
                    });
                }
                else
                {
                    childItemData.Add("linkUrl", "");
                }

                childContentData.Add(childItemData);
                childLayoutUdis.Add(new Dictionary<string, object>
                {
                    {"contentUdi", stableUdi.ToString()},
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

    public interface ITprHeaderMenuBlockListGenerator
    {

        public void GenerateTprHeaderMenuBlockList(Guid rootKey, Guid settingsKey);

        public BlockList GenerateChildMenuBlockList(List<TprHeaderMenuChildItem> childItems, IContentType contentType);
    }


}

