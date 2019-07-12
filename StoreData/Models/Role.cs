using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreData.Models
{
    

    [Table("Role")]
    public class Role
    {
        [Key, Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string NormalizedName { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
