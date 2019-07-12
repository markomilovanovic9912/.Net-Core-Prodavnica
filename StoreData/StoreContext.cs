using Microsoft.EntityFrameworkCore;
using System;
using StoreData.Models;

namespace StoreData
{

        public class StoreContext : DbContext
        {

            public StoreContext(DbContextOptions options) : base(options) { }

            public DbSet<Specs> Specs { get; set; }
            public DbSet<ItemType> ItemType { get; set; }
            public DbSet<ItemDepartment> ItemDepartment { get; set; }
            public DbSet<Model> Models { get; set; }
            public DbSet<Manufacturer> Manufacturer { get; set; }
            public DbSet<Reviews> Reviews { get; set; }
            public DbSet<ImageUrls> ImageUrls { get; set; }
            public DbSet<Items> Items { get; set; }
            public DbSet<UserItemHistory> UserItemHistory { get; set; }
            public DbSet<Role> Roles { get; set;}
            public DbSet<Users> Users { get;  set; }
            public DbSet<UserRole> UserRoles { get; set; }
            public DbSet<ExternalUserLogins> ExternalUserLogins { get; set; }
            public DbSet<UserClaims> UserClaims { get; set; }
            public DbSet<Order> Orders { get; set; }
            public DbSet<Status> Status { get; set; }
            public DbSet<BillingInfo> BillingInfo { get; set; }
            public DbSet<PaymentMethod> PaymentMethod { get; set; }
            public DbSet<Invoice> Invoice { get; set; }
            public DbSet<ItemTypeSub> ItemTypeSub { get; set; }
            public DbSet<SliderImageUrls> SliderImageUrls { get; set; }
        }

    }

