using Microsoft.AspNetCore.Mvc.Rendering;
using StoreData;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication6.Models.Order;

namespace WebApplication6.Models.Admin
{
  
    public class AdminModel
    {

        #region Product

        public string ManufacturerName { get; set; }
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public IEnumerable<Manufacturer> Manufacturers { get; set; }

        public List<SelectListItem> GetManuf()
        {
            List<SelectListItem> ManufList = new List<SelectListItem>();
           
            foreach (var manuf in Manufacturers)
            {
                ManufList.Add(new SelectListItem
                {
                    Text = manuf.Name,
                    Value = manuf.Id.ToString()
                });
            }
            return ManufList;
        }


        public string ItemTypeName { get; set; }
        public int ItemTypeId { get; set; }
        public ItemType ItemType { get; set; }
        public IEnumerable<ItemType> ItemTypes { get; set; }

        public List<SelectListItem> GetItemType()
        {
            List<SelectListItem> TypeList = new List<SelectListItem>();

            foreach (var type in ItemTypes)
            {
                TypeList.Add(new SelectListItem
                {
                    Text = type.Name,
                    Value = type.Id.ToString()
                });
            }
            return TypeList;
        }

        public int ItemSubTypeId { get; set; }
        public int ItemSubTypeTypeId { get; set; }
        public ItemTypeSub ItemSubType { get; set; }
        public IEnumerable<ItemTypeSub> ItemSubTypes { get; set; }

        public List<SelectListItem> GetSubItemType()
        {
            List<SelectListItem> SubTypeList = new List<SelectListItem>();

            foreach (var type in ItemSubTypes)
            {
                SubTypeList.Add(new SelectListItem
                {
                    Text = type.SubTypeName,
                    Value = type.Id.ToString()
                });
            }
            return SubTypeList;
        }


        public ItemDepartment ItemDepartment { get; set; }
        public IEnumerable<ItemDepartment> ItemDepartments { get; set; }

        public List<SelectListItem> GetDept()
        {
            List<SelectListItem> DeptList = new List<SelectListItem>();

            foreach (var dept in ItemDepartments)
            {
                DeptList.Add(new SelectListItem
                {
                    Text = dept.DeptName,
                    Value = dept.Id.ToString()
                });
            }
            return DeptList;
        }

        public int ModelId { get; set; }
        public string ModelName { get; set; }
        public Model Model { get; set; }
        public IEnumerable<Model> Models { get; set; }

        public List<SelectListItem> GetModel()
        {
            List<SelectListItem> ModelList = new List<SelectListItem>();

            foreach (var mod in Models)
            {
                ModelList.Add(new SelectListItem
                {
                    Text = mod.Name,
                    Value = mod.Name
                });
            }
            return ModelList;
        }


        public Reviews Review { get; set; }
        public IEnumerable<Reviews> Reviews { get; set; }
        public int ReviewId { get; set; }

        public int Id { get; set; }
        public string Color { get; set; }
        public string ImageUrls { get; set; }
        public Items Items { get; set; }
        public Specs Specs { get; set; }
        public double Price { get; set; }
        public int Discount { get; set; }
        public int Availibility { get; set; }
        public string SpecSpec { get; set; }
        public string SpecDesc { get; set; }
        public int SpecId { get; set; }
        public string Message { get; set; }
        public int SliderPictureId { get; set; }
        public string SliderPictureUrl { get; set; }
        #endregion


        #region User

        public Role Role { get; set; }

        public string UserSearch { get; set; }

        public UserRole UserRole { get; set;}
        public IEnumerable<UserRole> UserRoles { get; set; }

        public UserClaims UserClaim { get; set;}
        public IList <UserClaims> UserClaims { get; set; } 

        public string UserName { get; set; }
        public IEnumerable<string> UserNames { get; set; }
        public Users User { get; set; }
        public IEnumerable<Users> Users { get; set; }

        public List<SelectListItem> GetUsers()
        {
            List<SelectListItem> UserList = new List<SelectListItem>();

            foreach (var usr in Users)
            {
                UserList.Add(new SelectListItem
                {
                    Text = usr.UserName,
                    Value = usr.UserName
                });

               /* UserList.Add(new SelectListItem{
                    Text=usr.Id.ToString(),
                    Value=usr.Id.ToString()
                });*/
            }
            return UserList;
        }

        #endregion

     
    }
}
