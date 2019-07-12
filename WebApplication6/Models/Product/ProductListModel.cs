using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StoreData.Models;

namespace WebApplication6.Models.Product
{
    public class ProductListModel
    {

        public IEnumerable<ProductModel> Products { get; set; }
        public IEnumerable<ProductModel> ProductHistory { get; set; }
        public IEnumerable<ProductIndexModel> ProductIndex { get; set; }
        public IEnumerable<SliderImageUrls> SliderImages { get; set; }
        public ProductModel Detail { get; set; }
        public IEnumerable<ProductModel> RelatedProducts{get;set;}
        public List<int> ProductId { get; set; }
        public string Sort { get; set;}
        public string ItemGroup { get; set;}
        public string Name { get; set;}
        public string Man { get; set; }
        public string NumberOfItems { get; set; }
        public IEnumerable<int> NumberOfPages { get; set; }
        public string PageNumber { get; set; }
        public IEnumerable<string> ManufacturerList { get; set;}
        public IEnumerable<string> PriceRangeList { get; set; }
        public double MaxPrice { get; set; }
        public double MinPrice { get; set; }
        public IEnumerable<string> SubCategories { get; set; }
        public string ItemCategory { get; set; }
        public string ErrorMesage { get; set; }
        public string HeaderImageUrl { get; set; }
        public int BillingInfoId { get; set; }
        public int InvoiceId { get; set; }
    }
}
