using AutoMapper;
using LeaveManagementSystem.DATA.Common;
using LeaveManagementSystem.DATA.Dto;
using LeaveManagementSystem.DATA.Helpers;
using LeaveManagementSystem.DOMAINE.Entities;

namespace LeaveManagementSystem.DATA.Mappings
{
    public class EntityToDtoMapping : Profile
    {
        public EntityToDtoMapping()
        {
            CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap();

            CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
                .ConvertUsing(typeof(PagedResultConverter<,>));
        }

    }
}
