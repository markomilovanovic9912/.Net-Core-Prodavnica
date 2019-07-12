using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models.Order
{
    public class InvoiceModel
    {
        public int InvoiceId {get;set;}
        public IEnumerable<OrderModel> Orders { get; set; }
        public BillingInfo BillingInfo { get; set; }
        public string PaymentMethod { get; set; }
        public Users Customer { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public string InvoiceStatus { get; set; }

    }
}
