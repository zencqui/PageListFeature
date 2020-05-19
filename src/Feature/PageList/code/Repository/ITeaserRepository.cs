using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using SkillsAssessment.Feature.PageList.Models;

namespace SkillsAssessment.Feature.PageList.Repository
{
    public interface ITeaserRepository
    {
        IEnumerable<Teaser> GetTeasers(Item sourceItem);
    }
}