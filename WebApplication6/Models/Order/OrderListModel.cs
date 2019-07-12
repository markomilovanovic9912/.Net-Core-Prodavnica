using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models.Order
{
    public class OrderListModel
    {
        public IEnumerable<OrderModel> Orders { get; set; }
        public IEnumerable<OrderModel> PendingOrders { get; set; }
        public IEnumerable<OrderModel> ConfirmedOrders { get; set; }
        public InvoiceModel ReturnInvoice { get; set; }
        public double SumTotal { get; set; }
        public int BillingInfoId { get; set; }
        public int UserId { get; set; }
        public int OrderStatus { get; set; }
        public string UserName { get; set; }
        public string UserSrch { get; set; }
        public IEnumerable<Users> Users { get; set; }
        public IEnumerable<Invoice> Invoices { get; set; }
        public int InvoiceId { get; set; }
    }
}
