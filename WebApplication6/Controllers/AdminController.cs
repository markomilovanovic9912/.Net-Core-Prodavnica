using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StoreData;
using StoreData.Models;
using WebApplication6.Models.Admin;
using WebApplication6.Models.Order;

namespace WebApplication6.Controllers
{
    public class AdminController : Controller
    {
        private IProductService _products;
        private UserManager<Users> _userManager;
        private IUserService _users;
        private IHostingEnvironment _hostingEnv;


        public AdminController(IProductService products, UserManager<Users> userManager, IUserService users, IHostingEnvironment hostingEnv)
        {
            _products = products;
            _userManager = userManager;
            _users = users;
            _hostingEnv = hostingEnv; 

        }


        [Authorize(Policy = "IsAdmin")]
        [HttpGet]
        public IActionResult Index()
        {

            var model = new AdminModel
            {
                Manufacturers = _products.GetAllManufacturers(),
                ItemTypes = _products.GetAllItemTypes(),
                ItemDepartments = _products.GetAllItemDepartments(),
                Models = _products.GetAllModels(),
                Users = _users.GetAllUsers(),
            };


            return View(model);
        }


        #region Product Management


        public JsonResult GetSubTypes([FromBody] AdminModel item)
        {
            var subTypes = _products.GetItemSubTypes(item.ItemTypeName);

            var send = new AdminModel
            {
                ItemSubTypes = subTypes
            };

            return Json(send);
        }


        public JsonResult GetModels([FromBody] AdminModel item)
        {
            var models = _products.GetModelsByItemType(item.ItemTypeName);

            var send = new AdminModel
            {
                Models = models
            };

            return Json(send);
        }


        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public JsonResult AddItem([FromBody]AdminModel addItem)
        {


            /* if(ModelState.IsValid)
               {*/
            try
            {
                    var list = new List<string>();
                    list = addItem.ImageUrls.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                    List<ImageUrls> imgUrl = new List<ImageUrls>();

                    foreach (var var in list)
                    {
                        imgUrl.Add(new ImageUrls
                        {
                            Url = var

                        });

                    }

                    var item = new Items
                    {
                        Availability = addItem.Availibility,
                        Price = addItem.Price,
                        Discount = addItem.Discount,
                        ManuModel = addItem.Manufacturer,
                        Model = addItem.Model,
                        ImageUrls = imgUrl,
                        Color = addItem.Color,
                        ItemTypeSub = new ItemTypeSub { Id = addItem.ItemSubType.Id }
                    };

                    var model = new Model
                    {
                        ItemDepartment = addItem.ItemDepartment,
                        Name = addItem.ModelName,
                        SpecsId = addItem.Specs,
                        TypeId = addItem.ItemType
                    };

                    var spec = new Specs
                    {
                        Description = addItem.Specs.Description,
                        Specification = addItem.Specs.Specification
                    };

                    var addItems = _products.AddItem(item, spec, model);

                    return Json(addItem.Message = "Item added!"); ;
               }
                catch { return Json(addItem.Message = "Adding item failed!"); }
            /*}

            else { return Json(addItem.Message = "Something went wrong!"); }*/

           /* return Json(addItem);*/
           
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public JsonResult RemoveItem([FromBody]AdminModel removeItem)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var modelName = removeItem.ModelName;

                    var removeItems = _products.RemoveItem(modelName);

                    return Json(removeItem.Message = "Item Was Sucessfully Removed");
                }

                catch { return Json(removeItem.Message = "Item Removal Error!"); }
            }

