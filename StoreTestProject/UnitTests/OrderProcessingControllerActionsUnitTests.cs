using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using StoreData;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApplication6.Controllers;
using WebApplication6.Models.Order;
using Xunit;

namespace StoreTestProject
{
    public class OrderProcessingControllerActionsUnitTests
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
        public void Index_NotNull_Returns_viewResult()
        {

            Users user = new Users { Id = 1, UserName = "fakeUser" };
 
            var cancel = new CancellationToken();

            var mockProductService = new Mock<IProductService>(); 
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderProcessingController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);

        }

        [Fact]
        public void AddInvoice_Failed_Returns_JsonResult_Error()
        {

            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();
            var model = new OrderListModel { BillingInfoId = 1 };
   
            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();   
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderProcessingController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.AddInvoice(model);

            var viewResult = Assert.IsType<JsonResult>(result);
            string json = JsonConvert.SerializeObject(viewResult.Value);
            Assert.Equal("\"Invoice Addition Failed !\"", json);
        }

        [Fact]
        public void ChangeOrderStatus_Failed_Returns_JsonResult_Error()
        {

            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();
            var model = new OrderListModel { BillingInfoId = 1 };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderProcessingController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.ChangeOrderStatus(model);

            var viewResult = Assert.IsType<JsonResult>(result);
            string json = JsonConvert.SerializeObject(viewResult.Value);
            Assert.Equal("\"Billing Info Id Not found\"", json);
        }
 

        [Theory, InlineData (1)]
        public void Invoice_Failed_Returns_Catch_RedirectToAction_Error(int id)
        {

            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();
     
            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderProcessingController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.Invoice(id);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void InvoicePDF_Failed_Returns_Catch_RedirectToAction_Error()
        {
            InvoiceListModel model = new InvoiceListModel { Id = 1, InvoiceId = 1 };
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderProcessingController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.InvoicePdf(model);

            var viewResult = Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void InvoiceList_Returns_ViewResult_True()
        {
            Invoice invoice= new Invoice { Id = 1 , UserId = 1 };
            var invoicesToReturn = new List<Invoice> { invoice };
            OrderListModel model = new OrderListModel { UserId = 1  };
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(m => m.GetUserById(model.UserId)).Returns(Task.FromResult(user));
            var mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(m => m.GetInvoices(user.UserName)).Returns(invoicesToReturn);
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderProcessingController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.InvoiceList(model);

            var viewResult = Assert.IsType<ViewResult>(result);
        }


        [Fact]
        public void GetUsers_Returns_JsonResult_True()
        {
 
 
            OrderListModel model = new OrderListModel { UserSrch = "fakeUser"};
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var usersToReturn = new List<Users> { user };
            var cancel = new CancellationToken();

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(m => m.GetUserNames(user.UserName)).Returns((usersToReturn));
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderProcessingController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.GetUsers(model);

            var viewResult = Assert.IsType<JsonResult>(result);
        }


    }
}
