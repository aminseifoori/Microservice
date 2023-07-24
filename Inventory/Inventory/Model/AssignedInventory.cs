using Common;

namespace Inventory.Model
{
    public class AssignedInventory : IEntity
    {
        public Guid Id { get; set; }
        public Guid StaffId { get; set; }
        public Guid AssetId { get; set; }
        public DateTimeOffset AssignDate { get; set; }
    }
}
