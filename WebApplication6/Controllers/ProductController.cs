using DNTBreadCrumb.Core;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreData;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication6.Models.Product;


namespace WebApplication6.Controllers
{
     
    public class ProductController : Controller
    {

        private IProductService _products;
        private UserManager<Users> _userManager;

        public ProductController(IProductService products, UserManager<Users> userManager)
        {
            _products = products;
            _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index(string _name, string _itemGroup)
        {
            try
            {
            var product = _products.GetByType(_name);

            var model = product.Select(a => new ProductIndexModel
            {
                ItemId = a.Id,
                ManufacturerName = _products.GetManufacturerNameString(a.Id),
                Price = a.Price,
                ItemSubType = _products.GetSubCategory(a.Id)
            });

            var uniqueManList = new HashSet<string>();
            var uniquePrices = new HashSet<double>();
            var uniqueSubTypes = new HashSet<string>();

            foreach (var mod in model)
            {
                uniqueManList.Add(mod.ManufacturerName);
                uniquePrices.Add(mod.Price);
                uniqueSubTypes.Add(mod.ItemSubType);
            }

            var productmodel = new ProductListModel
            {
                ProductIndex = model,
                ItemGroup = _itemGroup,
                Name = _name,
                ManufacturerList = uniqueManList,
                NumberOfItems = model.Count().ToString(),
                MinPrice = uniquePrices.Min(),
                MaxPrice = uniquePrices.Max(),
                SubCategories = uniqueSubTypes,
                HeaderImageUrl = _products.GetItemTypeHeaderImg(_name)

            };

            var group = _itemGroup.Split(' '); string title = "";
            for (int i = 0; i < group.Count(); i++) {
                title += group[i].First().ToString().ToUpper() + group[i].Substring(1).ToLower() + " ";
            };

            this.AddBreadCrumb(new BreadCrumb
            {       
                Title = title,
                Url = string.Format("?_name=" + _name + "&_itemGroup=" + _itemGroup + ""),
                Order = 1
            });

            return View(productmodel);
             }

               catch
               {
                       return RedirectToAction("Error", "Home", new { errorDetail = "Invalid Request" });
               }
        }

  
        [AllowAnonymous]
        public IActionResult Detail(int id)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userManager.GetUserId(User);

                    if (user != null)
                    {
                        var userId = Int32.Parse(user);

                        var add = _products.AddItemHistory(id, userId);
                    }
                    var product = _products.GetById(id);

                    var model = new ProductModel
                    {
                        ItemId = product.Id,
                        ManufacturerAndModel = _products.GetProductName(product.Id),
                        Model = _products.GetModelName(product.Id),
                        ImageUrl = _products.GetImageUrls(product.Id),
                        Reviews = _products.GetReviews(product.Id),
                        Price = product.Price,
                        Discount = product.Discount,
                        Availibility = product.Availability,
                        Type = _products.GetType(product.Model.Id),
                        Specs = _products.GetSpecs(product.Model.Id),
                        AvrageUserRating = _products.GetAvrageRating(product.Id),
                        NumberOfReviews = _products.GetReviews(product.Id).Count()
                    };

                    var itemGroup = _products.GetItemGroup(product.Model.Id);
                    var group = itemGroup.Split(' '); string title = "";
                    for (int i = 0; i < group.Count(); i++)
                    {
                        title += group[i].First().ToString().ToUpper() + group[i].Substring(1).ToLower() + " ";
                    };

                    this.AddBreadCrumb(new BreadCrumb
                    {
                         Title = title,
                         Url = Url.Action ("Index","Product", values: new { _name = model.Type.Name, _itemGroup= itemGroup }), 
                         Order = 0
                    });
                    
                       this.AddBreadCrumb(new BreadCrumb
                       {
                           Title = model.ManufacturerAndModel.Name + " " + model.Model.Name ,
                           Url = string.Format(id.ToString()),
                           Order = 1
                       }); 

                    return View(model);
                }

                catch { return RedirectToAction("Error", "Home", new { errorDetail = "Item Not Found!" }); }
            }

