using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace StoreData.Models
{
    [Table("Users")]
    public class Users
    {
        
            [Key, Required]
            public int Id { get; set; }

            [Required, MaxLength(128)]
            public string UserName { get; set; }

            [Required, MaxLength(1024)]
            public string PasswordHash { get; set; }

            [Required, MaxLength(128)]
            public string Email { get; set; }

            public bool EmailConfirmed { get; set; }

            public bool LockoutEnabled { get; set; }

            public DateTimeOffset? LockoutEnd { get; set; }

            public int LoginTry { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public bool PhoneNumberConfirmed { get; set; }

            [Display(Name = "Adress")]
            public string Adress { get; set; }

            [Display(Name = "City")]
            public string City { get; set; }

            [Display(Name = "Country/State")]
            public string CountryOrState { get; set; }

            public string NormalizedEmail { get; set; }

            [MaxLength(32)]
            public string FirstName { get; set; }

            [MaxLength(1)]
            public string MiddleInitial { get; set; }

            [MaxLength(32)]
            public string LastName { get; set; }

            public virtual ICollection<UserRole> UserRoles { get; set; }

            public virtual ICollection<UserClaims> UserClaims { get; set; }
    }
}
