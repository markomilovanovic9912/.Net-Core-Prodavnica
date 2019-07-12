using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using StoreData;
using StoreData.Models;
using WebApplication6.Models.Order;

namespace WebApplication6.Controllers
{
    public class OrderProcessingController : Controller
    {
        private IProductService _products;
        private UserManager<Users> _userManager;
        private IUserService _users;
        private IOrderService _orders;

        public OrderProcessingController(IProductService products, UserManager<Users> userManager, IUserService users, IOrderService orders)
        {
            _products = products;
            _userManager = userManager;
            _users = users;
            _orders = orders;

        }

        [Authorize(Policy = "IsManager")]
        public IActionResult Index()
        {
            try
            {
                var confirmedOrders = _orders.GetConfirmedOrders();

                var pendingOrders = _orders.GetPendingOrders();

                var confirmedOrdersModel = confirmedOrders.Select(o => new OrderModel
                {
                    Id = o.Id,
                    ItemId = o.ItemId,
                    StatusId = o.StatusId,
                    UserId = o.UserId,
                    DateAdded = o.DateAdded,
                    Item = o.Items,
                    Manufacturer = _products.GetProductName(o.Items.Id),
                    Model = _products.GetModelName(o.ItemId),
                    NumberOfItems = o.NumberOfItems,
                    BillingInfo = _orders.GetBillingInfoById(o.BillingInfoId),
                    PaymentMethod = _orders.GetPaymentMethod(o.PaymentMethodId),
                    Status = _orders.GetStatus(o.StatusId),


                });

                var pendingOrdersModel = pendingOrders.Select(o => new OrderModel
                {
                    Id = o.Id,
                    ItemId = o.ItemId,
                    StatusId = o.StatusId,
                    UserId = o.UserId,
                    DateAdded = o.DateAdded,
                    Item = o.Items,
                    Manufacturer = _products.GetProductName(o.Items.Id),
                    Model = _products.GetModelName(o.ItemId),
                    NumberOfItems = o.NumberOfItems,
                    BillingInfo = _orders.GetBillingInfoById(o.BillingInfoId),
                    PaymentMethod = _orders.GetPaymentMethod(o.PaymentMethodId),
                    Status = _orders.GetStatus(o.StatusId)

                });


                var ordersList = new OrderListModel
                {
                    ConfirmedOrders = confirmedOrdersModel,
                    PendingOrders = pendingOrdersModel
                };


                return View(ordersList);
            }
            catch { return RedirectToAction("Error", "Home", new { errorDetail = "Something Went Wrong!" }); }
        }

        [Authorize(Policy = "IsManager")]
        public IActionResult AddInvoice([FromBody]OrderListModel model)
        {
            try
            {
                var orders = _orders.GetOrdersByBillingInfo(model.BillingInfoId);
                if (orders.Count() > 0)
                {
                    List<Items> items = new List<Items>();
                    List<int> numberOfItems = new List<int>();

                    foreach(var order in orders)
                    {
                        var item = new Items { Id = order.ItemId, Availability = order.NumberOfItems };
                        items.Add(item);
                    }

                    var first = orders.First();
                    var blInfo = _orders.GetBillingInfoById(first.BillingInfoId);
                    var user = _users.GetUserById(first.UserId);

                    var change = _orders.ChangeOrderStatusesToPend(blInfo.Id);

                    var decreaseItemNumber = _products.DecreaseNumberOfItems(items);
                    /*
                    var ordersToAdd = orders.Select(o => new OrderModel
                    {

                        Id = o.Id,
                        Item = _products.GetById(o.ItemId),
                        Manufacturer = _products.GetProductName(o.Items.Id),
                        Model = _products.GetModelName(o.ItemId),
                        NumberOfItems = o.NumberOfItems

                    });*/

                    var ord = orders.ToList();
                    var addInvoiceToDB = new Invoice
                    {

                        InvoiceDate = DateTimeOffset.Now,
                        Orders = ord,
                        User = user.Result,
                        UserId = user.Id
                    };

                    var add = _orders.AddInvoice(addInvoiceToDB);
                    var lastInvoice = _orders.GetLastInvoice();

                    return Json(lastInvoice.Id);
                 }
             else { return Json("Invoice Addition Failed !"); }
           }


           catch { return Json("Something went wrong!"); }
        }

