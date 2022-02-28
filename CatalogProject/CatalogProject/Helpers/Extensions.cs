using CatalogProject.Dtos;
using CatalogProject.Entities;

namespace CatalogProject.Helpers
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            var itemDto = new ItemDto() { 
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                CreatedDate = item.CreatedDate
            };
            return itemDto;
        }
    }
}
