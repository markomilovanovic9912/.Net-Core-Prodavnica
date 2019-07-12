using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using StoreData;
using StoreData.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreServices
{
    public class StoreProductService : IProductService
    {
        private StoreContext _context;

        public StoreProductService(StoreContext context)
        {
            _context = context;
        }
 
        public IEnumerable<Items> GetAll()
        {
            return _context.Items.Include(item => item.ManuModel).Include(item => item.Model);
        }

        /* public IEnumerable<Items> GetItemsByIds(List<int> itemId)
         {
             List<Items> list = new List<Items>();
             var context = _context.Items.AsEnumerable();

             foreach (var item in context)
             {
                 foreach (var id in itemId)
                 {

                     if (item.Id == id && item.Availability > 0)
                     {
                         list.Add(item);
                     }
                 }
             }

             return list;
         }*/

        public IEnumerable<Items> GetItemsByIds(List<int> itemId)
        {
            var items = _context.Items.Where(i => itemId.Contains(i.Id) && i.Availability > 0);
            return items.ToList();
        }

        /*public IEnumerable<Items> GetItemsRefreshFilter(List<int> itemId, string name, IEnumerable<string> manufacturer, IEnumerable<string> subCat)
        {
            var items = _context.Items.Where(item => item.Model.TypeId.Name == name && item.Availability > 0);

            if (manufacturer != null || subCat != null)
            {
                var old = GetItemsByIds(itemId);
                var oldFilterBrand = FilterByBrand(old, manufacturer);
                var oldReturn = FilterByCategory(oldFilterBrand, subCat);

                var newFilterBrand = FilterByBrand(items, manufacturer);
                var newReturn = FilterByCategory(newFilterBrand, subCat);

                var together = oldReturn.Union(newReturn, new IndexComparer());

                return together;
            }

            else { return items; }

        }*/

        public IEnumerable<Items> GetItemsRefreshFilter(string name, IEnumerable<string> manufacturer, IEnumerable<string> subCat)
        {
            var items = _context.Items.Where(item => item.Model.TypeId.Name == name && item.Availability > 0);

            if (manufacturer != null || subCat != null)
            {
                var newFilterBrand = FilterByBrand(items, manufacturer);
                var newReturn = FilterByCategory(newFilterBrand, subCat);             

                return newReturn;
            }
            else { return items; }
        }


        #region HelperMethods
        /*public IEnumerable<Items> FilterByBrand(IEnumerable<Items> PLModel, IEnumerable<string> Manufacturer)
        {
            if (Manufacturer != null)
            {
                List<Items> mdl = new List<Items>();

                foreach (var plm in PLModel)
                {
                    foreach (var man in Manufacturer)
                    {
                        if (GetManufacturerNameString(plm.Id) == man)
                        {
                            mdl.Add(plm);
                        }
                    }
                }


                return mdl;
            }

            else { return PLModel; }

        }*/

        public IEnumerable<Items> FilterByBrand(IEnumerable<Items> PLModel, IEnumerable<string> Manufacturer)
        {
            if (Manufacturer != null)
            {
                var modelBrand = PLModel.Where(plm => Manufacturer.Contains(GetManufacturerNameString(plm.Id)));
                return modelBrand;
            }

            else { return PLModel; }

        }

        /*  public IEnumerable<Items> FilterByCategory(IEnumerable<Items> PLModel, IEnumerable<string> Category)
          {

              if (Category != null)
              {

                  List<Items> mdl = new List<Items>();

                  foreach (var plm in PLModel)
                  {
                      foreach (var sub in Category) {
                          if (GetSubCategory(plm.Id) == sub)
                          {
                              mdl.Add(plm);
                          }
                      }
                  }

                  return mdl;
              }

              else { return PLModel; }

          }*/

        public IEnumerable<Items> FilterByCategory(IEnumerable<Items> PLModel, IEnumerable<string> Category)
        {
            if (Category != null)
            {
                var modelCategory = PLModel.Where(plm => Category.Contains(GetSubCategory(plm.Id)));
                return modelCategory;
            }

            else { return PLModel; }

        }

        public class IndexComparer : IEqualityComparer<Items>
        {
            public bool Equals(Items prod, Items prod2)
            {
                return prod.Id.Equals(prod2.Id);
            }

            public int GetHashCode(Items obj)
            {
                return obj.Id.GetHashCode();
            }
        }
        #endregion


        public int GetAvailability(int id)
        {
            return _context.Items.FirstOrDefault(item => item.Id == id).Availability;
        }

        public Items GetById(int id)
        {
            return _context.Items.FirstOrDefault(item => item.Id == id);
        }

        IEnumerable<ImageUrls> IProductService.GetImageUrls(int id)
        {
            return _context.ImageUrls.Where(item => item.Item.Id == id);
        }


        public double GetPrice(int id)
        {
            return _context.Items.FirstOrDefault(item => item.Id == id).Price;
        }

        public Model GetModelName(int id)
        {
            var model = _context.Models.FirstOrDefault(item => item.Items.Id == id);

            return model;

        }

        public Manufacturer GetProductName(int id)
        {
            var model = _context.Manufacturer.FirstOrDefault(item => item.Items.Id == id);

            return model;
        }

        public IEnumerable<Reviews> GetReviews(int id)
        {
            return _context.Reviews.Where(item => item.Item.Id == id).Include(item => item.Users);
        }

        public Specs GetSpecs(int id)
        {
            return _context.Specs.FirstOrDefault(item => item.Model.Id == id);
        }

        public ItemType GetType(int id)
        {
            return _context.ItemType.FirstOrDefault(item => item.Model.Id == id);
        }

        public string GetFirstImage(int id)
        {
            return _context.ImageUrls.First(item => item.Item.Id == id).Url;
        }

        public IEnumerable<Items> GetByType(string name)
        {
            return _context.Items.Include(item => item.ManuModel).Include(item => item.Model).Include(item => item.ItemTypeSub).Where(item => item.Model.TypeId.Name == name);
        }

        public string GetItemTypeHeaderImg(string itemGroup)
        {
            var itmGrp = _context.ItemType.FirstOrDefault(it => it.Name == itemGroup);

            return itmGrp.ItemTypeHeaderImageUrl;
        }


        public double GetAvrageRating(int id)
        {
            var review = GetReviews(id);
            var revNumber = 0;
            var sum = 0.0;
            double avrage = 0.0;

            foreach (var product in review)
            {

                sum += product.Rating;

                revNumber++;

            }

            avrage = sum / revNumber;

            return avrage;
        }

        /*public List<int> GetHistoryByUserId(int id)
        {
            var items = _context.UserItemHistory.Where(i => i.UserId == id);

            var ids = new List<int>();

            foreach (var item in items)
            {
                ids.Add(item.ItemId);
            }

            if (ids == null)
            {

                throw new NullReferenceException();
            }

            return ids;
        }*/

        /*public IEnumerable<Items> GetByHistoryId(List<int> itemId)
        {

            List<Items> list = new List<Items>();
            var context = _context.Items.AsEnumerable();

            foreach (var item in context)
            {
                foreach (var id in itemId)
                {

                    if (item.Id == id)
                    {
                        list.Add(item);
                    }
                }
            }

            return list;
        }*/

        public List<int> GetHistoryByUserId(int id)
        {
            var items = _context.UserItemHistory.Where(i => i.UserId == id).Select(u => u.ItemId);

            return items.ToList();
        }

        public IEnumerable<Items> GetByHistoryId(List<int> itemId)
        {
            var items = _context.Items.Where(i => itemId.Contains(i.Id));

            return items;
        } 

        public IEnumerable<Items> GetItemsBySubType(string itemSubType)
        {

            var items = _context.Items.Where(i => i.ItemTypeSub.SubTypeName == itemSubType).OrderBy(i => Guid.NewGuid());

            return items;

        }


        public IEnumerable<Items> GetRelatedItems(string itemSubType)
        {

            List<Items> list = new List<Items>();
            /* var context = _context.Items.AsEnumerable();*/

            var context = GetItemsBySubType(itemSubType).Take(7);


            return context;
        }


        public Task<UserItemHistory> AddItemHistory(int id, int userId)
        {

            var exist = _context.UserItemHistory.FirstOrDefault(u => u.ItemId == id && u.UserId == userId);
            var last = _context.UserItemHistory.Where(i => i.UserId == userId).Take(1);

            var number = GetHistoryByUserId(userId);

            if (number.Count >= 11)
            {
                _context.UserItemHistory.RemoveRange(last);
                _context.SaveChanges();
            }
            if (exist == null)
            {

                var item = new UserItemHistory
                {
                    UserId = userId,
                    ItemId = id,
                    TimeOfAccess = DateTimeOffset.Now

                };


                _context.UserItemHistory.Add(item);
                _context.SaveChanges();
            }

            return null;
        }

        public IEnumerable<Manufacturer> GetAllManufacturers()
        {
            return _context.Manufacturer;
        }

        public IEnumerable<ItemType> GetAllItemTypes()
        {
            return _context.ItemType;
        }

        public IEnumerable<ItemDepartment> GetAllItemDepartments()
        {
            return _context.ItemDepartment;
        }

        public IEnumerable<Model> GetAllModels()
        {
            return _context.Models.OrderBy(m => m.Name);
        }


        public IEnumerable<ItemTypeSub> GetItemSubTypes(string itemType)
        {
            var subTypes = _context.ItemTypeSub.Where(its => its.ItemType.Name == itemType);

            return subTypes;
        }

        public IEnumerable<Model> GetModelsByItemType(string itemType)
        {
            var models = _context.Models.Where(its => its.TypeId.Name == itemType);

            return models;
        }

        public Task<Items> AddItem(Items item, Specs specs, Model model)
        {

            var specToAdd = new Specs
            {
                Description = specs.Description,
                Specification = specs.Specification,
            };

            var modelToAdd = new Model
            {
                ItemDepartment = model.ItemDepartment,
                SpecsId = specToAdd,
                TypeId = model.TypeId,
                Name = model.Name
            };

            var itemToAdd = new Items
            {
                Availability = item.Availability,
                Color = item.Color,
                Price = item.Price,
                Discount = item.Discount,
                ManuModel = item.ManuModel,
                Model = modelToAdd,
                ImageUrls = item.ImageUrls,
                ItemTypeSub = item.ItemTypeSub
            };


            _context.ItemDepartment.Attach(model.ItemDepartment);
            _context.ItemType.Attach(model.TypeId);
            _context.ItemTypeSub.Attach(item.ItemTypeSub);
            _context.Manufacturer.Attach(item.ManuModel);

            _context.Items.Add(itemToAdd);

            _context.SaveChanges();

            return null;
        }


        public Task<Items> RemoveItem(string modelName)
        {

            var item = _context.Items.FirstOrDefault(i => i.Model.Name == modelName);

            var model = _context.Models.FirstOrDefault(m => m.Name == modelName);

            var specs = _context.Specs.FirstOrDefault(s => s.Model.Name == modelName);


            _context.Items.Remove(item);
            _context.Models.Remove(model);
            _context.Specs.Remove(specs);

            _context.SaveChanges();

            return null;
        }

        public Task<Items> UpdateItem(Items item, Specs specs, Model model)
        {
            var itemToUpdt = GetItemByModelName(model.Name);
            var specToUpdt = GetSpecsByName(model.Name);

            if (specToUpdt != null)
            {
                specToUpdt.Description = specs.Description;
                specToUpdt.Specification = specs.Specification;
            };


            if (itemToUpdt != null)
            {
                itemToUpdt.Availability = item.Availability;
                itemToUpdt.Price = item.Price;
                itemToUpdt.Discount = item.Discount;
                itemToUpdt.Color = item.Color;
            };


            _context.Specs.Update(specToUpdt);
            _context.SaveChanges();
            _context.Items.Update(itemToUpdt);

            _context.SaveChanges();

            return null;
 
        }


        public Task<Specs> AddSpecs(Specs specs)
        {
            var specToAdd = new Specs
            {
                Description = specs.Description,
                Specification = specs.Specification,
            };

            _context.Specs.Add(specToAdd);
            _context.SaveChanges();

            return null;
        }

        public Task<Model> AddModel(Model model)
        {
            var modelToAdd = new Model
            {
                ItemDepartment = model.ItemDepartment,
                SpecsId = model.SpecsId,
                TypeId = model.TypeId,
                Name = model.Name
            };

            _context.Specs.Attach(model.SpecsId);
            _context.ItemDepartment.Attach(model.ItemDepartment);
            _context.ItemType.Attach(model.TypeId);
            _context.Models.Add(modelToAdd);
            _context.SaveChanges();

            return null;

        }

        public Task<ItemType> AddItemType(ItemType itemType)
        {
            var itemTypeToAdd = new ItemType
            {
                Name = itemType.Name
            };

            _context.ItemType.Add(itemTypeToAdd);
            _context.SaveChanges();

            return null;
        }

        public Task<ItemType> RemoveItemType(int Id)
        {
            var itemTypeRem = _context.ItemType.FirstOrDefault(i => i.Id == Id);

            _context.ItemType.Remove(itemTypeRem);
            _context.SaveChanges();
            return Task.FromResult(itemTypeRem);
        }

        public Task<ItemTypeSub> AddItemSubType(ItemTypeSub itemSubType)
        {
            var itemSubTypeToAdd = new ItemTypeSub
            {
                SubTypeName = itemSubType.SubTypeName,
                ItemType = itemSubType.ItemType
            };

            _context.ItemType.Attach(itemSubType.ItemType);
            _context.ItemTypeSub.Add(itemSubTypeToAdd);
            _context.SaveChanges();

            return null;
        }

        public Task<ItemTypeSub> RemoveItemSubType(int Id)
        {
            var itemSubTypeRem = _context.ItemTypeSub.FirstOrDefault(i => i.Id == Id);

            _context.ItemTypeSub.Remove(itemSubTypeRem);
            _context.SaveChanges();
            return Task.FromResult(itemSubTypeRem);
        }

        public Task<ItemDepartment> AddItemDepartment(ItemDepartment itemDepartment)
        {
            var itemDeptToAdd = new ItemDepartment
            {
                DeptName = itemDepartment.DeptName
            };

            _context.ItemDepartment.Add(itemDeptToAdd);
            _context.SaveChanges();

            return null;
        }

        public Manufacturer GetManufacturerByName(string name)
        {
            var manufacturer = _context.Manufacturer.FirstOrDefault(i => i.Name == name);

            return manufacturer;
        }

        public Task<Manufacturer> AddManufacturer(Manufacturer manufacturer)
        {
            var manuToAdd = new Manufacturer
            {
                Name = manufacturer.Name
            };

            _context.Manufacturer.Add(manuToAdd);
            _context.SaveChanges();

            return null;
        }

        public Task<Manufacturer> RemoveManufacturer(int Id)
        {


            var manuf = _context.Manufacturer.FirstOrDefault(m => m.Id == Id);

            _context.Manufacturer.Remove(manuf);
            _context.SaveChanges();
            return null;
        }

        public Items GetItemByModelName(string modelName)
        {
            var item = _context.Items.FirstOrDefault(i => i.Model.Name == modelName);

            return item;
        }

        public Specs GetSpecsByName(string modelName)
        {
            var specs = _context.Specs.FirstOrDefault(m => m.Model.Name == modelName);

            return specs;
        } 

        public Task<Reviews> RemoveReview(int id)
        {

            var reviewRemove = _context.Reviews.FirstOrDefault(i => i.Id == id);

            _context.Reviews.Remove(reviewRemove);
            _context.SaveChanges();

            return null;
        }


        public IEnumerable<Reviews> GetReveiwsByModelName(string modelName)
        {
            var reviews = _context.Reviews.Where(r => r.Item.Model.Name == modelName);

            return reviews;
        }

        public Task<Reviews> AddReview(Reviews review)
        {
            var reviewToAdd = new Reviews
            {
                Item = review.Item,
                Rating = review.Rating,
                DayOfReview = DateTime.Now,
                Text = review.Text,
                Users = review.Users

            };

            _context.Reviews.Add(reviewToAdd);
            _context.SaveChanges();

            return null;
        }

        public string GetModelNameString(int id)
        {
            var model = _context.Models.FirstOrDefault(item => item.Items.Id == id);

            return model.Name;
        }

        public string GetManufacturerNameString(int id)
        {
            var model = _context.Manufacturer.FirstOrDefault(item => item.Items.Id == id);

            return model.Name;
        }

        public string GetSubCategory(int id)
        {
            var subCat = _context.ItemTypeSub.FirstOrDefault(cat => cat.Items.Id == id);

            return subCat.SubTypeName;
        }

        public string GetItemGroup(int id)
        {
            var type = GetType(id);

            return type.ItemGroupName;

        }

        public IEnumerable<Items> SearchProducts(string prodSearch)
        {
            var returnList = new HashSet<Items>();
            if (prodSearch.Length > 2)
            {
                var check = prodSearch.Split(" ").ToList();
              

                var items = _context.Items.Include(i => i.Model).Include(i => i.ManuModel);

                foreach (var itm in items)
                {
                    foreach (var str in check)
                    {
                        if (itm.ManuModel.Name.Contains(str) || itm.Model.Name.Contains(str))
                        {
                            returnList.Add(itm);
                        }
                    }
                }

            }


            return returnList;
        }

        public IEnumerable<Reviews> GetAllReviews()
        {
            return _context.Reviews;
        }

        public IEnumerable<SliderImageUrls> UpdateSliderImages(string imageUrl, int imageId)
        {
            var urlToUpdate = _context.SliderImageUrls.FirstOrDefault(s => s.Id == imageId);

            if (urlToUpdate != null)
            {
                urlToUpdate.Url = imageUrl;

            }

            _context.Update(urlToUpdate);
            _context.SaveChanges();

            return null;
        }

        public IEnumerable<SliderImageUrls> GetAllSliderImages()
        {
            var sliderImages = _context.SliderImageUrls;

            return sliderImages;
        }

        public string DecreaseNumberOfItems(IEnumerable<Items> items)
        {
            if (items.Count() > 0)
            {
                foreach (var item in items)
                {                    
                        var itemToUpd = GetById(item.Id);
                        itemToUpd.Availability = itemToUpd.Availability - item.Availability;
                        _context.Update(itemToUpd);
                    _context.SaveChanges();
                };
              
            }

            return "Availability Updated";

        }
    }
}
 