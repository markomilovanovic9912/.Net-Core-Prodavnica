using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoreData.Models
{
    public class ItemTypeSub
    {

        public int Id { get; set; }
        public virtual Items Items { get; set; }
        public virtual ItemType ItemType { get; set; }

        [Required]
        [MaxLength(32), MinLength(4)]
        public string SubTypeName { get; set; }
    }
}
