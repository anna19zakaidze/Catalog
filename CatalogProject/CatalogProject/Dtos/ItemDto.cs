namespace CatalogProject.Dtos
{
    public record ItemDto
    {
        public Guid Id { get; init; } //why record ? - afrter creation you can not modify this property
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}
