using DaDataService.Shared.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace DaDataService.BLL.Services
{
    public class AddressStandardizationService : IAddressStandardizationService
    {
        private readonly HttpClient _httpClient;

        private readonly IOptions<DaDataServiceOptions> _daDataServiceOptions;

        public AddressStandardizationService(HttpClient httpClient, IOptions<DaDataServiceOptions> daDataServiceOptions)
        {
            _httpClient = httpClient;
            _daDataServiceOptions = daDataServiceOptions;
        }

        public async Task<AddressResponseViewModel> GetAddressStandardization(AddressRequestInputModel addressRequestInputModel)
        {
            var apiKey = _daDataServiceOptions.Value.ApiKey; 
            var apiSecret = _daDataServiceOptions.Value.SecretKey;
            var baseUrl = _daDataServiceOptions.Value.BaseUrl;

            var address = new string[] { addressRequestInputModel.InputRawAddress };
            var content = new StringContent(
                JsonConvert.SerializeObject(address),
                Encoding.UTF8,
                "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Token {apiKey}");
            _httpClient.DefaultRequestHeaders.Add("X-Secret", apiSecret);

            var response = await _httpClient.PostAsync(baseUrl, content);
            response.EnsureSuccessStatusCode();

            var responseData = await response.Content.ReadAsStringAsync();
            var parsedResponse = JArray.Parse(responseData);
            var standardizedData = parsedResponse[0];

            return new AddressResponseViewModel
            {
                Country = standardizedData["country"]?.ToString(),
                City = standardizedData["city"]?.ToString(),
                Street = standardizedData["street"]?.ToString(),
                House = standardizedData["house"]?.ToString(),
                Flat = standardizedData["flat"]?.ToString()
            };
        }
    }
}