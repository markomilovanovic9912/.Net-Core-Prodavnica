using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using StoreData;
using StoreData.Models;
using StoreServices;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using WebApplication6.Controllers;
using WebApplication6.Models.Order;
using Xunit;

namespace StoreTestProject.IntegrationTests
{
    public class OrderProcessingControllerIntegrationTests : IDisposable
    {
        StoreContext _context;


        public OrderProcessingControllerIntegrationTests()
        {
           
            var provider = new ServiceCollection().AddEntityFrameworkSqlServer().BuildServiceProvider();

            var options = new DbContextOptionsBuilder<StoreContext>();

            options.UseSqlServer($"Server=(localdb)\\MSSQLLocalDB;Database=Store_DTB_{Guid.NewGuid()};Trusted_Connection=True;MultipleActiveResultSets=True; ")
                .UseInternalServiceProvider(provider).EnableSensitiveDataLogging();

            _context = new StoreContext(options.Options);
            _context.Database.Migrate();

        }

        [Fact]
        public void OrderController_AddInvoice_Success()
        {
            #region MockData
            Users user = new Users { UserName = "fakeUser", Email = "fakeEmail" };
            var cancel = new CancellationToken();

            var User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
               {
                        new Claim(ClaimTypes.NameIdentifier, "1"),
               }));

            BillingInfo billingInfo = new BillingInfo
            {
                Adress = "fakeAdress",
                City = "fakeCity",
                CountryOrState = "fakeCountry",
                Email = "fakeEmail",
                PhoneNumber = "fakeNumber",
            };

           Invoice invoice = new Invoice
            {
                InvoiceDate = DateTimeOffset.UtcNow,
                UserId = 1
            };

            Manufacturer manufacturer = new Manufacturer
            {
                /* Id = 1,*/
                Name = "manufacturerName"
            };

            ItemDepartment itemDept = new ItemDepartment
            {
                /*   Id = 1,*/
                DeptName = "DeptName"
            };

            ItemType itemType = new ItemType
            {
                /* Id = 1,*/
                Name = "itemTypeName",
                ItemTypeHeaderImageUrl = "itemTypeHeaderImageUrl",
                ItemGroupName = "itemGroupName"
            };

            ItemTypeSub itemSubType = new ItemTypeSub
            {
                /*   Id = 1,*/
                SubTypeName = "itemSubTypeName",
                ItemType = itemType
            };

            Specs spec = new Specs
            {
                /*  Id = 1, */
                Description = "Description",
                Specification = "Specification",
            };

            Model model = new Model
            {
                /*  Id = 1,*/
                ItemDepartment = itemDept,
                SpecsId = spec,
                TypeId = itemType,
                Name = "modelName"
            };

            Items item = new Items
            {
                /* Id = 1, */
                Availability = 1,
                Color = "itemColor",
                Price = 12,
                Discount = 0,
                ManuModel = manufacturer,
                Model = model,
                ItemTypeSub = itemSubType
            };

            PaymentMethod paymentMethod = new PaymentMethod { Method = "fakeMethod" };
            Status status = new Status { StatusText = "fakeStatus" };

            Order order = new Order
            {
                ItemId = 1,
                UserId = 1,
                NumberOfItems = 1,
                StatusId = 1,
                BillingInfoId = 1,
                PaymentMethodId = 1,
                InvoiceId = 1 
            };
            #endregion

