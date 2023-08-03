namespace Inventory.Dtos
{
    public record StaffDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
