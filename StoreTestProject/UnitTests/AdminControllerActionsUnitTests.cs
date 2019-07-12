using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StoreData;
using StoreData.Models;
using System;
using System.Threading;
using WebApplication6.Controllers;
using WebApplication6.Models.Admin;
using Xunit;

namespace StoreTestProject
{
    public class AdminControllerActionsUnitTests  
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

        [Fact]
        public void Index_Returns_ViewResult()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var manufac = new Manufacturer { Id = 1, Name = "manuf" };
            var adminModel = new AdminModel { Manufacturer = manufac };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(mockProductService.Object, mockManager.Object, mockUserService.Object, null);

            var index = Controller.Index();
            Assert.IsType<ViewResult>(index);
        }


        [Fact]
        public void GetSubTypes_Returns_JsonResult()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var manufac = new Manufacturer { Id = 1, Name = "manuf" };
            var adminModel = new AdminModel { ItemTypeName = "typeName" };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(mockProductService.Object, mockManager.Object, mockUserService.Object, null);

            var subType = Controller.GetSubTypes(adminModel);
            Assert.IsType<JsonResult>(subType);
        }

        [Fact]
        public void GetModels_Returns_JsonResult()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var manufac = new Manufacturer { Id = 1, Name = "manuf" };
            var adminModel = new AdminModel { ItemTypeName = "typeName" };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(mockProductService.Object, mockManager.Object, mockUserService.Object, null);

            var models = Controller.GetSubTypes(adminModel);
            Assert.IsType<JsonResult>(models);
        }


        [Fact]
        public void GetSpecs_Returns_JsonResult()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();       

            var manufac = new Manufacturer { Id = 1, Name = "manuf" };
            var adminModel = new AdminModel { ModelName = "modelName" };

            var mockProductService = new Mock<IProductService>();
            mockProductService.Setup(m => m.GetItemByModelName("modelName")).Returns(MockData.item);
            mockProductService.Setup(m => m.GetSpecsByName("modelName")).Returns(MockData.spec);  
            var mockUserService = new Mock<IUserService>();

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(mockProductService.Object, mockManager.Object, mockUserService.Object, null) ;

            var specs = Controller.GetSpecs(adminModel);
            Assert.IsType<JsonResult>(specs);
        }

        [Fact]
        public void GetReviews_Returns_JsonResult()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var manufac = new Manufacturer { Id = 1, Name = "manuf" };
            var adminModel = new AdminModel { ModelName = "modelName" };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(mockProductService.Object, mockManager.Object, mockUserService.Object, null);

            var reviews = Controller.GetReviews(adminModel);
            Assert.IsType<JsonResult>(reviews);
        }

        [Fact]
        public void GetUserNames_Returns_JsonResult()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var manufac = new Manufacturer { Id = 1, Name = "manuf" };
            var adminModel = new AdminModel { UserSearch = "fakeUser" };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(mockProductService.Object, mockManager.Object, mockUserService.Object, null);

            var users = Controller.GetUserNames(adminModel);
            Assert.IsType<JsonResult>(users);
        }

        [Fact]
        public void GetUser_Returns_NullReferenceException()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var manufac = new Manufacturer { Id = 1, Name = "manuf" };
            var adminModel = new AdminModel { UserName = "fakeUser" };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(mockProductService.Object, mockManager.Object, mockUserService.Object, null);

            var getResultException = Record.Exception(() => Controller.GetUser(adminModel));
            Assert.IsType<NullReferenceException>(getResultException);
        }

        [Fact]
        public void GetClaims_Returns_NullReferenceException()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var manufac = new Manufacturer { Id = 1, Name = "manuf" };
            var adminModel = new AdminModel { UserName = "fakeUser" };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(mockProductService.Object, mockManager.Object, mockUserService.Object, null);

            var getResultException = Record.Exception(() => Controller.GetClaims(adminModel));
            Assert.IsType<NullReferenceException>(getResultException);
        }

        [Fact]
         public void AddManufacturer_Returns_JsonResult_Added_Succesfully()
        {
             Users user = new Users { Id = 1, UserName = "fakeUser" };
             var cancel = new CancellationToken();

             var manufac = new Manufacturer { Id = 1, Name = "manuf" };
             var adminModel = new AdminModel { Manufacturer = manufac };

             var mockProductService = new Mock<IProductService>();
             var mockUserService = new Mock<IUserService>();

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(mockProductService.Object, mockManager.Object, mockUserService.Object, null);

            var add = Controller.AddManufacturer(adminModel);
            Assert.IsType<JsonResult>(add);
            Assert.Equal("Manufacturer added!",add.Value);

        }




     
    }
}
 