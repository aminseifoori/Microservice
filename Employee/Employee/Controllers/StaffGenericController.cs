using AutoMapper;
using Common;
using Contracts;
using Employee.Dtos;
using Employee.Model;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffGenericController : Controller
    {
        private readonly IMapper mapper;
        private readonly IRepository<Staff> repository;
        private readonly IPublishEndpoint publishEndpoint;

        public StaffGenericController(IMapper _mapper, IRepository<Staff> _repository,
            IPublishEndpoint _publishEndpoint)
        {
            mapper = _mapper;
            repository = _repository;
            publishEndpoint = _publishEndpoint;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var staff = await repository.GetAllAsync();
            var staffDto = mapper.Map<List<StaffDto>>(staff);
            return Ok(staffDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var staff = await repository.GetByIdAsync(id);
            var staffDto = mapper.Map<StaffDto>(staff);
            return Ok(staffDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStaffDto staffDto)
        {
            var staff = mapper.Map<Staff>(staffDto);
            staff.CreatedDate = DateTime.Now;
            await repository.CreateAsync(staff);

            var staffCreated = mapper.Map<StaffCreated>(staff);

            await publishEndpoint.Publish(staffCreated);

            return CreatedAtAction(nameof(GetById), new { id = staff.Id }, staff);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateStaffDto updateStaffDto)
        {
            var staff = await repository.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            mapper.Map(updateStaffDto, staff);
            await repository.UpdateAsync(staff);

            await publishEndpoint.Publish(mapper.Map<StaffUpdated>(staff));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var staff = await repository.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            await repository.RemoveAsync(id);

            await publishEndpoint.Publish(mapper.Map<StaffDeleted>(staff));

            return NoContent();
        }
    }
}
