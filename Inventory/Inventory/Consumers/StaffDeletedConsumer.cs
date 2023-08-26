using Common;
using Contracts;
using Inventory.Model;
using MassTransit;

namespace Inventory.Consumers
{
    public class StaffDeletedConsumer : IConsumer<StaffCreated>
    {
        private readonly IRepository<StaffEntity> repository;

        public StaffDeletedConsumer(IRepository<StaffEntity> _repository)
        {
            repository = _repository;
        }
        public async Task Consume(ConsumeContext<StaffCreated> context)
        {
            var message = context.Message;
            var item = await repository.GetByIdAsync(message.EmployeeId);

            if (item == null)
            {
                return;
            }
            else
            {

                await repository.RemoveAsync(item.Id);
            }

        }
    }
}
