using Common;
using Contracts;
using Inventory.Model;
using MassTransit;

namespace Inventory.Consumers
{
    public class StaffUpdatedConsumer : IConsumer<StaffCreated>
    {
        private readonly IRepository<StaffEntity> repository;

        public StaffUpdatedConsumer(IRepository<StaffEntity> _repository)
        {
            repository = _repository;
        }
        public async Task Consume(ConsumeContext<StaffCreated> context)
        {
            var message = context.Message;
            var item = await repository.GetByIdAsync(message.EmployeeId);

            if (item == null)
            {
                item = new StaffEntity
                {
                    Id = message.EmployeeId,
                    Name = message.Name,
                    Description = message.Description
                };

                await repository.CreateAsync(item);
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;

                await repository.UpdateAsync(item);
            }

        }
    }
}
