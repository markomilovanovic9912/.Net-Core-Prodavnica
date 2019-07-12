using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreData;
using StoreData.Models;
using Stripe;
using WebApplication6.Models.Order;
using WebApplication6.Models.Product;

namespace WebApplication6.Controllers
{
    public class OrderController : Controller
    {


        private IProductService _products;
        private UserManager<Users> _userManager;
        private IUserService _users;
        private IOrderService _orders;

        public OrderController(IProductService products, UserManager<Users> userManager, IUserService users, IOrderService orders)
        {
            _products = products;
            _userManager = userManager;
            _users = users;
            _orders = orders;

        }

        [Authorize(Policy = "IsUser")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "IsUser")]
        [HttpPost]
        public JsonResult AddOrder([FromBody]ProductModel productModel)
        {

            if (ModelState.IsValid) {

                try
                {
                    var userId = _userManager.GetUserId(User);

                    var order = new Order
                    {
                        UserId = Int32.Parse(userId),
                        ItemId = productModel.ItemId,
                        StatusId = 1,
                        NumberOfItems = productModel.NumberOfItems,
                        BillingInfoId = productModel.BillingInfoId,
                        InvoiceId = productModel.InvoiceId
                    };

                    var _orderToAdd = _orders.AddOrder(order);


                    return Json("Order Added Sucesffuly!");
               }
                catch { return Json("Order addition failed!"); }
            }

            else { return Json("Something went wrong!"); }


        }

        [Authorize(Policy = "IsUser")]
        [HttpPost]
        public JsonResult AddOrderFromIndex([FromBody]ProductModel productModel)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    
                    var userId = _userManager.GetUserId(User);

                    var order = new Order
                    {
                        UserId = Int32.Parse(userId),
                        ItemId = productModel.ItemId,
                        StatusId = 1,
                        NumberOfItems = 1,
                        BillingInfoId = productModel.BillingInfoId,
                        InvoiceId = productModel.InvoiceId
                    };

                    var _orderToAdd = _orders.AddOrder(order);


