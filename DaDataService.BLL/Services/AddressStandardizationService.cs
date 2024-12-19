using AutoMapper;
using DaDataService.Shared.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace DaDataService.BLL.Services
{
    public class AddressStandardizationService : IAddressStandardizationService
    {
        private readonly HttpClient _httpClient;

        private readonly IOptions<DaDataServiceOptions> _daDataServiceOptions;

        private readonly IMapper _addressMapper;

        public AddressStandardizationService(HttpClient httpClient, IOptions<DaDataServiceOptions> daDataServiceOptions, IMapper addressMapper)
        {
            _httpClient = httpClient;
            _daDataServiceOptions = daDataServiceOptions;
            _addressMapper = addressMapper;
        }

        /// <summary>
        /// Retrieves standardized address.
        /// </summary>
        /// <param name="addressRequestInputModel">Address to standardize.</param>
        /// <returns>Standardized address, which contains such information as country, city, street, house and flat number.</returns>
        public async Task<AddressResponseViewModel> GetAddressStandardization(AddressRequestInputModel addressRequestInputModel)
        {
            var apiKey = _daDataServiceOptions.Value.ApiKey; 
            var apiSecret = _daDataServiceOptions.Value.SecretKey;
            var baseUrl = _daDataServiceOptions.Value.BaseUrl;

            var content = new StringContent(
                JsonConvert.SerializeObject(new string[] { addressRequestInputModel.InputAddress }),
                Encoding.UTF8,
                "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {apiKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Secret", apiSecret);

            var response = await _httpClient.PostAsync(baseUrl, content);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var deserializedResponseData = JsonConvert.DeserializeObject<AddressResponseModel[]>(responseData);

            return _addressMapper.Map<AddressResponseViewModel>(deserializedResponseData[0]);
        }
    }
}