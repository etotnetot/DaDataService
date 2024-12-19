using DaDataService.Shared.Models;
using Microsoft.Extensions.Options;

namespace DaDataService.BLL.Services
{
    public class AddressStandardizationService : IAddressStandardizationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public AddressStandardizationService(HttpClient httpClient, IOptions<DaDataServiceOptions> options)
        {
            _apiKey = options.Value.ApiKey;
        }

        public async Task<AddressResponseViewModel> GetAddressStandardization(AddressRequestInputModel addressRequestInputModel)
        {

        }
    }
}