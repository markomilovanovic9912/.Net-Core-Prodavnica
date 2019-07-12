using Moq;
using StoreData;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StoreTestProject
{
    public class ProductMethodsUnitTests  
    {       

        public class MockData
        {
            public static Manufacturer manufacturer = new Manufacturer
            {
                Id = 1,
                Name = "manufacturerName"
            };

            public static ItemDepartment itemDept  = new ItemDepartment
            {
                Id = 1,
                DeptName = "DeptName"
            };

            public static ItemType itemType = new ItemType
            {
                Id = 1,
                Name = "itemTypeName",
                ItemTypeHeaderImageUrl = "itemTypeHeaderImageUrl",
                ItemGroupName = "itemGroupName"
            };

            public static ItemTypeSub itemSubType = new ItemTypeSub
            {
                Id = 1,
                SubTypeName = "itemSubTypeName",
                ItemType = itemType
            };

            public static Specs spec  = new Specs
            {
                Id = 1,
                Description = "Description",
                Specification = "Specification",
            };

            public static Model model  = new Model
            {
                Id = 1,
                ItemDepartment = itemDept ,
                SpecsId = spec ,
                TypeId = itemType ,
                Name = "modelName"
            };

            public static Items item  = new Items
            {
                Id = 1,
                Availability = 1,
                Color = "itemColor",
                Price = 12,
                Discount = 0,
                ManuModel = manufacturer,
                Model = model ,
                ItemTypeSub = itemSubType 
            };

            public static Reviews review = new Reviews
            {
                Id = 1,
                DayOfReview = DateTime.UtcNow,
                Item = item,
                Rating = 5.0,
                Text = "reviewText"


            };

            public static ImageUrls imageUrl = new ImageUrls
            {
                Id = 1,
                Item = item,
                Url = "imageUrl"
            };

        }

        public class MockData2
        {
            public static Manufacturer manufacturer2 = new Manufacturer
            {
                Id = 2,
                Name = "manufacturerName2"
            };

            public static ItemDepartment itemDept2 = new ItemDepartment
            {
                Id = 2,
                DeptName = "DeptName"
            };

            public static ItemType itemType2 = new ItemType
            {
                Id = 2,
                Name = "itemTypeName",
                ItemTypeHeaderImageUrl = "itemTypeHeaderImageUrl",
                ItemGroupName = "itemGroupName"
            };

            public static ItemTypeSub itemSubType2 = new ItemTypeSub
            {
                Id = 2,
                SubTypeName = "itemSubTypeName",
                ItemType = itemType2
            };

            public static Specs spec2 = new Specs
            {
                Id = 2,
                Description = "Description2",
                Specification = "Specification2",
            };

            public static Model model2 = new Model
            {
                Id = 2,
                ItemDepartment = itemDept2,
                SpecsId = spec2,
                TypeId = itemType2,
                Name = "modelName2"
            };

            public static Items item2 = new Items
            {
                Id = 2,
                Availability = 2,
                Color = "itemColor2",
                Price = 10,
                Discount = 0,
                ManuModel = manufacturer2,
                Model = model2,
                ItemTypeSub = itemSubType2
            };

            public static Reviews review2 = new Reviews
            {
                Id = 2,
                DayOfReview = DateTime.UtcNow,
                Item = item2,
                Rating = 4.0,
                Text = "reviewText2"


            };

        }
       

        [Fact]
        public void Check_GetAll_NotNull()
        {
            List<Items> ItemsReturn = new List<Items>
            {
                MockData.item,
                MockData2.item2
            };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetAll()).Returns(ItemsReturn);

            Assert.NotEmpty(mock.Object.GetAll());
            Assert.Equal(2,mock.Object.GetAll().Count());
        }
   
         [Fact]
         public void Check_GetAll_Null_Or_Empty()
         {
            List<Items> ItemsReturn = new List<Items>();

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetAll()).Returns(ItemsReturn);

            Assert.Empty(mock.Object.GetAll());

        }
       
        [Fact]
        public void Check_GetById_NotNull()
        {  
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetById(1)).Returns(MockData.item);

            Assert.NotNull(mock.Object.GetById(1));
        }

       [Fact]
        public void Check_GetByIdS_NotNull_Count_FirstId()
        {
            Items item = new Items
            {
                Id = 2,
                Availability = 1,
                Color = "itemColor",
                Price = 13,
                Discount = 0,
                ManuModel = MockData.manufacturer,
                Model = MockData.model,
                ItemTypeSub = MockData.itemSubType
            };

            List<int> itemIds = new List<int>{1,2};
            List<Items> ItemsReturnList = new List<Items> { MockData.item, item };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetItemsByIds(itemIds)).Returns(ItemsReturnList);

            Assert.NotEmpty(mock.Object.GetItemsByIds(itemIds));
            Assert.Equal(2, mock.Object.GetItemsByIds(itemIds).Count());
            Assert.Equal(1, mock.Object.GetItemsByIds(itemIds).First().Id);
            Assert.Equal("itemColor", mock.Object.GetItemsByIds(itemIds).Last().Color);
        }

        [Fact]
        public void Check_GetByType_NotNull()
        {
            Items item = new Items
            {
                Id = 2,
                Availability = 1,
                Color = "itemColor",
                Price = 13,
                Discount = 0,
                ManuModel = MockData.manufacturer,
                Model = MockData.model,
                ItemTypeSub = MockData.itemSubType
            };

            List<Items> ItemsReturnList = new List<Items> { MockData.item, item };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetByType(MockData.itemType.Name)).Returns(ItemsReturnList);

            Assert.NotEmpty(mock.Object.GetByType(MockData.itemType.Name));
            Assert.Equal(2, mock.Object.GetByType(MockData.itemType.Name).Count());

        }

        [Fact]
        public void Check_GetByHistoryId_NotNull()
        {
            Items item = new Items
            {
                Id = 2,
                Availability = 1,
                Color = "itemColor",
                Price = 13,
                Discount = 0,
                ManuModel = MockData.manufacturer,
                Model = MockData.model,
                ItemTypeSub = MockData.itemSubType
            };

            List<int> itemIds = new List<int> { 1, 2 };

            List<Items> ItemsReturnList = new List<Items> { MockData.item, item };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetByHistoryId(itemIds)).Returns(ItemsReturnList);

            Assert.NotEmpty(mock.Object.GetByHistoryId(itemIds));
            Assert.Equal(2, mock.Object.GetByHistoryId(itemIds).Count());

        }

        [Fact]
        public void Check_GetItemsBySubType_NotNull()
        {
            Items item = new Items
            {
                Id = 2,
                Availability = 1,
                Color = "itemColor",
                Price = 13,
                Discount = 0,
                ManuModel = MockData.manufacturer,
                Model = MockData.model,
                ItemTypeSub = MockData.itemSubType
            };

            List<Items> ItemsReturnList = new List<Items> { MockData.item, item };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetItemsBySubType(MockData.itemSubType.SubTypeName)).Returns(ItemsReturnList);

            Assert.NotEmpty(mock.Object.GetItemsBySubType(MockData.itemSubType.SubTypeName));
            Assert.Equal(2, mock.Object.GetItemsBySubType(MockData.itemSubType.SubTypeName).Count());

        }

        [Fact]
        public void Check_GetRelatedItems_NotNull()
        {
            Items item = new Items
            {
                Id = 2,
                Availability = 1,
                Color = "itemColor",
                Price = 13,
                Discount = 0,
                ManuModel = MockData.manufacturer,
                Model = MockData.model,
                ItemTypeSub = MockData.itemSubType
            };

            List<Items> ItemsReturnList = new List<Items> { MockData.item, item };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetRelatedItems(MockData.itemSubType.SubTypeName)).Returns(ItemsReturnList);

            Assert.NotEmpty(mock.Object.GetRelatedItems(MockData.itemSubType.SubTypeName));
            Assert.Equal(2, mock.Object.GetRelatedItems(MockData.itemSubType.SubTypeName).Count());

        }
        
        [Fact]
        public void GetItemByModelName_NotNull()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetItemByModelName(MockData.model.Name)).Returns(MockData.item);

            Assert.NotNull(mock.Object.GetItemByModelName(MockData.model.Name));
        }

        [Fact]
        public void Check_GetPrice()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetPrice(1)).Returns(11.00);

            Assert.Equal(11.00, mock.Object.GetPrice(1));
        }

        [Fact]
        public void Check_GetAvailability()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetAvailability(1)).Returns(12);

            Assert.Equal(12, mock.Object.GetAvailability(1));
        }
       
         [Fact]
         public void GetModelName_NotNull()
         {               
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetModelName(1)).Returns(MockData.model);

            Assert.NotNull(mock.Object.GetModelName(1));
            Assert.Equal(MockData.model.Name, mock.Object.GetModelName(1).Name);
         }

        [Fact]
        public void GetModelNameString_NotNull()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetModelNameString(1)).Returns(MockData.model.Name);

            Assert.NotNull(mock.Object.GetModelNameString(1));
            Assert.Equal(MockData.model.Name, mock.Object.GetModelNameString(1));
        }

        [Fact]
        public void GetManufacturerName_NotNull()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetProductName(1)).Returns(MockData.manufacturer);

            Assert.NotNull(mock.Object.GetProductName(1));
            Assert.Equal(MockData.manufacturer.Name, mock.Object.GetProductName(1).Name);
        }

        [Fact]
        public void GetManufacturerByName_NotNull()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetManufacturerByName(MockData.manufacturer.Name)).Returns(MockData.manufacturer);

            Assert.NotNull(mock.Object.GetManufacturerByName(MockData.manufacturer.Name));
            Assert.Equal(MockData.manufacturer.Name, mock.Object.GetManufacturerByName(MockData.manufacturer.Name).Name);
        }

        [Fact]
        public void GetManufacturerNameString_NotNull()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetManufacturerNameString(MockData.item.Id)).Returns(MockData.manufacturer.Name);

            Assert.NotNull(mock.Object.GetManufacturerNameString(MockData.item.Id));
            Assert.Equal(MockData.manufacturer.Name, mock.Object.GetManufacturerNameString(MockData.item.Id));
        }

        [Fact]
        public void GetSpecs_NotNull_and_Description_and_Specification()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetSpecs(1)).Returns(MockData.spec);

            Assert.NotNull(mock.Object.GetSpecs(1));
            Assert.Equal(MockData.spec.Description, mock.Object.GetSpecs(1).Description);
            Assert.Equal(MockData.spec.Specification, mock.Object.GetSpecs(1).Specification);
        }

        [Fact]
        public void GetSpecsByModelName_NotNull_and_Description_and_Specification()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetSpecsByName(MockData.model.Name)).Returns(MockData.spec);

            Assert.NotNull(mock.Object.GetSpecsByName(MockData.model.Name));
            Assert.Equal(MockData.spec.Description , mock.Object.GetSpecsByName(MockData.model.Name).Description);
            Assert.Equal(MockData.spec.Specification , mock.Object.GetSpecsByName(MockData.model.Name).Specification);
        }

        [Fact]
        public void GetType_NotNull_and_Name_and_HeaderImageUrl()
        {
            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetType(1)).Returns(MockData.itemType);

            Assert.NotNull(mock.Object.GetType(1));
            Assert.Equal(MockData.itemType.Name , mock.Object.GetType(1).Name);
            Assert.Equal(MockData.itemType.ItemTypeHeaderImageUrl, mock.Object.GetType(1).ItemTypeHeaderImageUrl);
        }

        [Fact]
        public void GetReviews_NotNull_and_Text_and_Rating()
        {
            List<int> reviewIds = new List<int> { 1, 2 };
            List<Reviews> reviewsReturn = new List<Reviews> { MockData.review };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetReviews(1)).Returns(reviewsReturn);

            Assert.NotEmpty(mock.Object.GetReviews(1));
            Assert.Equal(MockData.review.Text, mock.Object.GetReviews(1).First().Text);
            Assert.Equal(5.0, mock.Object.GetReviews(1).First().Rating);

        }
        
        [Fact]
        public void GetReviewsByModelName_NotNull_and_Text_and_Rating()
        {
            List<Reviews> reviewsReturn = new List<Reviews> { MockData.review };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetReveiwsByModelName(MockData.model.Name)).Returns(reviewsReturn);

            Assert.NotEmpty(mock.Object.GetReveiwsByModelName(MockData.model.Name));
            Assert.Equal(MockData.review.Text, mock.Object.GetReveiwsByModelName(MockData.model.Name).First().Text);
            Assert.Equal(5.0, mock.Object.GetReveiwsByModelName(MockData.model.Name).First().Rating);

        }

        [Fact]
        public void GetAllManufacturers_Not_Null()
        {
            List<Manufacturer> manufReturn = new List<Manufacturer> { MockData.manufacturer };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetAllManufacturers()).Returns(manufReturn);

            Assert.NotEmpty(mock.Object.GetAllManufacturers());
            Assert.Equal(MockData.manufacturer.Name, mock.Object.GetAllManufacturers().First().Name);

        }
        

        [Fact]
        public void GetAllItemTypes_Not_Null()
        {
            List<ItemType> itemTypesReturn = new List<ItemType> { MockData.itemType};

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetAllItemTypes()).Returns(itemTypesReturn);

            Assert.NotEmpty(mock.Object.GetAllItemTypes());
            Assert.Equal(MockData.itemType.Name, mock.Object.GetAllItemTypes().First().Name);

        }

        [Fact]
        public void GetItemGroup_Not_Null()
        {
            List<ItemType> itemTypesReturn = new List<ItemType> {  };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetItemGroup(MockData.item.Id)).Returns(MockData.itemType.ItemGroupName);

            Assert.NotNull(mock.Object.GetItemGroup(MockData.item.Id));
            Assert.Equal(MockData.itemType.ItemGroupName, mock.Object.GetItemGroup(MockData.item.Id));

        }

        [Fact]
        public void GetAllItemDepartments_Not_Null()
        {
            List<ItemDepartment> itemDeptReturn = new List<ItemDepartment> { MockData.itemDept};

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetAllItemDepartments()).Returns(itemDeptReturn);

            Assert.NotEmpty(mock.Object.GetAllItemDepartments());
            Assert.Equal(MockData.itemDept.DeptName, mock.Object.GetAllItemDepartments().First().DeptName);

        }

        [Fact]
        public void GetAllModels_Not_Null()
        {
            List<Model> itemDeptReturn = new List<Model> { MockData.model};

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetAllModels()).Returns(itemDeptReturn);

            Assert.NotEmpty(mock.Object.GetAllModels());
            Assert.Equal(MockData.model.Name, mock.Object.GetAllModels().First().Name);

        }

        [Fact]
        public void GetItemSubTypes_NotNull()
        {
            List<ItemTypeSub> itemSubTypeReturn = new List<ItemTypeSub> { MockData.itemSubType };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetItemSubTypes(MockData.itemSubType.SubTypeName)).Returns(itemSubTypeReturn);

            Assert.NotEmpty(mock.Object.GetItemSubTypes(MockData.itemSubType.SubTypeName));
            Assert.Equal(MockData.itemSubType.SubTypeName, mock.Object.GetItemSubTypes(MockData.itemSubType.SubTypeName).First().SubTypeName);
        }

        [Fact]
        public void GetSubCategory_NotNull()
        {
            List<ItemTypeSub> itemSubTypeReturn = new List<ItemTypeSub> { MockData.itemSubType };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetSubCategory(MockData.item.Id)).Returns(MockData.itemSubType.SubTypeName);

            Assert.NotNull(mock.Object.GetSubCategory(MockData.item.Id));
            Assert.Equal(MockData.itemSubType.SubTypeName, mock.Object.GetSubCategory(MockData.item.Id));
        }

        [Fact]
        public void GetModelsByItemType_NotNull()
        {
            List<Model> modelReturn = new List<Model> { MockData.model };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetModelsByItemType(MockData.itemType.Name)).Returns(modelReturn);

            Assert.NotEmpty(mock.Object.GetModelsByItemType(MockData.itemType.Name));
            Assert.Equal(MockData.model.Name, mock.Object.GetModelsByItemType(MockData.itemType.Name).First().Name);

        }

        [Fact]
        public void GetImageUrls_NotNull_()
        {
            List<ImageUrls> imageUrlsList = new List<ImageUrls> { MockData.imageUrl };

            var mock = new Mock<IProductService>();
            mock.Setup(m => m.GetImageUrls(1)).Returns(imageUrlsList);

            Assert.NotEmpty(mock.Object.GetImageUrls(1));
            Assert.Equal(MockData.imageUrl.Url, mock.Object.GetImageUrls(1).First().Url);

        }

    }
}
