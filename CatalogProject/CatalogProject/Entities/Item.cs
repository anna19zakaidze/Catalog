namespace CatalogProject.Entities
{
    public record Item
    {
        public Guid Id { get; init; } //afrter creation you can not modify this property
        public string Name { get; init; }
        public decimal Price { get; init; }
        public DateTimeOffset CreatedDate { get; init; }
    }
}
