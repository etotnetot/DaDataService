using DaDataService.BLL.Services;
using DaDataService.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace DaDataService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressStandardizationService _addressStandardizationService;

        public AddressController(IAddressStandardizationService addressStandardizationService)
        {
            _addressStandardizationService = addressStandardizationService;
        }

        /// <summary>
        /// Retrieves standardized address.
        /// </summary>
        /// <param name="requestInputModel">Address to standardize.</param>
        /// <returns>Standardized address.</returns>
        [HttpGet("GetAddressStandardization")]
        public async Task<IActionResult> GetAddressStandardization([FromQuery] AddressRequestInputModel requestInputModel) 
        {
            if (string.IsNullOrWhiteSpace(requestInputModel.InputAddress))
                return BadRequest(new { Message = "Address cannot be null or empty" });

            return Ok(await _addressStandardizationService.GetAddressStandardization(requestInputModel));
        }
    }
}