using AutoMapper;
using Contracts;
using Employee.Dtos;
using Employee.Model;

namespace Employee
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Staff, StaffDto>().ReverseMap();
            CreateMap<Staff, CreateStaffDto>().ReverseMap();
            CreateMap<Staff, UpdateStaffDto>().ReverseMap();
            CreateMap<Staff, StaffCreated>()
                .ForMember(x => x.EmployeeId, option => option.MapFrom(o => o.Id));
            CreateMap<Staff, StaffUpdated>()
                .ForMember(x => x.EmployeeId, option => option.MapFrom(o => o.Id));
            CreateMap<Staff, StaffDeleted>()
                .ForMember(x => x.EmployeeId, option => option.MapFrom(o => o.Id));

        }
    }
}
