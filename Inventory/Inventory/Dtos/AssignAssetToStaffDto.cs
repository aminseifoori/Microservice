namespace Inventory.Dtos
{
    public record AssignAssetToStaffDto
    {
        public Guid StaffId { get; set; }
        public Guid AssetId { get; set; }
        public DateTimeOffset AssignDate { get; set; }
    }
}
