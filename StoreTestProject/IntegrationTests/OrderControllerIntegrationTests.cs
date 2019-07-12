using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StoreData;
using StoreData.Models;
using StoreServices;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using WebApplication6.Controllers;
using WebApplication6.Models.Order;
using WebApplication6.Models.Product;
using Xunit;

namespace StoreTestProject.IntegrationTests
{
    public class OrderControllerIntegrationTests : IDisposable
    {
        StoreContext _context;

        ProductModel productModel;

        public OrderControllerIntegrationTests()
        {
            #region MockData
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

            Reviews review = new Reviews
            {
                /*    Id = 1,*/
                /*  DayOfReview = DateTime.UtcNow,*/
                Item = item,
                Rating = 5.0,
                Text = "reviewText"


            };

            ImageUrls imageUrl = new ImageUrls
            {
                /* Id = 1,*/
                Item = item,
                Url = "imageUrl"
            };

            BillingInfo billingInfo = new BillingInfo
            {
                Adress = "fakeAdress",
                City = "fakeCity",
                CountryOrState = "fakeCountry",
                Email = "fakeEmail",
                PhoneNumber = "fakeNumber",
            };
            #endregion

            productModel = new ProductModel
            {
                Review = review,
                ItemId = 1,
                UserId = 1,
                NumberOfItems = 1,
                BillingInfoId = 1,
                InvoiceId = 1
            };


            var provider = new ServiceCollection().AddEntityFrameworkSqlServer().BuildServiceProvider();

            var options = new DbContextOptionsBuilder<StoreContext>();

            options.UseSqlServer($"Server=(localdb)\\MSSQLLocalDB;Database=Store_DTB_{Guid.NewGuid()};Trusted_Connection=True;MultipleActiveResultSets=True; ")
                .UseInternalServiceProvider(provider).EnableSensitiveDataLogging();

            _context = new StoreContext(options.Options);
            _context.Database.Migrate();

        }

