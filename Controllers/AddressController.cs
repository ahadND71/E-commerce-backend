
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("/api/address")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;
        public AddressController()
        {
            _addressService = new AddressService();
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var address = _addressService.GetAllAddressService();
            return Ok(address);
        }

        [HttpGet("{addressId}")]
        public IActionResult GetAddress(string addressId)
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid))
            {
                return BadRequest("Invalid address ID Format");
            }
            var address = _addressService.GetAddressById(addressIdGuid);
            if (address == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(address);
            }

        }

        [HttpPost]
        public IActionResult CreateAddress(Address newAddress)
        {
            var createdAddress = _addressService.CreateAddressService(newAddress);
            return CreatedAtAction(nameof(GetAddress), new { addressId = createdAddress.AddressId }, createdAddress);
        }


        [HttpPut("{addressId}")]
        public IActionResult UpdateAddress(string addressId, Address updateAddress)
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid))
            {
                return BadRequest("Invalid address ID Format");
            }
            var address = _addressService.UpdateAddressService(addressIdGuid, updateAddress);
            if (address == null)
            {
                return NotFound();
            }
            return Ok(address);
        }


        [HttpDelete("{addressId}")]
        public IActionResult DeleteAddress(string addressId)
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid))
            {
                return BadRequest("Invalid address ID Format");
            }
            var result = _addressService.DeleteAddressService(addressIdGuid);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}