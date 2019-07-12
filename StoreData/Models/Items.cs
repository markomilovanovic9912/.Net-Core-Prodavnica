using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StoreData.Models
{

    public class Items
    {
        public int Id { get; set; }
        public virtual Manufacturer ManuModel { get; set; }
        public virtual Model Model{ get; set; }
        public virtual ItemTypeSub ItemTypeSub { get; set; }

        [Required]
        [Range(1, 100)]
        public int Availability { get; set; }

        [Required]
        public double Price { get; set; }

        public int Discount { get; set; }

        [Required]
        [MaxLength(32), MinLength(4)]
        public string Color { get; set; }

        public virtual ICollection<Reviews> Reviews { get; set; }
        public virtual ICollection<ImageUrls> ImageUrls { get; set; }


    }

}
