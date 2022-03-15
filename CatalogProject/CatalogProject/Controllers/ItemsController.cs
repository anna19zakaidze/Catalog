using CatalogProject.Dtos;
using CatalogProject.Entities;
using CatalogProject.Helpers;
using CatalogProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CatalogProject.Controllers
{
    [ApiController]
    [Route("[controller]")] //GET/items
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _repository;
        public ItemsController(IItemsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        //[Route("GetItems")]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await _repository.GetItems()).Select(item=> item.AsDto());
            return items;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id) 
        { 
            var item = await _repository.GetItemAsync(id);

            if(item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            Item createItem = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

           await _repository.CreateItem(createItem);

            return CreatedAtAction(nameof(GetItemAsync), new { id = createItem.Id }, createItem.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem =await _repository.GetItemAsync(id);

            if(existingItem == null)
            {
                return NotFound(id);
            }

            Item updateItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            await _repository.UpdateItem(updateItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem =await _repository.GetItemAsync(id);

            if (existingItem == null)
            {
                return NotFound(id);
            }

            await _repository.DeleteItem(id);

            return NoContent();
        }
    }
}
