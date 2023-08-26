using Common;

namespace Inventory.Model
{
    public class StaffEntity : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
