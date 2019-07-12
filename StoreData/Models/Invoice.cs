using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StoreData.Models
{
    public class Invoice
    {
        public int Id { get; set; }
   
        public virtual ICollection<Order> Orders { get; set; }

        [Required, ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual Users User { get; set; }

        public DateTimeOffset InvoiceDate { get; set; }

    }
} 
