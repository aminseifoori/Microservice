using AutoMapper;
using Common;
using Inventory.Dtos;
using Inventory.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRepository<AssignedInventory> repository;

        public InventoryController(IMapper _mapper,IRepository<AssignedInventory> _repository)
        {
            mapper = _mapper;
            repository = _repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var inventorylist = await repository.GetAllAsync();
            var inventoryListDto = mapper.Map<List<AssignedAssetToStaffDto>>(inventorylist);
            return Ok(inventoryListDto);
        }

        [HttpGet("{staffId}")]
        public async Task<IActionResult> GetAllStaffInventoryAsync(Guid staffId)
        {
            var inventorylist = await repository.GetAllAsync(x=> x.StaffId == staffId);
            var inventoryListDto = mapper.Map<List<AssignedAssetToStaffDto>>(inventorylist);
            return Ok(inventoryListDto);
        }

        [HttpGet("{assetId}")]
        public async Task<IActionResult> GetAssignedAssetAsync(Guid assetId)
        {
            var inventory = await repository.GetByIdAsync(x => x.AssetId == assetId);
            var inventoryDto = mapper.Map<AssignedAssetToStaffDto>(inventory);
            return Ok(inventoryDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var inventory = await repository.GetByIdAsync(id);
            var inventoryDto = mapper.Map<AssignedAssetToStaffDto>(inventory);
            return Ok(inventoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> AssignAssetToStaff(AssignAssetToStaffDto assignment)
        {
            var exitingAssignment = await repository.
                GetByIdAsync(x=> x.StaffId == assignment.StaffId && x.AssetId == assignment.AssetId); 
            if(exitingAssignment == null) 
            {
                AssignedInventory assignedInventory = mapper.Map<AssignedInventory>(assignment);
                await repository.CreateAsync(assignedInventory);
                return CreatedAtAction(nameof(GetById), new { id = assignedInventory.Id }, assignedInventory);
            }
            return BadRequest();

        }
    }
}