        [Fact]
        public void OrderController_AddOrder_Success()
        {
            #region MockData
            Users user = new Users {   UserName = "fakeUser",Email = "fakeEmail" };
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
            #endregion

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);
            var orderService = new OrderService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            var Controller = new OrderController(productService, manager, userService, orderService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = User }
                }
            };
            
            var addItem = productService.AddItem(item, spec, model);
            var addUser = userService.AddUser(user);
            var addInvoice = orderService.AddInvoice(invoice);
            var addPaymentMethod = orderService.AddPaymentMethod(paymentMethod);
            var addStatus = orderService.AddStatus(status);
            var addBillInfo = orderService.AddBillingInfo(billingInfo);

            var addOrder = Controller.AddOrder(productModel);
            var getOrders = orderService.GetAllOrders();
            Assert.Single(getOrders);
            Assert.Equal(1, getOrders.First().NumberOfItems);
            Assert.Equal("Order Added Sucesffuly!", addOrder.Value);
        }

        [Fact]
        public void OrderController_AddOrderFromIndex_Success()
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
            #endregion

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);
            var orderService = new OrderService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            var Controller = new OrderController(productService, manager, userService, orderService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = User }
                }
            };

            var addItem = productService.AddItem(item, spec, model);
            var addUser = userService.AddUser(user);
            var addInvoice = orderService.AddInvoice(invoice);
            var addPaymentMethod = orderService.AddPaymentMethod(paymentMethod);
            var addStatus = orderService.AddStatus(status);
            var addBillInfo = orderService.AddBillingInfo(billingInfo);

            var addOrder = Controller.AddOrderFromIndex(productModel);
            var getOrders = orderService.GetAllOrders();
            Assert.Single(getOrders);
            Assert.Equal(1, getOrders.First().NumberOfItems);
            Assert.Equal("Order Added Sucesffuly!", addOrder.Value);
        }

        [Fact]
        public void OrderController_AddBillingInfo_Success()
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

            var order = new Order
            {
                UserId = 1,
                ItemId = 1,
                StatusId = 1,
                NumberOfItems = 1,
                BillingInfoId = 1,
                InvoiceId = 1
            };

            var orderModel = new OrderModel
            {
                Adress = billingInfo.Adress,
                City = billingInfo.City,
                CountryOrState = billingInfo.
                CountryOrState,
                Email = billingInfo.Email,
                PhoneNumber = billingInfo.PhoneNumber,
                PaymentMethod = paymentMethod.Method
            };

            #endregion

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);
            var orderService = new OrderService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            var Controller = new OrderController(productService, manager, userService, orderService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = User }
                }
            };

            var addItem = productService.AddItem(item, spec, model);
            var addUser = userService.AddUser(user);
            var addInvoice = orderService.AddInvoice(invoice);
            var addBillingInfo = Controller.AddBillingInfo(orderModel);
            var addPaymentMethod = orderService.AddPaymentMethod(paymentMethod);
            var addStatus = orderService.AddStatus(status);
            var addOrder = orderService.AddOrder(order);

           
            var getBillingInfo = orderService.GetBillingInfoById(1);   
            Assert.Equal(1, getBillingInfo.Id);
            Assert.Equal("fakeAdress", getBillingInfo.Adress);
            Assert.IsType<JsonResult>(addBillingInfo);
            Assert.Equal("FinaliseOrders", addBillingInfo.Value);
        }

        [Fact]
        public void OrderController_ConfirmOrder_Success()
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
            #endregion

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);
            var orderService = new OrderService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            var Controller = new OrderController(productService, manager, userService, orderService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = User }
                }
            };

            var addItem = productService.AddItem(item, spec, model);
            var addUser = userService.AddUser(user);
            var addInvoice = orderService.AddInvoice(invoice);
            var addPaymentMethod = orderService.AddPaymentMethod(paymentMethod);
            var addStatus = orderService.AddStatus(status);
            var addStatus2 = orderService.AddStatus(status);
            var addStatus3 = orderService.AddStatus(status);
            var addBillInfo = orderService.AddBillingInfo(billingInfo);

            var addOrder = Controller.AddOrder(productModel);
            var confirmOrders = Controller.ConfirmOrder();
            var getOrders = orderService.GetAllOrders();
            Assert.Single(getOrders);
            Assert.Equal(3, getOrders.First().StatusId);
        }

      /*  [Fact]
        public void OrderController_FinaliseOrders_Success()
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
               
                Name = "manufacturerName"
            };

            ItemDepartment itemDept = new ItemDepartment
            {
             
                DeptName = "DeptName"
            };

            ItemType itemType = new ItemType
            { 
                Name = "itemTypeName",
                ItemTypeHeaderImageUrl = "itemTypeHeaderImageUrl",
                ItemGroupName = "itemGroupName"
            };

            ItemTypeSub itemSubType = new ItemTypeSub
            {
             
                SubTypeName = "itemSubTypeName",
                ItemType = itemType
            };

            Specs spec = new Specs
            {
                
                Description = "Description",
                Specification = "Specification",
            };

            Model model = new Model
            { 
               
                ItemDepartment = itemDept,
                SpecsId = spec,
                TypeId = itemType,
                Name = "modelName"
            };

            Items item = new Items
            {
            
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
            OrderListModel modelCharge = new OrderListModel { SumTotal = 100 };
            #endregion

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);
            var orderService = new OrderService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            var Controller = new OrderController(productService, manager, userService, orderService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext() { User = User }
                }
            };

            var addItem = productService.AddItem(item, spec, model);
            var addUser = userService.AddUser(user);
            var addInvoice = orderService.AddInvoice(invoice);
            var addPaymentMethod = orderService.AddPaymentMethod(paymentMethod);
            var addStatus = orderService.AddStatus(status);
            var addStatus2 = orderService.AddStatus(status);
            var addBillInfo = orderService.AddBillingInfo(billingInfo);

            var addOrder = Controller.AddOrder(productModel);
            var confirmOrders = Controller.FinaliseOrders( );
            var getOrders = orderService.GetAllOrders();
            Assert.Single(getOrders);
            Assert.IsType<ViewResult>(confirmOrders);
        }*/



        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
