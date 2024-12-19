using AutoMapper;
using DaDataService.Shared.Models;

namespace DaDataService.BLL.Mappers
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressRequestInputModel, AddressResponseViewModel>();
        }
    }
}