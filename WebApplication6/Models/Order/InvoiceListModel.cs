using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models.Order
{
    public class InvoiceListModel
    {

        public IEnumerable<Invoice> Invoices { get; set; }
        public int InvoiceId { get; set; }
        public int Id { get; set; }

    }
}
