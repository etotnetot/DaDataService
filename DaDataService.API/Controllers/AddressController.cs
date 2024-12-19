using DaDataService.BLL.Services;
using DaDataService.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DaDataService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressStandardizationService _addressStandardizationService;

        public AddressController(IAddressStandardizationService addressStandardizationService)
        {
            _addressStandardizationService = addressStandardizationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddressStandardization([FromQuery] AddressRequestInputModel requestLink) 
        {
            return await Ok(_addressStandardizationService.GetAddressStandardization(requestLink));
        }
    }
}