using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StoreData;
using StoreData.Models;
using StoreServices;
using System;
using System.Linq;
using System.Threading;
using WebApplication6.Controllers;
using WebApplication6.Models.Admin;
using Xunit;

namespace StoreTestProject
{   
    public class AdminControllerIntegrationTests : IDisposable
    {
        
        StoreContext _context;
     
        AdminModel adminModel;

        public AdminControllerIntegrationTests()
        {
            #region MockData
            Manufacturer manufacturer = new Manufacturer
            {
                /* Id = 1,*/
                Name = "manufacturerName"
            };

       ItemDepartment itemDept = new ItemDepartment
        {
            /*   Id = 1,*/
            DeptName = "DeptName"
        };

       ItemType itemType = new ItemType
        {
            /* Id = 1,*/
            Name = "itemTypeName",
            ItemTypeHeaderImageUrl = "itemTypeHeaderImageUrl",
            ItemGroupName = "itemGroupName"
        };

       ItemTypeSub itemSubType = new ItemTypeSub
        {
            /*   Id = 1,*/
            SubTypeName = "itemSubTypeName",
            ItemType = itemType
        };

        Specs spec = new Specs
        {
             /*  Id = 1, */
            Description = "Description",
            Specification = "Specification",
        };

         Model model = new Model
        {
            /*  Id = 1,*/
            ItemDepartment = itemDept,
            SpecsId = spec,
            TypeId = itemType,
            Name = "modelName"
        };

      Items item = new Items
        {
           /* Id = 1, */
            Availability = 1,
            Color = "itemColor",
            Price = 12,
            Discount = 0,
            ManuModel = manufacturer,
            Model = model,
            ItemTypeSub = itemSubType
        };

        Reviews review = new Reviews
        {
            /*    Id = 1,*/
          /*  DayOfReview = DateTime.UtcNow,*/
            Item = item,
            Rating = 5.0,
            Text = "reviewText"


        };

        ImageUrls imageUrl = new ImageUrls
        {
            /* Id = 1,*/
            Item = item,
            Url = "imageUrl"
        };
            #endregion

            adminModel = new AdminModel
            {
                Availibility = item.Availability,
                Price = item.Price,
                Discount = item.Discount,
                Manufacturer = manufacturer,
                Model = model,
                ImageUrls =imageUrl.Url,
                Color = item.Color,
                ItemSubType = itemSubType,
                ItemDepartment = itemDept,
                ModelName = model.Name,
                Specs = spec,
                ItemType = itemType,
                Review = review
            };


            var provider = new ServiceCollection().AddEntityFrameworkSqlServer().BuildServiceProvider();

            var options = new DbContextOptionsBuilder<StoreContext>();

            options.UseSqlServer($"Server=(localdb)\\MSSQLLocalDB;Database=Store_DTB_{Guid.NewGuid()};Trusted_Connection=True;MultipleActiveResultSets=True; ")
                .UseInternalServiceProvider(provider).EnableSensitiveDataLogging();

            _context = new StoreContext(options.Options);
            _context.Database.Migrate();

        }


        [Fact]
        public void AdminController_AddItem_Success()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);
         