            else { return Json(removeItem.Message = "Something went wrong!"); }

        }



        public static AdminModel modelUpdt = new AdminModel();

        [Authorize(Policy = "IsAdmin")]
        public IActionResult UpdateItem()
        {
           /*var model = new AdminModel
            {
                Manufacturers = _products.GetAllManufacturers(),
                ItemTypes = _products.GetAllItemTypes(),
                ItemDepartments = _products.GetAllItemDepartments(),
                Models = _products.GetAllModels(),
                Users = _users.GetAllUsers(),
            };*/

            return View();
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
         public JsonResult UpdateItems([FromBody] AdminModel updateItem)
         {

            /*  if (ModelState.IsValid)
              {*/
            try
            {
                    var item = new Items
                    {
                        Id = updateItem.Id,
                        Availability = updateItem.Availibility,
                        Price = updateItem.Price,
                        Discount = updateItem.Discount,
                        Color = updateItem.Color
                    };

                    var model = new Model
                    {

                        Name = updateItem.ModelName,
                    };

                    var spec = new Specs
                    {
                        Id = updateItem.SpecId,
                        Description = updateItem.SpecDesc,
                        Specification = updateItem.SpecSpec
                    };

                    var updateItems = _products.UpdateItem(item, spec, model);

                    return Json("Item Updated");
                }

                catch { return Json("Update failed"); }
 
         
            /*else { return Json("Something went wrong"); }*/
         

         }

        [HttpPost]
        public JsonResult GetSpecs([FromBody] AdminModel item)
        {
             var itemUpdt = _products.GetItemByModelName(item.ModelName);

             var specsUpdt = _products.GetSpecsByName(item.ModelName);

             var specGet = new Specs
             {
                 Id= specsUpdt.Id,
                 Specification = specsUpdt.Specification,
                 Description = specsUpdt.Description
                
             };

            modelUpdt.Id = itemUpdt.Id;
            modelUpdt.Availibility = itemUpdt.Availability;
            modelUpdt.Color = itemUpdt.Color;
            modelUpdt.Discount = itemUpdt.Discount;
            modelUpdt.Price = itemUpdt.Price;
            modelUpdt.Specs = specGet;
            modelUpdt.ModelName = item.ModelName;

            return Json(modelUpdt);
        }


        [Authorize(Policy = "IsAdmin")]
        public JsonResult AddManufacturer ([FromBody]AdminModel addManufacturer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var add = _products.AddManufacturer(addManufacturer.Manufacturer);

                    return Json(addManufacturer.Message = "Manufacturer added!");
                }

                catch { return Json(addManufacturer.Message = "Adding Manufacturer Failed!"); }
            }

            else { return Json(addManufacturer.Message = "Something went wrong!"); }
        }

        [Authorize(Policy = "IsAdmin")]
        public JsonResult RemoveManufacturer([FromBody]AdminModel removeManufacturer)
        {
            if (ModelState.IsValid)
             {   try
                 {
                     var remove = _products.RemoveManufacturer(removeManufacturer.ManufacturerId);


                    return Json(removeManufacturer.Message = "Manufacturer Deleted");
                 }

                catch
                {
                    return Json(removeManufacturer.Message = "Manufacturer Deletion Failed");
                }
            }

            else
            {
                return Json(removeManufacturer.Message = "Something went wrong");
            }
        }

        [Authorize(Policy = "IsAdmin")]
        public JsonResult AddItemType([FromBody]AdminModel addItemType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var add = _products.AddItemType(addItemType.ItemType);

                    return Json(addItemType.Message = "Item Type added!");
                }

                catch { return Json(addItemType.Message = "Adding Item Type Failed!"); }
            }

            else { return Json(addItemType.Message = "Something went wrong!"); }
        }

        [Authorize(Policy = "IsAdmin")]
        public JsonResult RemoveItemType([FromBody]AdminModel removeItemType)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var remove = _products.RemoveItemType(removeItemType.ItemTypeId);

                    return Json(removeItemType.Message = "Item Type Deleted");
                }

                catch
                {
                    return Json(removeItemType.Message = "Item Type Deletion Failed");
                }
            }

            else
            {
                return Json(removeItemType.Message = "Something went wrong");
            }

        }

        [Authorize(Policy = "IsAdmin")]
        public JsonResult AddItemSubType([FromBody]AdminModel addItemSubType)
        {
            /*if (ModelState.IsValid)
             {*/
                 try
                 {
                     var addModel = new ItemTypeSub
                       {
                         ItemType = addItemSubType.ItemSubType.ItemType,
                         SubTypeName = addItemSubType.ItemSubType.SubTypeName
                       };

                     var add = _products.AddItemSubType(addModel);

                     return Json(addItemSubType.Message = "Item Sub Type added!");
                 }

                 catch { return Json(addItemSubType.Message = "Adding Item Sub Type Failed!"); }
            /* }
        
            else { return Json(addItemSubType.Message = "Something went wrong!"); }*/

           /* return Json(addItemSubType);*/
        }

        [Authorize(Policy = "IsAdmin")]
        public JsonResult RemoveItemSubType([FromBody]AdminModel removeItemType)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var remove = _products.RemoveItemSubType(removeItemType.ItemSubTypeId);

                    return Json(removeItemType.Message = "Item Sub Type Deleted");
                }

                catch
                {
                    return Json(removeItemType.Message = "Item Sub Type Deletion Failed");
                }
            }

            else
            {
                return Json(removeItemType.Message = "Something went wrong");
            }

       
        }


        public static AdminModel GetReview = new AdminModel();

        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public JsonResult RemoveReview ([FromBody]AdminModel removeReview)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var removeRev = _products.RemoveReview(removeReview.ReviewId);


                    return Json(removeReview.Message = "Review Deleted");
                }

                catch
                {
                    return Json(removeReview.Message = "Review Deletion Failed");
                }
            }

            else
            {
                return Json(removeReview.Message = "Something went wrong");
            }
        }

        [HttpPost]
        public JsonResult GetReviews([FromBody] AdminModel item)
        {
            var reviews = _products.GetReveiwsByModelName(item.ModelName);
             
            GetReview.Reviews = reviews;

            return Json(GetReview.Reviews);
        }

        #endregion

        #region User Management

        public static AdminModel getUser = new AdminModel();

        [Authorize(Policy = "IsAdmin")]
        public IActionResult UpdateRemoveUser()
        {

            return View(getUser);
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public JsonResult UpdateRemoveUser([FromBody]AdminModel user)
        {
            /*return Json(user);*/

            if (ModelState.IsValid) {

                try{ 
                    var userGet = _users.GetUserByUserName(user.UserName);

                    var removeUser = _users.RemoveUser(userGet.Id);

                    return Json("User Removed!");
                 }

                catch { return Json("User Removal Failed!"); }
        }

            else { return  Json("Something went wrong!"); } 
        }



        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public JsonResult AddUserClaim([FromBody]AdminModel addUserClaim)
        {

            if (ModelState.IsValid)
            {

                try
                {
                    var addClaim = _users.AddUserClaim(addUserClaim.UserClaim);

                    return Json("User Claim Added !");
                }

                catch { return Json("User Claim Addition Failed!"); }
            }

            else { return Json("Something went wrong!"); }
        }


        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public JsonResult RemoveUserClaim([FromBody]AdminModel removeUserClaim)
        {

          /*  if (ModelState.IsValid)
            {*/

                try
                {

                    var addClaim = _users.RemoveUserClaim(removeUserClaim.UserClaim);

                    return Json("User Claim Removed!");
                }

                catch { return Json("User Claim Removal Failed!"); }

            /*}

            else { return Json("Something went wrong!"); }
            */
        }

        public JsonResult GetUserNames([FromBody]AdminModel recieve)
        {
            
            var users = _users.GetUserNames(recieve.UserSearch);

            List<string> List = new List<string>();

            foreach(var name in users)
            {
                List.Add(name.UserName);
            }

            var send =  new AdminModel
            {
                UserNames = List
            };

            return Json(send);
        }
        
        [HttpPost]
        public JsonResult GetUser([FromBody] AdminModel user)
        {
            var userGet = _userManager.FindByNameAsync(user.UserName);

            var userGet1 = new Users
            {
                Id = userGet.Result.Id,
                UserName = userGet.Result.UserName,
                LastName = userGet.Result.LastName,
                FirstName = userGet.Result.FirstName,
                MiddleInitial = userGet.Result.MiddleInitial,
                PhoneNumber = userGet.Result.PhoneNumber,
                PhoneNumberConfirmed = userGet.Result.PhoneNumberConfirmed,
                Email = userGet.Result.Email,
                EmailConfirmed = userGet.Result.EmailConfirmed,
                UserClaims = userGet.Result.UserClaims,
                UserRoles = userGet.Result.UserRoles,

            };

            return Json(userGet1);
        }

        [HttpPost]
        public JsonResult GetClaims([FromBody] AdminModel user)
        {

            var newUser = _userManager.FindByNameAsync(user.UserName);

            var claimsGet = _users.GetUserClaims(newUser.Result.Id);

             var claims = new AdminModel
            {
               
                UserClaims= claimsGet.Result
            };

            return Json(claims.UserClaims);
        }


        [HttpPost]
        [Authorize(Policy = "IsAdmin")]
        public IActionResult UploadProductPictures()
        {
            long size = 0;
            var files = Request.Form.Files;
            List<string> FileNamesToReturn = new List<string>();
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                FileNamesToReturn.Add( $@"{filename}" + ",");
                filename = _hostingEnv.WebRootPath + $@"\images" + $@"\ProductImages" + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }

                
            }
           
            return Json(FileNamesToReturn);
        }

      /*  [HttpPost]
        [Authorize(Policy = "IsAdmin")]*/
        public IActionResult UploadMainSliderPictures()
        {
            long size = 0;
            var files = Request.Form.Files;
            List<string> FileNamesToReturn = new List<string>();
            foreach (var file in files)
            {
                var filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName
                                .Trim('"');
                FileNamesToReturn.Add($@"{filename}");
                filename = _hostingEnv.WebRootPath + $@"\images" + $@"\IndexPageSliderImages" + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }

            return Json(FileNamesToReturn);
        }

        [HttpPost]
        [Authorize(Policy = "IsAdmin")]
        public IActionResult UpdateMainSliderImages([FromBody] AdminModel model)
        {
            try
            {
                var update = _products.UpdateSliderImages(model.SliderPictureUrl, model.SliderPictureId);

                return Json("Slider Picture Updated!");
            }
            catch { return Json("Slider Picture Upload Error!"); }



        }


        #endregion


    }

}
