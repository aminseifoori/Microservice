namespace Inventory.Dtos
{
    public record AssignedAssetToStaffDto
    {
        public Guid StaffId { get; set; }
        public Guid AssetId { get; set; }
        public DateTimeOffset AssignDate { get; set; }
    }
}
