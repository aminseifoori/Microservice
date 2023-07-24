using AutoMapper;
using Inventory.Dtos;
using Inventory.Model;

namespace Inventory
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AssignAssetToStaffDto, AssignedInventory>().ReverseMap();
            CreateMap<AssignedAssetToStaffDto, AssignedInventory>().ReverseMap();
        }
    }
}
