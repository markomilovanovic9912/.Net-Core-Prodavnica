using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace StoreData.Models
{


    [Table("UserItemHistory")]
    public class UserItemHistory
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public DateTimeOffset? TimeOfAccess { get; set; }

        [Required, ForeignKey(nameof(Users))]
        public int UserId { get; set; }

        [Required, ForeignKey(nameof(Items))]
        public int ItemId { get; set; }

        public virtual Users Users {get;set;}

        public virtual Items Items { get; set; }

    }
}
