using DaDataService.Shared.Models;

namespace DaDataService.BLL.Services
{
    public interface IAddressStandardizationService
    {
        Task<AddressResponseViewModel> GetAddressStandardization(AddressRequestInputModel addressRequestInputModel);
    }
}