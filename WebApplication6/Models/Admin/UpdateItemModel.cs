using Microsoft.AspNetCore.Mvc.Rendering;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication6.Models.Admin
{
    public class UpdateItemModel
    {
        public string Name { get; set; }
        public string LastName { get; set; }

        public string ModelName { get; set; }
        public string ImageUrls { get; set; }
        public Items Items { get; set; }
        public Specs Specs { get; set; }


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
    }
}
