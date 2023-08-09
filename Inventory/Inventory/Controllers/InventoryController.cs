using AutoMapper;
using Common;
using Inventory.Clients;
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
        private readonly EmployeeClient employeeClient;

        public InventoryController(IMapper _mapper,IRepository<AssignedInventory> _repository, EmployeeClient _employeeClient)
        {
            mapper = _mapper;
            repository = _repository;
            employeeClient = _employeeClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var inventorylist = await repository.GetAllAsync();
            var stafflist = await employeeClient.GetAllStaff();
            var joinedInventoryStaff = inventorylist
                .Join(stafflist, i => i.StaffId, s => s.Id, (i, s) => new { i, s })
                .Select(s => new AssignedAssetToStaffDto
                {
                    AssetId = s.i.AssetId,
                    StaffId = s.i.StaffId,
                    StaffName = s.s.Name,
                    StaffDescription = s.s.Description
                });

            return Ok(joinedInventoryStaff);
        }

        [HttpGet("GetAllStaffInventory/{staffId}")]
        public async Task<IActionResult> GetAllStaffInventoryAsync(Guid staffId)
        {
            var inventorylist = await repository.GetAllAsync(x=> x.StaffId == staffId);

            var stafflist = await employeeClient.GetAllStaff();
            var joinedInventoryStaff = inventorylist
                .Join(stafflist, i => i.StaffId, s => s.Id, (i, s) => new { i, s })
                .Select(s => new AssignedAssetToStaffDto
                {
                    AssetId = s.i.AssetId,
                    StaffId = s.i.StaffId,
                    StaffName = s.s.Name,
                    StaffDescription = s.s.Description
                });

            return Ok(joinedInventoryStaff);
        }

        [HttpGet("GetAssignedAsset/{assetId}")]
        public async Task<IActionResult> GetAssignedAssetAsync(Guid assetId)
        {
            var inventory = await repository.GetByIdAsync(x => x.AssetId == assetId);
            var inventoryDto = mapper.Map<AssignedAssetToStaffDto>(inventory);
            var staff = await employeeClient.GetStaffInformationAsync(inventoryDto.StaffId) ;
            inventoryDto.StaffName = staff.Name;
            inventoryDto.StaffDescription = staff.Description;
            return Ok(inventoryDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var inventory = await repository.GetByIdAsync(id);
            var inventoryDto = mapper.Map<AssignedAssetToStaffDto>(inventory);
            var staff = await employeeClient.GetStaffInformationAsync(inventoryDto.StaffId);
            inventoryDto.StaffName = staff.Name;
            inventoryDto.StaffDescription = staff.Description;   
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
