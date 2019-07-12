using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoreData.Models
{
    public class ItemDepartment
    {
        public int Id { get; set; }

        public virtual Model Model { get; set; }

        [Required]
        [MaxLength(32), MinLength(4)]
        public string DeptName { get; set; }


    }
}
