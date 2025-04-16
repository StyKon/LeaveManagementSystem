using AutoMapper;
using LeaveManagementSystem.DATA.Dto;
using LeaveManagementSystem.DOMAINE.Entities;

namespace LeaveManagementSystem.DATA.Mappings
{
    public class EntityToDtoMapping : Profile
    {
        public EntityToDtoMapping()
        {
            CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap();
        }

    }
}
