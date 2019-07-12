using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DNTBreadCrumb.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreData;
using StoreData.Models;
using WebApplication6.Models;
using WebApplication6.Models.Admin;
using WebApplication6.Models.Product;

namespace WebApplication6.Controllers
{  
    [BreadCrumb(Title = "Home", UseDefaultRouteUrl = true, Order = 0)]
    public class HomeController : Controller
    {

        private IProductService _products;
        private UserManager<Users> _userManager;

        public HomeController(IProductService products, UserManager<Users> userManager)
        {
            _products = products;
            _userManager = userManager;

        }
 
        [BreadCrumb(Title = "Main index", Order = 1)] 
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Index()
        {
            var sliders = _products.GetAllSliderImages();

            var userId = _userManager.GetUserId(User);

            if (userId != null) { 

            var id = Int32.Parse(userId);

            var histid = _products.GetHistoryByUserId(id);

            var hist = _products.GetByHistoryId(histid);

            var model = hist.Select(a => new ProductModel
            {
                ItemId = a.Id,
                ManufacturerAndModel = _products.GetProductName(a.Id),
                Model = _products.GetModelName(a.Id),
                FirstImage = _products.GetFirstImage(a.Id),
                Price = a.Price,
                AvrageUserRating = _products.GetAvrageRating(a.Id)
            });


            var productmodel = new ProductListModel
            {
                SliderImages = sliders,
                ProductHistory = model
            };



            return View(productmodel);
            }

            var productmodelNoUser = new ProductListModel
            {
                SliderImages = sliders,

            };


            return View(productmodelNoUser);
        }

        public JsonResult GetProductNames([FromBody] string ProdSearch)
        {
            var products = _products.SearchProducts(ProdSearch);

            var prod = products.Select(p => new ProductIndexModel
            {
                ItemId = p.Id,
                ManufacturerName = _products.GetManufacturerNameString(p.Id),
                ModelName = _products.GetModelNameString(p.Id),
            });

            var send = new ProductListModel
            {
                ProductIndex = prod
            };

            return Json(send);
        }

        public IActionResult Department(string department)
        {
            var userId = _userManager.GetUserId(User);

            if (userId != null)
            {

                var id = Int32.Parse(userId);

                var histid = _products.GetHistoryByUserId(id);

                var hist = _products.GetByHistoryId(histid);

                var model = hist.Select(a => new ProductModel
                {
                    ItemId = a.Id,
                    ManufacturerAndModel = _products.GetProductName(a.Id),
                    Model = _products.GetModelName(a.Id),
                    FirstImage = _products.GetFirstImage(a.Id),
                    Price = a.Price,
                    AvrageUserRating = _products.GetAvrageRating(a.Id)
                });


                var productmodel = new ProductListModel
                {
                    ProductHistory = model
                };



                return View(department,productmodel);
            }

            return View(department);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error(string errorDetail)
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,errorMessage = errorDetail });
        }
    }
}
