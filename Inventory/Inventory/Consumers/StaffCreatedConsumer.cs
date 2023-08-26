using Common;
using Contracts;
using Inventory.Model;
using MassTransit;

namespace Inventory.Consumers
{
    public class StaffCreatedConsumer : IConsumer<StaffCreated>
    {
        private readonly IRepository<StaffEntity> repository;

        public StaffCreatedConsumer(IRepository<StaffEntity> _repository)
        {
            repository = _repository;
        }
        public async Task Consume(ConsumeContext<StaffCreated> context)
        {
            var message = context.Message;
            var item = await repository.GetByIdAsync(message.EmployeeId);

            if (item != null)
            {
                return;
            }

            item = new StaffEntity
            {
                Id = message.EmployeeId,
                Name = message.Name,
                Description = message.Description
            };

            await repository.CreateAsync(item);
        }
    }
}
