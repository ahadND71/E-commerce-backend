using AutoMapper;

namespace api.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<Admin, AdminDto>();

        }
    }
}