            else { return RedirectToAction("Error", "Home", new { errorDetail = "Something Went Wrong!" }); }

        }

        [AllowAnonymous]
        public IActionResult GetHistory()
        {
            
            var userId = Int32.Parse(_userManager.GetUserId(User));
            var histId = _products.GetHistoryByUserId(userId);
            var hist = _products.GetByHistoryId(histId);
          

            var historyModel = hist.Select(a => new ProductModel
            {
                ItemId = a.Id,
                ManufacturerAndModel = _products.GetProductName(a.Id),
                Model = _products.GetModelName(a.Id),
                FirstImage = _products.GetFirstImage(a.Id),
                Price = a.Price,
                AvrageUserRating = _products.GetAvrageRating(a.Id),
                Discount = a.Discount
            });

            var send = new ProductListModel
            {
                ProductHistory = historyModel
            };

            return PartialView("History", send);
        }

        [AllowAnonymous]
        public IActionResult GetRelatedItems(int Id)
        {
            
            var itemSubType = _products.GetSubCategory(Id);

            var related = _products.GetRelatedItems(itemSubType);

            var relatedModel = related.Select(a => new ProductModel
            {
                ItemId = a.Id,
                ManufacturerAndModel = _products.GetProductName(a.Id),
                Model = _products.GetModelName(a.Id),
                FirstImage = _products.GetFirstImage(a.Id),
                Price = a.Price,
                AvrageUserRating = _products.GetAvrageRating(a.Id),
                Discount = a.Discount
            });

            var send = new ProductListModel
            {
                RelatedProducts = relatedModel
            };

            return PartialView("RelatedProducts", send);
        }

        public JsonResult AddReview ([FromBody]ProductModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
 
                    var user = _userManager.GetUserAsync(User);

                    var item = _products.GetById(model.ItemId);


                    var reviewToAdd = new Reviews
                    {
                        Item = item,
                        Rating = model.Review.Rating,
                        Text = model.Review.Text,
                        Users = user.Result,
                    };

                    var add = _products.AddReview(reviewToAdd);

                    return Json("Review Added!");
               }

                    catch { return Json( "Review addition failed!"); }
            }

            else { return Json("Something went wrong!"); }

            /*return Json(model);*/
        }

        public JsonResult RefreshItems([FromBody] ProductListModel modl)
        {          
                var product = _products.GetItemsRefreshFilter(/*modl.ProductId,*/ modl.Name,modl.ManufacturerList,modl.SubCategories);

                var model = product.Select(a => new ProductIndexModel
                {   ItemId = a.Id,
                    ManufacturerName = _products.GetManufacturerNameString(a.Id),
                    ModelName = _products.GetModelNameString(a.Id),
                    FirstImage = _products.GetFirstImage(a.Id),
                    Price = a.Price,
                    Discount = a.Discount,
                    AvrageUserRating = _products.GetAvrageRating(a.Id),
                    ItemSubType = _products.GetSubCategory(a.Id)
                });
                
                var priceFilter = FilterByPrice(model,modl.MinPrice,modl.MaxPrice);
                var sort = Sort(priceFilter.ProductIndex, modl.Sort);

                var send = new ProductListModel { ProductIndex = sort.ProductIndex,};
                return Json(send);
        }
 /*     
        public JsonResult SortAndFilter([FromBody] ProductListModel modl)
        {
            if (ModelState.IsValid)
            {
                var product = _products.GetItemsByIds(modl.ProductId);

                var model = product.Select(a => new ProductIndexModel
                {
                    ItemId = a.Id,
                    ManufacturerName = _products.GetManufacturerNameString(a.Id),
                    ModelName = _products.GetModelNameString(a.Id),
                    FirstImage = _products.GetFirstImage(a.Id),
                    Price = a.Price,
                    AvrageUserRating = _products.GetAvrageRating(a.Id)
                });

                var send = Sort(model, modl.Sort);

                return Json(send);
            } 

            else { return Json(modl.ErrorMesage = "Something Went Wrong!"); }
        }*/

        #region HelperMethods

        public ProductListModel Sort(IEnumerable<ProductIndexModel> PLModel,string Sort)
        {

            if (Sort == "Relevance")
            {
                var productmodel = new ProductListModel
                {
                    ProductIndex = PLModel.OrderByDescending(m => m.ItemId),

                };

                return productmodel;
            }

            if (Sort == "BrandAZ")
            {
                var productmodel = new ProductListModel
                {
                    ProductIndex = PLModel.OrderBy(m => m.ManufacturerName),

                };

                return productmodel;
            }

            if (Sort == "BrandZA")
            {
                var productmodel = new ProductListModel
                {
                    ProductIndex = PLModel.OrderByDescending(m => m.ManufacturerName),

                };

                return productmodel;
            }

            if (Sort == "Price(Hi to Low)")
            {
                var productmodel = new ProductListModel
                {
                    ProductIndex = PLModel.OrderByDescending(m => m.Price),

                };

                return productmodel;
            }

            if (Sort == "Price(Low to Hi)")
            {
                var productmodel = new ProductListModel
                {
                    ProductIndex = PLModel.OrderBy(m => m.Price),

                };

                return productmodel;
            }

            else
            {
                var productmodel = new ProductListModel
                {
                    ProductIndex = PLModel,

                };

                return productmodel;
            }
           
        }
  
        public ProductListModel FilterByPrice(IEnumerable<ProductIndexModel> PLModel, double PriceMin, double PriceMax)
        {
            

            if (PriceMin != 0  || PriceMax != 0)
            {
                List<ProductIndexModel> mdl = new List<ProductIndexModel>();

                foreach (var plm in PLModel)
                {

                        if (plm.Price > PriceMin && plm.Price < PriceMax)
                        {
                            mdl.Add(plm);
                        }
                     
                }

                var productmodel = new ProductListModel
                {
                    ProductIndex = mdl,
                };

                return productmodel;

            }

            else
            {
                var productmodel = new ProductListModel
                {
                    ProductIndex = PLModel,

                };
                 
                return productmodel;
            }

        }

        #endregion
    }
}