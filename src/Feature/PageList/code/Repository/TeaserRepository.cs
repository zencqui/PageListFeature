using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Abstractions;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;
using SkillsAssessment.Feature.PageList.Models;

namespace SkillsAssessment.Feature.PageList.Repository
{
    public class TeaserRepository : ITeaserRepository
    {
        private readonly BaseLinkManager _linkManager;
        private readonly BaseMediaManager _mediaManager;
        public TeaserRepository(BaseLinkManager linkManager, BaseMediaManager mediaManager)
        {
            _linkManager = linkManager;
            _mediaManager = mediaManager;
        }

        public IEnumerable<Teaser> GetTeasers(Item sourceItem)
        {
            if (sourceItem == null)
                throw new ArgumentNullException(nameof(sourceItem));

            var items = sourceItem.Children ?? Enumerable.Empty<Item>();
            var teasers = new List<Teaser>();

            foreach (var item in items)
            {
                var imageField = (ImageField)item.Fields["Image"];
                var mediaItem = imageField.MediaItem == null ? null : new MediaItem(imageField?.MediaItem);

                teasers.Add(new Teaser
                {
                    ItemUrl = _linkManager.GetItemUrl(item),
                    Title = item["Title"],
                    ImageUrl = mediaItem == null ? string.Empty : _mediaManager.GetMediaUrl(mediaItem),
                    Description = item["Description"]
                });
            }

            return teasers;
        }
    }
}