using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StoreData.Models
{
    public class Order
    {

        [Key, Required]
        public int Id { get; set; }

        public DateTimeOffset? DateAdded { get; set; }

        [Required, ForeignKey(nameof(Users))]
        public int UserId { get; set; }

        public virtual Users Users { get; set; }

        [Required, ForeignKey(nameof(Items))]
        public int ItemId { get; set; }

        public virtual Items Items { get; set; }

        [Required, ForeignKey(nameof(Statuses))]
        public int StatusId { get; set; }

        public virtual Status Statuses { get; set; }

        public int NumberOfItems { get; set; }


        [ForeignKey(nameof(BillingInfo))]
        public int BillingInfoId { get; set; }
        public virtual BillingInfo BillingInfo { get; set; }

        [ForeignKey(nameof(PaymentMethod))]
        public int PaymentMethodId { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }

        public int InvoiceId { get; set; }

    }
}
