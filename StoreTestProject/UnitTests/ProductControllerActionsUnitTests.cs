using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using StoreData;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using WebApplication6.Controllers;
using WebApplication6.Models.Product;
using Xunit;

namespace StoreTestProject
{
    public class ProductControllerActionsUnitTests
    {

        public class MockData
        {
            public static Manufacturer manufacturer = new Manufacturer
            {
                Id = 1,
                Name = "manufacturerName"
            };

            public static ItemDepartment itemDept = new ItemDepartment
            {
                Id = 1,
                DeptName = "DeptName"
            };

            public static ItemType itemType = new ItemType
            {
                Id = 1,
                Name = "itemTypeName",
                ItemTypeHeaderImageUrl = "itemTypeHeaderImageUrl",
                ItemGroupName = "itemGroupName"
            };

            public static ItemTypeSub itemSubType = new ItemTypeSub
            {
                Id = 1,
                SubTypeName = "itemSubTypeName",
                ItemType = itemType
            };

            public static Specs spec = new Specs
            {
                Id = 1,
                Description = "Description",
                Specification = "Specification",
            };

            public static Model model = new Model
            {
                Id = 1,
                ItemDepartment = itemDept,
                SpecsId = spec,
                TypeId = itemType,
                Name = "modelName"
            };

            public static Items item = new Items
            {
                Id = 1,
                Availability = 1,
                Color = "itemColor",
                Price = 12,
                Discount = 0,
                ManuModel = manufacturer,
                Model = model,
                ItemTypeSub = itemSubType
            };

            public static Reviews review = new Reviews
            {
                Id = 1,
                DayOfReview = DateTime.UtcNow,
                Item = item,
                Rating = 5.0,
                Text = "reviewText"


            };

            public static ImageUrls imageUrl = new ImageUrls
            {
                Id = 1,
                Item = item,
                Url = "imageUrl"
            };

        }


        [Theory,
        InlineData("Electric Guitar", "ELECTRIC GUITARS")]
        public void Index_NotNull_Returns_viewResult(string _name , string _itemGroup)
        {

            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var returnItems = new List<Items> { MockData.item };
            var cancel = new CancellationToken();

            var mockService = new Mock<IProductService>();
            mockService.Setup(m => m.GetByType(_name)).Returns(returnItems);
            mockService.Setup(m => m.GetItemGroup(MockData.model.Id)).Returns("itemGroupName");

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new ProductController(mockService.Object, mockManager.Object);

            var result = Controller.Index(_name, _itemGroup);

            var viewResult = Assert.IsType<ViewResult>(result); 

        }

        [Theory,
        InlineData(1)]
        public void Detail_NotNull_Returns_viewResult(int id)
        {

            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var mockService = new Mock<IProductService>();
            mockService.Setup(m => m.GetById(id)).Returns(MockData.item);
            mockService.Setup(m => m.GetItemGroup(MockData.model.Id)).Returns("itemGroupName");

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new ProductController(mockService.Object, mockManager.Object);

            var result = Controller.Detail(id);

            /* var viewResult = Assert.IsType<ViewResult>(result); */
            var viewResult2 = Assert.IsType<RedirectToActionResult>(result);

        }

        [Fact]
        public void GetHistory_NotNull_Returns_partialViewResult()
        {

            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var returnItems = new List<Items> { MockData.item };
            var cancel = new CancellationToken();
            var returnIds = new List<int> { 1, 2, 3 };

            var mockService = new Mock<IProductService>();
            mockService.Setup(m => m.GetHistoryByUserId(user.Id)).Returns(returnIds);
            mockService.Setup(m => m.GetByHistoryId(returnIds)).Returns(returnItems);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new ProductController(mockService.Object, mockManager.Object);

            var result = Controller.GetHistory();
            
            var viewResult = Assert.IsType<PartialViewResult>(result);

        }

        [Fact]
        public void GetRelatedItems_NotNull_Returns_partialViewResult()
        {

            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var returnItems = new List<Items> { MockData.item };
            var cancel = new CancellationToken();

            var mockService = new Mock<IProductService>();
            mockService.Setup(m => m.GetSubCategory(MockData.itemSubType.Id)).Returns(MockData.itemSubType.SubTypeName);
            mockService.Setup(m => m.GetRelatedItems(MockData.itemSubType.SubTypeName)).Returns(returnItems);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new ProductController(mockService.Object, mockManager.Object);

            var result = Controller.GetRelatedItems(1);

            var viewResult = Assert.IsType<PartialViewResult>(result);

        }

        [Fact]
        public void AddReview_NotNull_Returns_JsonResult_RevAdded()
        {

            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var returnItems = new List<Items> { MockData.item };
            var cancel = new CancellationToken();

            var model = new ProductModel { ItemId = 1 , Review = MockData.review};

            var mockService = new Mock<IProductService>();
            mockService.Setup(m => m.GetById(model.ItemId)).Returns(MockData.item);
            mockService.Setup(m => m.AddReview(MockData.review));

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new ProductController(mockService.Object, mockManager.Object);

            var result = Controller.AddReview(model);

            var viewResult = Assert.IsType<JsonResult>(result);

            string json = JsonConvert.SerializeObject(result.Value);
            Assert.Equal( "\"Review Added!\"" , json);

        }

        [Fact]
        public void AddReview_NotNull_Returns_JsonResult_RevFailed()
        {

            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var returnItems = new List<Items> { MockData.item };
            var cancel = new CancellationToken();

            var model = new ProductModel {/* ItemId = 1, Review = MockData.review */};

            var mockService = new Mock<IProductService>();
            mockService.Setup(m => m.GetById(model.ItemId)).Returns(MockData.item);
            mockService.Setup(m => m.AddReview(MockData.review)); 

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new ProductController(mockService.Object, mockManager.Object);

            var result = Controller.AddReview(model);

            var viewResult = Assert.IsType<JsonResult>(result);

            string json = JsonConvert.SerializeObject(result.Value);
            Assert.Equal("\"Review addition failed!\"", json);

        }

        [Fact]
        public void RefreshItems_NotNull_Returns_JsonResult_RevFailed()
        {

            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var returnItems = new List<Items> { MockData.item };
            var cancel = new CancellationToken();

            var prodctIds = new List<int> { MockData.model.Id };
            var manuNames = new List<string> { MockData.manufacturer.Name };
            var subCategoryes = new List<string> { MockData.itemSubType.SubTypeName };

            var model = new ProductListModel { ProductId = prodctIds, Name = MockData.model.Name, ManufacturerList = manuNames,SubCategories = subCategoryes};

            var mockService = new Mock<IProductService>();

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new ProductController(mockService.Object, mockManager.Object);

            var result = Controller.RefreshItems(model);

            var viewResult = Assert.IsType<JsonResult>(result);
            Assert.IsType<ProductListModel>(result.Value);
 

        }


    }
}
