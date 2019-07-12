using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models.Order
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTimeOffset? DateAdded { get; set; }
        public int UserId { get; set; }
        public int ItemId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public int NumberOfItems { get; set; }
        public BillingInfo BillingInfo { get; set; }
        public int BillingInfoId { get; set; }
        public string PaymentMethod { get; set; }
        public bool PmIsEnabled { get; set; }
        public bool PmIsDisabled { get; set; }
        public Model Model { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public Items Item { get; set; }
        public string FirstImage { get; set; }
        public double SumTotal { get; set; }
        public string FirstName{get;set;}
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string CountryOrState { get; set; }
        public string RedirectUrl { get; set; }


    }
}
