namespace Inventory.Dtos
{
    public record AssignedAssetToStaffDto
    {
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
        public string StaffDescription { get; set; }
        public Guid AssetId { get; set; }
        public DateTimeOffset AssignDate { get; set; }
    }
}
