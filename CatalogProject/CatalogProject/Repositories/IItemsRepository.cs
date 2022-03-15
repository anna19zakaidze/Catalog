using CatalogProject.Entities;

namespace CatalogProject.Repositories
{
    public interface IItemsRepository
    {
        Task<Item> GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItems();
        Task CreateItem(Item item); //void CreateItem(Item item);
        Task UpdateItem(Item item);
        Task DeleteItem(Guid id);
    }
}