                    return Json("Order Added Sucesffuly!");
                }


                catch { return Json("Order addition failed!"); }
            }

            else { return Json("Something went wrong!"); }
        }

        [Authorize(Policy = "IsUser")]
        [HttpPost]
        public IActionResult RemoveOrder(Order order)
        {

            var _orderToAdd = _orders.RemoveOrder(order.Id);

            return RedirectToAction("ShopingCart");

        }

        [Authorize(Policy = "IsUser")]
        public IActionResult ShopingCart()
        {
            try
            {
                var user = _userManager.GetUserId(User);

                if (user != null)
                {
                    var userId = Int32.Parse(user);

                    var orders = _orders.GetOrdersByUser(userId);

                    var ordersModel = orders.Select(o => new OrderModel
                    {
                        Id = o.Id,
                        ItemId = o.ItemId,
                        StatusId = o.StatusId,
                        UserId = o.UserId,
                        DateAdded = o.DateAdded,
                        Item = o.Items,
                        Manufacturer = _products.GetProductName(o.ItemId),
                        Model = _products.GetModelName(o.ItemId),
                        NumberOfItems = o.NumberOfItems,
                        FirstImage = _products.GetFirstImage(o.ItemId),
                        BillingInfoId = o.BillingInfoId,


                    });

                    var ordersList = new OrderListModel
                    {
                        Orders = ordersModel

                    };
                    
                    this.AddBreadCrumb(new BreadCrumb
                    {
                        Title = "Shoping Cart",
                        Url = string.Format("ShopingCart"),
                        Order = 1
                    });

                    return View(ordersList);
                }

                else { return RedirectToAction("Login", "Account"); }

            }

            catch { return RedirectToAction("Error", "Home", new { errorDetails = "Something went wrong!" }); }

        }

        [Authorize(Policy = "IsUser")]
        public IActionResult BillingInfo(OrderListModel listmodel)
        {
            /*if (ModelState.IsValid)
            {*/
                var ordersList = new OrderModel
                {
                    SumTotal = listmodel.SumTotal

                };

            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Shoping Cart",
                Url = Url.Action("ShopingCart", "Order" ),
                Order = 1
            });

            this.AddBreadCrumb(new BreadCrumb
            {
                Title = "Billing Info",
                Url = Url.Action("BillingInfo", "Order"),
                Order = 2
            });

            return View(ordersList);
          /*  }

            else {return }*/
        }

        [Authorize(Policy = "IsUser")]
        [HttpPost]
        public IActionResult UserInfo()
        {
            if (_userManager.GetUserId(User) != null)
            {
               
                var user = _users.GetUserById(Int32.Parse(_userManager.GetUserId(User)));

                var userGet = new Users
                {
                    Id = user.Result.Id,
                    UserName = user.Result.UserName,
                    LastName = user.Result.LastName,
                    FirstName = user.Result.FirstName,
                    MiddleInitial = user.Result.MiddleInitial,
                    PhoneNumber = user.Result.PhoneNumber,
                    Adress = user.Result.Adress,
                    City = user.Result.City,
                    CountryOrState = user.Result.CountryOrState,
                    PhoneNumberConfirmed = user.Result.PhoneNumberConfirmed,
                    Email = user.Result.Email,
                    EmailConfirmed = user.Result.EmailConfirmed,
                    UserClaims = user.Result.UserClaims,
                    UserRoles = user.Result.UserRoles,
                };

                return Json(userGet);
            }

            else return RedirectToAction("Login", "Account");
        }

        [Authorize(Policy = "IsUser")]
        [HttpPost]
        public JsonResult AddBillingInfo([FromBody]OrderModel info)
        {

             if (ModelState.IsValid)
             {
                 var user = _userManager.GetUserId(User);

                 if (user == null) { return Json(info.RedirectUrl = "Login");}

                 else
                 {         
                    var userId = Int32.Parse(user);

                    var billingInfo = new BillingInfo
                    {
                        Adress = info.Adress,
                        City = info.City,
                        CountryOrState = info.CountryOrState,
                        Email = info.Email,
                        PhoneNumber = info.PhoneNumber
                    };

                    var addBillingInfo = _orders.AddBillingInfo(billingInfo);

                    var updateOrders = _orders.UpdateOrders(info.PaymentMethod, addBillingInfo.Result.Id, userId);

                    return Json(info.RedirectUrl = "FinaliseOrders");
              }
           }

            else return Json(info.RedirectUrl = "ShopingCart");
        }

        [Authorize(Policy = "IsUser")]
        [HttpPost]
        public IActionResult ConfirmOrder()
        {
            try
            {
                var user = _userManager.GetUserId(User);

                if (user != null)
                {
                    var userId = Int32.Parse(user);

                    var confirmOrder = _orders.ChangeOrderStatus(userId, 3);

                    return RedirectToAction("ShopingCart");
                }

                else { return RedirectToAction("Login", "Account"); }
            }

            catch
            {   return RedirectToAction("Error", "Home" , new { errorDetails = "Something went wrong!"}); } 

        }

        [Authorize(Policy = "IsUser")]
        public IActionResult Charge(string stripeEmail, string stripeToken, OrderListModel order)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.GetUserId(User);

                if (user == null) { return RedirectToAction("Login", "Account"); }
                else
                {
                    var userId = Int32.Parse(_userManager.GetUserId(User));

                    int total = Convert.ToInt32(order.SumTotal);

                    var customers = new StripeCustomerService();
                    var charges = new StripeChargeService();

                    var customer = customers.Create(new StripeCustomerCreateOptions
                    {
                        Email = stripeEmail,
                        SourceToken = stripeToken
                    });

                    var charge = charges.Create(new StripeChargeCreateOptions
                    {
                        Amount = total * 100,
                        Description = "Sample Charge",
                        Currency = "eur",
                        CustomerId = customer.Id
                    });

                    var changeStatus = _orders.ChangeOrderStatus(userId, 2);

                    return RedirectToAction("FinaliseOrders");
                }
            }

            else { return RedirectToAction("Error", "Home", new { errorDetail = "Something Went Wrong!" }); }
        }

        [Authorize(Policy = "IsUser")]
        public IActionResult FinaliseOrders() {

            try {
                var user = _userManager.GetUserId(User);
                if (user != null)
                {
                    var userId = Int32.Parse(user);

                    var orders = _orders.GetOrdersByUser(userId);

                    var ordersModel = orders.Select(o => new OrderModel
                    {
                        Id = o.Id,
                        ItemId = o.ItemId,
                        StatusId = o.StatusId,
                        UserId = o.UserId,
                        DateAdded = o.DateAdded,
                        Item = o.Items,
                        Manufacturer = _products.GetProductName(o.ItemId),
                        Model = _products.GetModelName(o.ItemId),
                        NumberOfItems = o.NumberOfItems,
                        FirstImage = _products.GetFirstImage(o.ItemId),
                        PaymentMethod = _orders.GetPaymentMethod(o.PaymentMethodId)

                    });

                    var ordersList = new OrderListModel
                    {
                        Orders = ordersModel

                    };

                    this.AddBreadCrumb(new BreadCrumb
                    {
                        Title = "Shoping Cart",
                        Url = Url.Action("ShopingCart", "Order"),
                        Order = 0
                    });

                    this.AddBreadCrumb(new BreadCrumb
                    {
                        Title = "Billing Info",
                        Url = Url.Action("BillingInfo", "Order"),
                        Order = 1
                    });

                    this.AddBreadCrumb(new BreadCrumb
                    {
                        Title = "Finalise Orders",
                        Url = Url.Action("FinaliseOrders", "Order"),
                        Order = 2
                    });

                    return View(ordersList);
                }

                else { return RedirectToAction("Login", "Account"); }

            }

            catch { return RedirectToAction("Error", "Home", new { errorDetails = "Something went wrong!" }); }
           

        }

        [Authorize(Policy = "IsUser")]
        public IActionResult CustomerPastOrders()
        {
            var user = _userManager.GetUserId(User);

            var userId = _users.GetUserById(Int32.Parse(user));

            var invoices = _orders.GetInvoices(userId.Result.UserName);

            var invoicesToAdd = invoices.Select(i => new Invoice
            {
                Id = i.Id,
                InvoiceDate = i.InvoiceDate,
                User = i.User,
                Orders = i.Orders
            });

            var pastInvoices = new OrderListModel
            {
                Invoices = invoicesToAdd
            };

            return View(pastInvoices); 
        }

    }
}