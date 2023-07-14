using AutoMapper;
using Employee.Dtos;
using Employee.Model;
using Employee.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Employee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly StaffRepository repository = new();
        private readonly IMapper mapper;

        public StaffController(IMapper _mapper)
        {
            mapper = _mapper;
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
            await repository.CreateAsync(staff);
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
            return NoContent(); 
        }
    }
}
