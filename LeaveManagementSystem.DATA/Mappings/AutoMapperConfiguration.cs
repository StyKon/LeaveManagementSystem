using AutoMapper;

namespace LeaveManagementSystem.DATA.Mappings
{
    public class AutoMapperConfiguration
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<EntityToDtoMapping>();
            });

            IMapper mapper = config.CreateMapper();

            return mapper;
        }
    }
}
