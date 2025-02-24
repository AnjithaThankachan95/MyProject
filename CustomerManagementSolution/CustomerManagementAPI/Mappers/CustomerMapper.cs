using AutoMapper;
using CustomerManagement.API.Models;
using CustomerManagement.Domain.Entities;

namespace CustomerManagement.API.Mappers
{
    public class CustomerMapper : Profile
    {
        public CustomerMapper() {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
        }        
    }
}
