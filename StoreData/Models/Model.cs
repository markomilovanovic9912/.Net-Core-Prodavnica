using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StoreData.Models
{
    public class Model
    {
        public int Id { get; set; }
        public virtual Items Items { get; set; }
        public virtual ItemType TypeId { get; set; }
        public virtual Specs SpecsId { get; set; }
        public virtual ItemDepartment ItemDepartment{get;set;}

        [Required]
        [MaxLength(32), MinLength(4)]
        public string Name { get; set; }
    }
}
