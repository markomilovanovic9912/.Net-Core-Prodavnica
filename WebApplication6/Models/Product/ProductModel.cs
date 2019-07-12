using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models.Product
{
    public class ProductModel
    {
         

        public int ItemId { get; set; }
        public Manufacturer ManufacturerAndModel { get; set; }
        public ItemType Type { get; set; }
        public ItemTypeSub SubType { get; set; }
        public Specs Specs { get; set; }
        public Model Model { get; set; }
        public IEnumerable<ImageUrls> ImageUrl { get; set; }
        public IEnumerable<Reviews> Reviews { get; set; }
        public Reviews Review { get; set; }
        public int NumberOfReviews { get; set; }
        public string FirstImage { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public int Availibility { get; set; }
        public double AvrageUserRating { get; set; }
        public int UserId { get; set; }
        public int BillingInfoId { get; set; }
        public int InvoiceId { get; set; }

        public int StatusId { get; set; }

        public int NumberOfItems { get; set; }

        public string Message { get; set; }

    }


}

