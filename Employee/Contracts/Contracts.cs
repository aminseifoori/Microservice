using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public record StaffCreated
    {
        public Guid EmployeeId { get; init; }
        public string Name { get; init; }
        public string  Description { get; init; }

    }
    public record StaffUpdated
    {
        public Guid EmployeeId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }

    }
    public record StaffDeleted
    {
        public Guid EmployeeId { get; init; }
    }
}