            OrderListModel addInvoiceModel = new OrderListModel{ BillingInfoId = 1} ;

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);
            var orderService = new OrderService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            var Controller = new OrderProcessingController(productService, manager, userService, orderService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = User }
                }
            };

            var addUser = userService.AddUser(user);
            var addItem = productService.AddItem(item, spec, model);
            var addFirstInvoice = orderService.AddInvoice(invoice);
            var addStatus = orderService.AddStatus(status);
            var addStatus2 = orderService.AddStatus(status);
            var addStatus3 = orderService.AddStatus(status);
            var addStatus4 = orderService.AddStatus(status);
            var addPaymentMethod = orderService.AddPaymentMethod(paymentMethod);
            var addBillInfo = orderService.AddBillingInfo(billingInfo);
            var addOrder = orderService.AddOrder(order);
            var addInvoice = Controller.AddInvoice(addInvoiceModel);

            var lastInvoice = orderService.GetLastInvoice();

            Assert.Equal(2, lastInvoice.Id);
            var jsonResult = Assert.IsType<JsonResult>(addInvoice);
            string json = JsonConvert.SerializeObject(jsonResult.Value);
            Assert.NotEqual("Invoice Addition Failed !", json);
            Assert.NotEqual("Something went wrong!", json);
        }

        [Fact]
        public void OrderController_ChangeOrderStatus_Success()
        {
            #region MockData
            Users user = new Users { UserName = "fakeUser", Email = "fakeEmail" };
            var cancel = new CancellationToken();

            var User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
               {
                        new Claim(ClaimTypes.NameIdentifier, "1"),
               }));

            BillingInfo billingInfo = new BillingInfo
            {
                Adress = "fakeAdress",
                City = "fakeCity",
                CountryOrState = "fakeCountry",
                Email = "fakeEmail",
                PhoneNumber = "fakeNumber",
            };

            Invoice invoice = new Invoice
            {
                InvoiceDate = DateTimeOffset.UtcNow,
                UserId = 1
            };

            Manufacturer manufacturer = new Manufacturer
            {
                /* Id = 1,*/
                Name = "manufacturerName"
            };

            ItemDepartment itemDept = new ItemDepartment
            {
                /*   Id = 1,*/
                DeptName = "DeptName"
            };

            ItemType itemType = new ItemType
            {
                /* Id = 1,*/
                Name = "itemTypeName",
                ItemTypeHeaderImageUrl = "itemTypeHeaderImageUrl",
                ItemGroupName = "itemGroupName"
            };

            ItemTypeSub itemSubType = new ItemTypeSub
            {
                /*   Id = 1,*/
                SubTypeName = "itemSubTypeName",
                ItemType = itemType
            };

            Specs spec = new Specs
            {
                /*  Id = 1, */
                Description = "Description",
                Specification = "Specification",
            };

            Model model = new Model
            {
                /*  Id = 1,*/
                ItemDepartment = itemDept,
                SpecsId = spec,
                TypeId = itemType,
                Name = "modelName"
            };

            Items item = new Items
            {
                /* Id = 1, */
                Availability = 1,
                Color = "itemColor",
                Price = 12,
                Discount = 0,
                ManuModel = manufacturer,
                Model = model,
                ItemTypeSub = itemSubType
            };

            PaymentMethod paymentMethod = new PaymentMethod { Method = "fakeMethod" };
            Status status = new Status { StatusText = "fakeStatus" };

            Order order = new Order
            {
                ItemId = 1,
                UserId = 1,
                NumberOfItems = 1,
                StatusId = 1,
                BillingInfoId = 1,
                PaymentMethodId = 1,
                InvoiceId = 1
            };
            #endregion

            OrderListModel changeOrderModel = new OrderListModel { BillingInfoId = 1, OrderStatus = 2 };

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);
            var orderService = new OrderService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            var Controller = new OrderProcessingController(productService, manager, userService, orderService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = User }
                }
            };

            var addUser = userService.AddUser(user);
            var addItem = productService.AddItem(item, spec, model);
            var addInvoice = orderService.AddInvoice(invoice);
            var addStatus = orderService.AddStatus(status);
            var addStatus2 = orderService.AddStatus(status);
            var addPaymentMethod = orderService.AddPaymentMethod(paymentMethod);
            var addBillInfo = orderService.AddBillingInfo(billingInfo);
            var addOrder = orderService.AddOrder(order);
            var changeOrderStatus = Controller.ChangeOrderStatus(changeOrderModel);
            var getOrders = orderService.GetAllOrders();
 
            Assert.Equal(2, getOrders.First().StatusId);
            var jsonResult = Assert.IsType<JsonResult>(changeOrderStatus);
            string json = JsonConvert.SerializeObject(jsonResult.Value);
            Assert.Equal("\"Orders Status Changed !\"", json);
            Assert.NotEqual("Billing Info Id Not found", json);
            Assert.NotEqual("Something went wrong!", json);
        }




        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }

    }
}
