using System;
using System.Collections.Generic;
using Sitecore.Data.Items;

namespace SkillsAssessment.Feature.PageList.Models
{
    public class Teasers
    {
        public IEnumerable<Teaser> TeaserItems { get; }

        public Teasers(IEnumerable<Teaser> teaserItems)
        {
            if(teaserItems == null)
                throw  new ArgumentNullException(nameof(teaserItems));

            TeaserItems = teaserItems;
        }
    }
}