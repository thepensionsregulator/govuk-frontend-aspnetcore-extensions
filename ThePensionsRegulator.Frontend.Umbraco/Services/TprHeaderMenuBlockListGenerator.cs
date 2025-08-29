using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using ThePensionsRegulator.Frontend.HtmlGeneration;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Trees;
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
            if(settings == null)
            {
                throw new ArgumentException();
            }

            var contentTypes = _contentTypeService.GetAll();
            var parentItemType = contentTypes.Where(x => x.Alias == "tprHeaderMenuParentItem").FirstOrDefault();
            var chidlItemType = contentTypes.Where(x => x.Alias == "tprHeaderMenuChildItem").FirstOrDefault();

            var mainBlockList = new TprHeaderMenuBlockList();
            var mainContentData = new List<Dictionary<string, object>>();
            var mainLayoutUdis = new List<Dictionary<string, string>>();

            var menuItems = _tprGlobalNavigationSerivce.GetMenuItems(rootId);

            foreach (var menuItem in menuItems)
            {
                GuidUdi contentUdi = new GuidUdi("element", Guid.NewGuid());

                var mainItemData = new Dictionary<string, object>
                {
                    { "contentTypeKey", parentItemType.Key.ToString() },
                    { "linkText", menuItem.LinkText },
                    //{ "linkDestination", menuItem.LinkDestination },
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

                mainContentData.Add(mainItemData);

                if (menuItem.HeaderMenuChildItems != null)
                {

                    var childMenuBlock = GenerateChildMenuBlockList(menuItem.HeaderMenuChildItems, chidlItemType);
                    mainContentData.Add(new Dictionary<string, object> { { "tprHeaderMenuChildItems", childMenuBlock } });
                }

                //mainContentData.Add(mainItemData);

                mainLayoutUdis.Add(new Dictionary<string, string> { { "contentUdi", contentUdi.ToString() } });
            }
            mainBlockList.layout = new TprHeaderMenuBlockListUdi(mainLayoutUdis);
            mainBlockList.contentData = mainContentData;
            mainBlockList.settingsData = new List<Dictionary<string, string>>();

            settings?.SetValue("tprHeaderMenu", JsonConvert.SerializeObject(mainBlockList));

            if (settings != null)
            {
                _contentService.SaveAndPublish(settings);
            }

        }

        public TprHeaderChildMenuBlockList GenerateChildMenuBlockList(List<TprHeaderMenuChildItem> childItems, IContentType contentType)
        {
            var childBlockList = new TprHeaderChildMenuBlockList();
            var childContentData = new List<Dictionary<string, object>>();
            var childLayoutUdis = new List<Dictionary<string, string>>();

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
                childLayoutUdis.Add(new Dictionary<string, string>
                {
                    {"contentUdi", childItemUdi.ToString()},
                });

            }

            childBlockList.layout = new TprHeaderMenuBlockListUdi(childLayoutUdis);
            childBlockList.contentData = childContentData;
            childBlockList.settingsData = new List<Dictionary<string, string>>();

            return childBlockList;

        }
    }


    public class TprHeaderMenuBlockList
    {
        public TprHeaderMenuBlockListUdi? layout { get; set; }
        public List<Dictionary<string, object>>? contentData { get; set; }
        public List<Dictionary<string, string>>? settingsData { get; set; }
    }

    public class TprHeaderMenuBlockListUdi
    {
        [JsonProperty("Umbraco.BlockList")]
        public List<Dictionary<string, string>> _contentUdi { get; set; }

        public TprHeaderMenuBlockListUdi(List<Dictionary<string, string>> contentUdi)
        {

            _contentUdi = contentUdi;
        }
    }

    public class TprHeaderChildMenuBlockList
    {
        public TprHeaderMenuBlockListUdi? layout { get; set; }
        public List<Dictionary<string, object>>? contentData { get; set; }
        public List<Dictionary<string, string>>? settingsData { get; set; }
    }

    public interface ITprHeaderMenuBlockListGenerator
    {

        public void GenerateTprHeaderMenuBlockList(int rootId, int settingsId);

        public TprHeaderChildMenuBlockList GenerateChildMenuBlockList(List<TprHeaderMenuChildItem> childItems, IContentType contentType);
    }


}

