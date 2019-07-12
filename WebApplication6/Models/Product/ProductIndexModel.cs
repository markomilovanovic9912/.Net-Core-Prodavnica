using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models.Product
{
    public class ProductIndexModel
    {

        public int ItemId { get; set; }
        public string ManufacturerName { get; set;}
        public string ModelName { get; set;}
        public string FirstImage { get; set;}
        public double Price { get; set;}
        public int Discount { get; set;}
        public int Availibility { get; set;}
        public double AvrageUserRating { get; set;}
        public string ItemSubType { get; set; }
        public string ErrorMessage { get; set;}

    }
}
