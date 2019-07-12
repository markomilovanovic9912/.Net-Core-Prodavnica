using StoreData.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace StoreData
{
    public interface IProductService
    {

        IEnumerable<Items> GetAll();
        IEnumerable<Items> GetItemsByIds(List<int> itemId);
        IEnumerable<Items> GetItemsRefreshFilter(/*List<int> itemId,*/ string name, IEnumerable<string> manufacturer, IEnumerable<string> subCat);
        IEnumerable<Items> GetByType(string name);
        IEnumerable<Items> GetByHistoryId(List<int> itemId);
        IEnumerable<Items> GetRelatedItems(string itemSubType);
        IEnumerable<Items> GetItemsBySubType(string itemSubType);
        IEnumerable<ImageUrls> GetImageUrls(int id);
        IEnumerable<Reviews> GetReviews(int id);
        IEnumerable<Manufacturer> GetAllManufacturers();
        IEnumerable<ItemType> GetAllItemTypes();
        IEnumerable<ItemDepartment> GetAllItemDepartments();
        IEnumerable<Model> GetAllModels();
        IEnumerable<Reviews> GetReveiwsByModelName(string modelName);
        IEnumerable<ItemTypeSub> GetItemSubTypes(string itemType);
        IEnumerable<Model> GetModelsByItemType(string itemType);
        IEnumerable<Items> SearchProducts(string prodSearch);
        List<int> GetHistoryByUserId(int id);

        Task<UserItemHistory> AddItemHistory(int id, int userId);
        Task<Items> AddItem(Items item, Specs specs,Model model);
        Task<Items> RemoveItem(string modelName);
        Task<Items> UpdateItem(Items item, Specs specs, Model model);
        Task<Specs> AddSpecs(Specs specs);
        Task<Model> AddModel(Model model);
        Task<ItemType> AddItemType(ItemType itemType);
        Task<ItemType> RemoveItemType(int Id);
        Task<ItemTypeSub> AddItemSubType(ItemTypeSub itemSubType);
        Task<ItemTypeSub> RemoveItemSubType(int Id);
        Task<ItemDepartment> AddItemDepartment(ItemDepartment itemDepartment);
        Task<Manufacturer> AddManufacturer(Manufacturer manufacturer);
        Task<Manufacturer> RemoveManufacturer(int Id);
        Task<Reviews> RemoveReview(int id);
        Task<Reviews> AddReview(Reviews review);


        Items GetItemByModelName(string modelName);
        Items GetById(int id);
        Model GetModelName(int id);
        Specs GetSpecsByName(string modelName);
        Specs GetSpecs(int id);
        ItemType GetType(int id);
        Manufacturer GetProductName(int id);
        Manufacturer GetManufacturerByName(string name);

        string GetItemTypeHeaderImg(string itemGroup);
        string GetFirstImage(int id);
        int GetAvailability(int id);
        double GetPrice(int id);
        double GetAvrageRating(int id);
        string GetModelNameString(int id);
        string GetManufacturerNameString(int id);
        string GetSubCategory(int id);
        string GetItemGroup(int id);
        IEnumerable<Reviews> GetAllReviews();
        IEnumerable<SliderImageUrls> UpdateSliderImages(string imageUrl, int imageId);
        IEnumerable<SliderImageUrls> GetAllSliderImages();
        string DecreaseNumberOfItems(IEnumerable<Items> items);
 
    }
}