            var Controller = new AdminController(productService, manager, userService, null);
            var addItem = Controller.AddItem(adminModel);
            var getItems = productService.GetAll();
            Assert.Single(getItems);
            Assert.Equal("itemColor",getItems.First().Color);
            Assert.Equal("Item added!", addItem.Value);
        }
 
        [Fact]
        public void AdminController_UpdateItem_Success()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();
            
            var adminUpdateModel = new AdminModel
            {
                Availibility = 1,
                Price = 12,
                Discount = 0,
                Color = "Orange",
                ModelName = "modelName",
                SpecSpec = "Specification",
                SpecDesc = "Description"
            };
      
        var productService = new StoreProductService(_context);
            var userService = new UserService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);
            var Controller = new AdminController(productService, manager, userService, null);

            var addItem = Controller.AddItem(adminModel);

            var getItemsBefore = productService.GetAll();
            Assert.Equal("itemColor", getItemsBefore.First().Color);

            var updateItem = Controller.UpdateItems(adminUpdateModel);

            var getItemsPost = productService.GetAll();
            Assert.Single(getItemsPost);
            Assert.Equal("Orange", getItemsPost.First().Color);
            Assert.Equal(1, getItemsPost.First().Availability);
            Assert.Equal("Item Updated", updateItem.Value);
        }
        
        [Fact]
        public void AdminController_RemoveItem_Success()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var adminRemoveModel = new AdminModel
            {  
                ModelName = "modelName",
            };

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(productService, manager, userService, null);

            var addItem = Controller.AddItem(adminModel);
            var getItemsBefore = productService.GetAll();
            Assert.Single(getItemsBefore);

            var removeItem = Controller.RemoveItem(adminRemoveModel);
            var getItemsAfter = productService.GetAll();
            Assert.Empty(getItemsAfter);
            Assert.Equal("Item Was Sucessfully Removed", removeItem.Value);
        }

        [Fact]
        public void AdminController_AddManufacturer_Success()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var addManufModel = new AdminModel { Manufacturer = adminModel.Manufacturer };

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(productService, manager, userService, null);

            var addManufacturer = Controller.AddManufacturer(addManufModel);
            var getManufacturers = productService.GetAllManufacturers();
            Assert.Equal("manufacturerName", getManufacturers.First().Name);
        }

        [Fact]
        public void AdminController_RemoveManufacturer_Success()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var removeManufModel = new AdminModel { ManufacturerId = 1 };
            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(productService, manager, userService, null);

            var addManufacturer = productService.AddManufacturer(adminModel.Manufacturer);
            var getManufacturersBefore = productService.GetAllManufacturers();
            Assert.Equal("manufacturerName", getManufacturersBefore.First().Name);

            var removeManufacturer = Controller.RemoveManufacturer(removeManufModel);
            var getManufacturersAfter = productService.GetAllManufacturers();
            Assert.Empty(getManufacturersAfter);
        }

        [Fact]
        public void AdminController_AddItemType_Success()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var addItemTypeModel = new AdminModel { ItemType = adminModel.ItemType };

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(productService, manager, userService , null);

            var addItemType = Controller.AddItemType(addItemTypeModel);
            var getItemTypes = productService.GetAllItemTypes();
            Assert.Equal("itemTypeName", getItemTypes.First().Name);
        }

        [Fact]
        public void AdminController_RemoveItemType_Success()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var removeItemType = new AdminModel { ItemTypeId = 1 };
            var addItemTypeModel = new AdminModel { ItemType = adminModel.ItemType };
            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(productService, manager, userService, null);

            var addItemType = Controller.AddItemType(addItemTypeModel);
            var getItemTypesBefore = productService.GetAllItemTypes();
            Assert.Equal("itemTypeName", getItemTypesBefore.First().Name);

            var removeManufacturer = Controller.RemoveItemType(removeItemType);
            var getItemTypesAfter = productService.GetAllItemTypes();
            Assert.Empty(getItemTypesAfter);
        }

        [Fact]
        public void AdminController_AddItemSubType_Success()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var addItemSubTypeModel = new AdminModel { ItemSubType = adminModel.ItemSubType };

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(productService, manager, userService, null);

            var addItemSubType = Controller.AddItemSubType(addItemSubTypeModel);
            var getItemSubTypes = productService.GetItemSubTypes(adminModel.ItemType.Name);
            Assert.Equal("itemSubTypeName", getItemSubTypes.First().SubTypeName);
        }

        [Fact]
        public void AdminController_RemoveItemSubType_Success()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var addItemSubTypeModel = new AdminModel { ItemSubType = adminModel.ItemSubType };
            var removeItemSubType = new AdminModel { ItemSubTypeId = 1 };

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(productService, manager, userService, null);

            var addItemSubType = Controller.AddItemSubType(addItemSubTypeModel);
            var getItemSubTypesBefore = productService.GetItemSubTypes(adminModel.ItemType.Name);
            Assert.Equal("itemSubTypeName", getItemSubTypesBefore.First().SubTypeName);

            var removeManufacturer = Controller.RemoveItemSubType(removeItemSubType);
            var getItemSubTypesAfter = productService.GetItemSubTypes(adminModel.ItemType.Name);
            Assert.Empty(getItemSubTypesAfter);
        }

        [Fact]
        public void AdminController_RemoveReview_Success()
        {
            Users user = new Users { Id = 1, UserName = "fakeUser" };
            var cancel = new CancellationToken();

            var removeReview = new AdminModel { ReviewId = 1};

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(productService, manager, userService, null);

            var addReview = productService.AddReview(adminModel.Review);
            var getReviewsBefore = productService.GetReviews(1);
            Assert.Equal("reviewText", getReviewsBefore.First().Text);
             
            var removeManufacturer = Controller.RemoveReview(removeReview);
            var getReviewsAfter = productService.GetReviews(1);
            Assert.Empty(getReviewsAfter); 
        }

        [Fact]
        public void AdminController_RemoveUser_Success()
        {
            Users user = new Users { UserName = "fakeUser", Email = "fakeEmail"};
            var cancel = new CancellationToken();

            var removeUserModel = new AdminModel {  UserName = user.UserName};

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(productService, manager, userService, null);

            var addUser = userService.AddUser(user);
            var getUsersBefore = userService.GetAllUsers();
            Assert.Equal("fakeUser", getUsersBefore.First().UserName);
            Assert.Single(getUsersBefore);

            var removeUser = Controller.UpdateRemoveUser(removeUserModel);
            var getUsersAfter = userService.GetAllUsers();
            Assert.Empty(getUsersAfter);
            Assert.Equal("User Removed!", removeUser.Value);
        }

        [Fact]
        public void AdminController_AddUserClaim_Success()
        {
            Users user = new Users {  UserName = "fakeUser", Email = "fakeEmail" };
            UserClaims userClaim = new UserClaims { ClaimType = "fakeClaimType", ClaimValue = "fakeClaimValue", UserId = 1  };
            var cancel = new CancellationToken();

            var addUserClaimModel = new AdminModel { UserClaim = userClaim};

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);
            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(productService, manager, userService, null);

            var addUser = userService.AddUser(user);
            var addUserClaim = Controller.AddUserClaim(addUserClaimModel);
            var getUserClaims = userService.GetUserClaims(1);
            Assert.Single(getUserClaims.Result);
            Assert.Equal("fakeClaimValue", getUserClaims.Result.First().ClaimValue);
            Assert.Equal("User Claim Added !", addUserClaim.Value);
        }

        [Fact]
        public void AdminController_RemoveUserClaim_Success()
        {
            Users user = new Users { UserName = "fakeUser" , Email = "fakeEmail" };
            UserClaims userClaim = new UserClaims {Id=1, ClaimType = "fakeClaimType", ClaimValue = "fakeClaimValue", UserId = 1 };
            var cancel = new CancellationToken();

            var addUserClaimModel = new AdminModel { UserClaim = userClaim };
            var removeUserClaimModel = new AdminModel { UserClaim = userClaim };

            var productService = new StoreProductService(_context);
            var userService = new UserService(_context);

            var mockUserStore = new Mock<IUserStore<Users>>();
            mockUserStore.Setup(u => u.CreateAsync(user, cancel));

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Controller = new AdminController(productService, manager, userService, null);

            var addUser = userService.AddUser(user);
            var addUserClaim = Controller.AddUserClaim(addUserClaimModel);
            var getUserClaimsBefore = userService.GetUserClaims(1);
            Assert.Single(getUserClaimsBefore.Result);
            Assert.Equal("fakeClaimValue", getUserClaimsBefore.Result.First().ClaimValue);

            var removeUserClaim = Controller.RemoveUserClaim(removeUserClaimModel);
            var getUserClaimsAfter = userService.GetUserClaims(1);
            Assert.Empty(getUserClaimsAfter.Result);
            Assert.Equal("User Claim Removed!", removeUserClaim.Value);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
