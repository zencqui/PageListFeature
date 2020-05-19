using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sitecore;
using Sitecore.Abstractions;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Xml;
using SkillsAssessment.Feature.PageList.Models;
using SkillsAssessment.Feature.PageList.Repository;

namespace SkillsAssessment.Feature.PageList.Controllers
{
    public class PageListComponentController : Controller
    {
        private readonly ITeaserRepository _repository;

        public PageListComponentController(ITeaserRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new Teasers(_repository.GetTeasers(Sitecore.Context.Item));
            return PartialView("/Views/PageListComponent/Index.cshtml", model);
        }

    }
}