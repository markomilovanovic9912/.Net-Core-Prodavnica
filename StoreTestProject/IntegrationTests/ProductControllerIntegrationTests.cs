using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StoreData;
using StoreData.Models;
using StoreServices;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using WebApplication6.Controllers;
using WebApplication6.Models.Product;
using Xunit;

namespace StoreTestProject.IntegrationTests
{
    public class ProductControllerIntegrationTests : IDisposable
    {


        StoreContext _context;

        ProductModel productModel;

        public ProductControllerIntegrationTests()
        {
            #region MockData
            Users user = new Users { UserName = "fakeUser", Email = "fakeEmail" };

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
                /*  Id = 1,*/
                /*  DayOfReview = DateTime.UtcNow,*/
                Item = item,
                Rating = 5.0,
                Text = "reviewText",
                Users = user

            };

            ImageUrls imageUrl = new ImageUrls
            {
                /* Id = 1,*/
                Item = item,
                Url = "imageUrl"
            };
            #endregion

            productModel = new ProductModel
            {
                Review = review,
                ItemId = 1,
                UserId = 1
            };

            var provider = new ServiceCollection().AddEntityFrameworkSqlServer().BuildServiceProvider();

            var options = new DbContextOptionsBuilder<StoreContext>();

            options.UseSqlServer($"Server=(localdb)\\MSSQLLocalDB;Database=Store_DTB_{Guid.NewGuid()};Trusted_Connection=True;MultipleActiveResultSets=True;")
                .UseInternalServiceProvider(provider).EnableSensitiveDataLogging();

            _context = new StoreContext(options.Options);
            _context.Database.Migrate();

        } 


        [Fact]
        public void ProductController_AddReview_Sucess()
        {
            var productService = new StoreProductService(_context);
            
            var mockUserStore = new Mock<IUserStore<Users>>();

            var manager = new UserManager<Users>(mockUserStore.Object, null, null, null, null, null, null, null, null);

            var Usrclaim = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
             {
                        new Claim(ClaimTypes.NameIdentifier, "1"),
             }));

            var Controller = new ProductController(productService, manager);
            Controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = Usrclaim }
            };

            var addReview = Controller.AddReview(productModel);
            var getReviews= productService.GetAllReviews();
            Assert.Equal("reviewText", getReviews.First().Text);
            Assert.Equal("Review Added!", addReview.Value);
        }


        public void Dispose()
        {
            _context.Database.EnsureDeleted();
        }
    }
}