        [Authorize(Policy = "IsManager")]
        public IActionResult ChangeOrderStatus([FromBody]OrderListModel model)
        {
            try
            {
                var orders = _orders.GetOrdersByBillingInfo(model.BillingInfoId);
                if (orders.Count() > 0)
                {
                    var first = orders.First();
                    var blInfo = _orders.GetBillingInfoById(first.BillingInfoId);

                    var change = _orders.ChangeOrderStatuses(blInfo.Id, model.OrderStatus);

                    return Json("Orders Status Changed !");

                }
                else

                { return Json("Billing Info Id Not found"); }
            }

            catch { return Json("Something went wrong!"); }
           
        }

        [AllowAnonymous]
        public IActionResult Invoice(int id)
        {
            try
            {

                int invoiceId = id;
                var ordersById = _orders.GetOrderByInvoiceId(invoiceId);
                if (ordersById.Count() > 0)
                {
                    var orders = ordersById.Select(o => new OrderModel
                    {
                        Id = o.Id,
                        Item = _products.GetById(o.ItemId),
                        Manufacturer = _products.GetProductName(o.Items.Id),
                        Model = _products.GetModelName(o.ItemId),
                        NumberOfItems = o.NumberOfItems,
                        StatusId = o.StatusId
                    });


                    var invoice = new InvoiceModel
                    {
                        InvoiceId = invoiceId,
                        Orders = orders,
                        Customer = _users.GetUserById(ordersById.First().UserId).Result,
                        BillingInfo = _orders.GetBillingInfoById(ordersById.First().BillingInfoId),
                        InvoiceDate = _orders.GetInvoiceDate(invoiceId),
                        PaymentMethod = _orders.GetPaymentMethod(ordersById.First().PaymentMethodId),
                        InvoiceStatus = _orders.GetStatus(orders.First().StatusId)
                    };


                    return View(invoice);
                }

                else { return RedirectToAction("Error", "Home", new { errorDetail = "Invoice Not Found!" }); }
            }

            catch { return RedirectToAction("Error", "Home", new { errorDetail = "Something went wrong!" }); }

        } 

        [AllowAnonymous]
        public IActionResult InvoicePdf( InvoiceListModel listModel)
        {
            try
            {
                var invoice = _orders.GetInvoice(listModel.InvoiceId);
                var orders = _orders.GetOrderByInvoiceId(invoice.Id);
                if (orders.Count() > 0)
                {
                    var first = orders.First();
                    var blInfo = _orders.GetBillingInfoById(first.BillingInfoId);
                    var user = _users.GetUserById(first.UserId);

                    var ordersToAdd = orders.Select(o => new OrderModel
                    {

                        Id = o.Id,
                        Item = _products.GetById(o.ItemId),
                        Manufacturer = _products.GetProductName(o.Items.Id),
                        Model = _products.GetModelName(o.ItemId),
                        NumberOfItems = o.NumberOfItems,


                    });


                    var invoiceToAdd = new InvoiceModel
                    {
                        Orders = ordersToAdd,
                        Customer = user.Result,
                        BillingInfo = blInfo,
                        InvoiceDate = DateTimeOffset.Now,
                        PaymentMethod = _orders.GetPaymentMethod(first.PaymentMethodId)

                    };

                    return new ViewAsPdf("InvoicePdf", invoiceToAdd);
                }

                else { return RedirectToAction("Error", "Home", new { errorDetail = "Invoice Not Found!" }); }
            }

            catch { return RedirectToAction("Error", "Home", new { errorDetail = "Something went wrong!" }); }

        }
   
        [AllowAnonymous]
        public IActionResult InvoiceList(OrderListModel model)
        {
 
            var user = _users.GetUserById(model.UserId);

            var invoices = _orders.GetInvoices(user.Result.UserName);

            var invoicesToAdd = invoices.Select(i => new Invoice
            {   
                Id = i.Id,
                InvoiceDate = i.InvoiceDate,
                User = i.User,
                Orders = i.Orders
            });

            var invoice = new InvoiceListModel
            {
              Invoices = invoicesToAdd
            };

            return View(invoice);
        }

        public JsonResult GetUsers([FromBody]OrderListModel recieve)
        {

            var users = _users.GetUserNames(recieve.UserSrch);      
            var usr = users.Select(u => new Users
            {
                Id = u.Id,
                UserName = u.UserName

            });

            var send = new OrderListModel
            {
                Users = usr       
            };

            return Json(send);
        }


    }
}