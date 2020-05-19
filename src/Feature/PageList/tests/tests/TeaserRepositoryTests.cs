using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sitecore.Abstractions;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using SkillsAssessment.Feature.PageList.Models;
using SkillsAssessment.Feature.PageList.Repository;
using Xunit;

namespace SkillsAssessment.Feature.PageList.Tests
{
    public class TeaserRepositoryTests
    {
        [Fact]
        public void GetTeasers_ItemIsNull_RetrunsEmpty()
        {
            var linkManager = Substitute.For<BaseLinkManager>();
            var mediaManager = Substitute.For<BaseMediaManager>();
            var sut = new TeaserRepository(linkManager, mediaManager);

            var result = Assert.Throws<ArgumentNullException>(() => sut.GetTeasers(null));

            Assert.Contains("Value cannot be null", result.Message);
        }

        [Fact]
        public void GetTeasers_ItemChildrenIsNull_RetrunsEmpty()
        {
            var linkManager = Substitute.For<BaseLinkManager>();
            var mediaManager = Substitute.For<BaseMediaManager>();
            var sut = new TeaserRepository(linkManager, mediaManager);

            var db = Substitute.For<Database>();
            var item = Substitute.For<Item>(ID.NewID, ItemData.Empty, db);
            item.Children.ReturnsNull();

            var result = sut.GetTeasers(item);

            Assert.Empty(result);
        }


        [Fact]
        public void GetTeasers_ItemChildrenIsNotNull_RetrunsTeasers()
        {
            var db = Substitute.For<Database>();

            var item = CreateItem(db);
            var imageItem1 = CreateItem(db);
            var imageItem2 = CreateItem(db);

            var article1 = CreateItem(db);
            SetItemField(article1, "Image", $"<image mediaid=\"{imageItem1.ID}\" />");
            SetItemField(article1,"Title", "article1");
            SetItemField(article1, "Description", "article1 description");

            var article2 = CreateItem(db);
            SetItemField(article2, "Image", $"<image mediaid=\"{imageItem2.ID}\" />");
            SetItemField(article2, "Title", "article2");
            SetItemField(article2, "Description", "article2 description");

            var childrenCollection = new List<Item>();
            childrenCollection.Add(article1);
            childrenCollection.Add(article1);

            var children = Substitute.For<ChildList>(item, childrenCollection);
            item.Children.Returns(children);

            var linkManager = Substitute.For<BaseLinkManager>();
            linkManager.GetItemUrl(article1).Returns("link1");
            linkManager.GetItemUrl(article2).Returns("link2");

            var mediaManager = Substitute.For<BaseMediaManager>();
            mediaManager.GetMediaUrl(imageItem1).Returns("imageurl1");
            mediaManager.GetMediaUrl(imageItem2).Returns("imageurl2");

            var sut = new TeaserRepository(linkManager, mediaManager);

            var result = sut.GetTeasers(item);

            Assert.NotEmpty(result);
        }

        private Item CreateItem(Database database = null)
        {
            var db = database ?? Substitute.For<Database>();

            var item = Substitute.For<Item>(ID.NewID, ItemData.Empty, db);
            var fields = Substitute.For<FieldCollection>(item);
            item.Fields.Returns(fields);

            db.GetItem(item.ID).Returns(item);
            db.GetItem(item.ID.ToString()).Returns(item);

            return item;
        }

        private void SetItemField(Item item, string fieldName, string fieldValue)
        {
            item[fieldName].Returns(fieldValue);

            var field = Substitute.For<Field>(ID.NewID, item);
            field.Database.Returns(item.Database);
            field.Value = fieldValue;
            item.Fields[fieldName].Returns(field);
        }
    }
}
