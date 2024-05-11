using AutoMapper;
using Backend.Dtos;
using Backend.Models;

namespace Backend.Helpers
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<Admin, AdminDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderProduct, OrderProductDto>();
            CreateMap<Address, AddressDto>();
        }
    }
}