using AutoMapper;
using DaDataService.Shared.Models;

namespace DaDataService.BLL.Mappers
{
    /// <summary>
    /// Profile for mapping response data into the view model
    /// </summary>
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressResponseModel, AddressResponseViewModel>()
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.city))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.street))
                .ForMember(dest => dest.HouseNumber, opt => opt.MapFrom(src => src.house))
                .ForMember(dest => dest.ApartmentNumber, opt => opt.MapFrom(src => src.flat));
        }
    }
}