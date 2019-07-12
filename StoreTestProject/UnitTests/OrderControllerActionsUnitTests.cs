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
using WebApplication6.Models.Product;
using Xunit;


namespace StoreTestProject
{
    public class OrderControllerActionsUnitTests
    {

        [Fact]
        public void AddOrder_Returns_JsonResult_AdditionFailed()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            ProductModel model = new ProductModel { ItemId = 1, UserId = 1 , StatusId = 1, NumberOfItems = 1};
            Order order = new Order { Id = 1, ItemId = 1 };
            var cancel = new CancellationToken();

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(m => m.AddOrder(order)).Returns(Task.FromResult(order));
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.AddOrder(model);

            var jsonResult = Assert.IsType<JsonResult>(result);
            string json = JsonConvert.SerializeObject(jsonResult.Value);
            Assert.Equal("\"Order addition failed!\"", json);

        }

        [Fact]
        public void AddOrderFromIndex_Returns_JsonResult_AdditionFailed()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            ProductModel model = new ProductModel { ItemId = 1, UserId = 1, StatusId = 1, NumberOfItems = 1 };
            Order order = new Order { Id = 1, ItemId = 1 };
            var cancel = new CancellationToken();

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();
            mockOrderService.Setup(m => m.AddOrder(order)).Returns(Task.FromResult(order));
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.AddOrderFromIndex(model);

            var jsonResult = Assert.IsType<JsonResult>(result);
            string json = JsonConvert.SerializeObject(jsonResult.Value);
            Assert.Equal("\"Order addition failed!\"", json);

        }

        [Fact]
        public void RemoveOrder_Returns_Error_RedirectToAction()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            Order order = new Order { Id = 1, ItemId = 1 };
            var cancel = new CancellationToken();

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.RemoveOrder(order);

            var jsonResult = Assert.IsType<RedirectToActionResult>(result);

        }

        [Fact]
        public void ShopingCart_Returns_Error_RedirectToAction()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.ShopingCart();

            var jsonResult = Assert.IsType<RedirectToActionResult>(result);

        }

        [Fact]
        public void BillingInfo_Returns_ArgumentNullException()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            OrderListModel listmodel = new OrderListModel { };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var getResultException = Record.Exception(() => Controller.BillingInfo(listmodel));
            Assert.IsType<ArgumentNullException>(getResultException);
        }

        [Fact]
        public void UserInfo_Returns_Error_RedirectToAction()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            OrderListModel listmodel = new OrderListModel { };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            mockUserService.Setup(m => m.GetUserById(user.Id)).Returns(Task.FromResult(user));
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.UserInfo();

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void AddBillingInfo_Returns_RedirectToLogin()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            OrderModel model = new OrderModel { };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();   
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var getResult = Controller.AddBillingInfo(model);
            Assert.IsType<JsonResult>(getResult);
            Assert.Equal("Login", getResult.Value);
        }

        [Fact]
        public void ConfirmOrders_Returns_RedirectToAction_Error()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            OrderModel model = new OrderModel { };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.ConfirmOrder();

            Assert.IsType<RedirectToActionResult>(result);
        }


        [Fact]
        public void Charge_Returns_RedirectToAction_Error()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            OrderListModel listmodel = new OrderListModel { };
            var cancel = new CancellationToken();

            OrderModel model = new OrderModel { };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.Charge("fakeStripeMail","fakeStripeToken", listmodel);

            Assert.IsType<RedirectToActionResult>(result);
        }

        [Fact]
        public void FinaliseOrders_Returns_RedirectToAction_Error()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            OrderListModel listmodel = new OrderListModel { };
            var cancel = new CancellationToken();

            OrderModel model = new OrderModel { };

            var mockProductService = new Mock<IProductService>();
            var mockUserService = new Mock<IUserService>();
            var mockOrderService = new Mock<IOrderService>();
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var mockManager = new Mock<UserManager<Users>>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new OrderController(mockProductService.Object, mockManager.Object, mockUserService.Object, mockOrderService.Object);

            var result = Controller.FinaliseOrders();

            Assert.IsType<RedirectToActionResult>(result);
        }

    }
}
