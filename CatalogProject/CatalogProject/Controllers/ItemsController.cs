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
        private readonly IInMemoryItemsRepository _repository;
        public ItemsController(IInMemoryItemsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetItems")]
        public IEnumerable<ItemDto> GetItems()
        {
            var items = _repository.GetItems().Select(item=> item.AsDto());
            return items;
        }
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetItem(Guid id) 
        { 
            var item = _repository.GetItem(id);

            if(item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
        {
            Item createItem = new()
            {
                Id = Guid.NewGuid(),
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            _repository.CreateItem(createItem);

            return CreatedAtAction(nameof(GetItem), new { id = createItem.Id }, createItem.AsDto());
        }

        [HttpPut("{id}")]
        public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = _repository.GetItem(id);

            if(existingItem == null)
            {
                return NotFound(id);
            }

            Item updateItem = existingItem with
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };

            _repository.UpdateItem(updateItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(Guid id)
        {
            var existingItem = _repository.GetItem(id);

            if (existingItem == null)
            {
                return NotFound(id);
            }

            _repository.DeleteItem(id);

            return NoContent();
        }
    }
}
