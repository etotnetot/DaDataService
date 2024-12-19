namespace DaDataService.Shared.Models
{
    public class AddressStandardizationHttpClient
    {
        public HttpClient Client { get; }

        public AddressStandardizationHttpClient(HttpClient client)
        {
            Client = client;
        }
    }
}