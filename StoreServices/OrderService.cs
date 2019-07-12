using Microsoft.EntityFrameworkCore;
using StoreData;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreServices
{
    public class OrderService : IOrderService
    {

        private StoreContext _context;

        public OrderService(StoreContext context)
        {
            _context = context;
        }


        public Task<Order> AddOrder(Order order)
        {
            var orderToAdd = new Order
            {
                ItemId = order.ItemId,
                UserId = order.UserId,
                DateAdded = DateTimeOffset.Now,
                NumberOfItems = order.NumberOfItems,
                StatusId = order.StatusId,
                BillingInfoId = order.BillingInfoId,
                PaymentMethodId = 1,
                InvoiceId = order.InvoiceId
            };

            _context.Orders.Add(orderToAdd);
            _context.SaveChanges();

            return null;
        }

        public int GetNumberOfItems(int userId)
        {
           
            var orders = GetOrdersByUser(userId);

            return orders.Count();
        }

        public IEnumerable<Order> GetOrdersByUser(int userId)
        {
            var order = _context.Orders.Include(o => o.Items).Where(o => o.Users.Id == userId).Where(o => o.StatusId == 1 || o.StatusId == 2);

            return order;
        }

        public Task<Order> RemoveOrder(int orderId)
        {
            var orderToRemove = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            _context.Orders.Remove(orderToRemove);
            _context.SaveChanges();
            return null;
        }

        public Task UpdateOrders(string PaymentMethod, int BillingInfoId, int userId)
        {
            var orders = GetOrdersByUser(userId);  

            var PaymentMethodId = _context.PaymentMethod.FirstOrDefault(pm => pm.Method == PaymentMethod);


            foreach (var ord in orders)
            {
                 ord.BillingInfoId = BillingInfoId;
                ord.PaymentMethodId = PaymentMethodId.Id;

                _context.Orders.Update(ord);
            }

   
            _context.SaveChanges();

            return null;
        }

        public Task<BillingInfo> AddBillingInfo (BillingInfo info)
        {


            var billToAdd = new BillingInfo
            {
                Adress = info.Adress,
                City = info.City,
                CountryOrState = info.CountryOrState,
                Email = info.Email,
                PhoneNumber = info.PhoneNumber


            };

            _context.BillingInfo.Add(billToAdd);
            _context.SaveChanges();

            var billToReturn = _context.BillingInfo.LastOrDefault(bi => bi.Adress == info.Adress);


            return Task.FromResult(billToReturn);
        }

        public Task ChangeOrderStatus(int userId, int status)
        {
            var orders = _context.Orders.Where(o => o.UserId == userId && o.StatusId < 3);

            foreach (var order in orders)
            {

                order.StatusId = status;
                _context.Orders.Update(order);
            }

            _context.SaveChanges();

            return null;

        }

        public string GetPaymentMethod(int paymentId)
        {
            var option = _context.PaymentMethod.FirstOrDefault(po => po.Id == paymentId);

            return option.Method;
        }

        public IEnumerable<Order> GetConfirmedOrders()
        {
            var orders = _context.Orders.Include(o => o.Items).Where(o => o.StatusId == 3).OrderBy(o => o.BillingInfoId);

            return orders;
        }

        public IEnumerable<Order> GetPendingOrders()
        {
            var orders = _context.Orders.Include(o => o.Items).Where(o => o.StatusId == 4).OrderBy(o => o.BillingInfoId);

            return orders;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            var orders = _context.Orders.Include(o => o.Items);

            return orders;
        }

        public IEnumerable<Order> GetOrdersByBillingInfo(int billInfoId)
        {
            var orders = _context.Orders.Include(o => o.Items).Where(o => o.BillingInfoId == billInfoId);

            return orders;
        }


        public BillingInfo GetBillingInfoById(int infoId)
        {
            var info = _context.BillingInfo.FirstOrDefault(bi => bi.Id == infoId);

            return info;
        }

        public string GetStatus(int statusId)
        {
            var status = _context.Status.FirstOrDefault(s => s.Id == statusId);

            var text = status.StatusText;

            return text;
        }

        public IEnumerable<Invoice> GetInvoices(string userName)
          {
              var invoices = _context.Invoice.Where(o => o.User.UserName == userName);

            return invoices;

          }

        public Invoice GetInvoice(int invoiceId)
        {
            var invoice = _context.Invoice.FirstOrDefault(i => i.Id == invoiceId);

            return invoice;
        }

        public Invoice GetLastInvoice()
        {
            var invoice = _context.Invoice.Last();

            return invoice;
        }

        public Task<Invoice> AddInvoice(Invoice invoice)
        {
            var invoiceToAdd = new Invoice
            {
             InvoiceDate = invoice.InvoiceDate,
             Orders = invoice.Orders,
             UserId = invoice.UserId,
             User = invoice.User
            };

            _context.Invoice.Add(invoiceToAdd);
            _context.SaveChanges();

            return null;
        }

        public Task ChangeOrderStatusesToPend(int billingInfoId)
        {
            var orders = _context.Orders.Where(o => o.BillingInfoId == billingInfoId);

            foreach (var ord in orders)
            {
                ord.StatusId = 4;
                _context.Orders.Update(ord);

            };
           
            _context.SaveChanges();

            return null;
        }


        public Task ChangeOrderStatuses(int billingInfoId,int statusId)
        {
            var orders = _context.Orders.Where(o => o.BillingInfoId == billingInfoId);

            foreach (var ord in orders)
            {
                ord.StatusId = statusId;
                _context.Orders.Update(ord);

            };

            _context.SaveChanges();

            return null;
        }

        public IEnumerable<Order> GetOrderByInvoiceId(int invoiceId)
        {
            var orders = _context.Orders.Where(o => o.InvoiceId == invoiceId);

            return orders;
        }

        public DateTimeOffset GetInvoiceDate(int invoiceId)
        {
            var date = _context.Invoice.FirstOrDefault(i => i.Id == invoiceId);

            return date.InvoiceDate;
        }

        public PaymentMethod AddPaymentMethod(PaymentMethod paymentMehod)
        {
            var paymentMethodToAdd = new PaymentMethod
            {
                Id = paymentMehod.Id,
                Method = paymentMehod.Method
            };

            _context.Add(paymentMehod);
            _context.SaveChanges();

            return null;
        }

        public IEnumerable<PaymentMethod> GetAllPaymentMethods()
        {
            throw new NotImplementedException();
        }

        public Status AddStatus(Status status)
        {
            var statusToAdd = new Status
            {
                Id = status.Id,
              StatusText = status.StatusText
            };

            _context.Add(statusToAdd);
            _context.SaveChanges();

            return null;
        }
    }
} 
