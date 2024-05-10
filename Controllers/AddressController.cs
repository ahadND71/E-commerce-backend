using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api.Services;

namespace api.Controllers;


[ApiController]
[Route("/api/address")]
public class AddressController : ControllerBase
{
    private readonly AddressService _addressService;
    public AddressController(AddressService addressService)
    {
        _addressService = addressService;
    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllAddress([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
    {
        var addresses = await _addressService.GetAllAddressService(currentPage, pageSize);
        if (addresses.TotalCount < 1)
        {
            return ApiResponse.NotFound("No Addresses To Display");
        }

        return ApiResponse.Success<IEnumerable<Address>>(
            addresses.Items,
            "Addresses are returned successfully");
    }



    [Authorize]
    [HttpGet("{addressId}")]
    public async Task<IActionResult> GetAddress(string addressId)
    {
        if (!Guid.TryParse(addressId, out Guid addressIdGuid))
        {
            return ApiResponse.BadRequest("Invalid address ID Format");
        }
        var address = await _addressService.GetAddressById(addressIdGuid);
        if (address == null)

        {
            return ApiResponse.NotFound(
             $"No Address Found With ID : ({addressIdGuid})");
        }
        else
        {
            return ApiResponse.Success<Address>(
              address,
              "Address is returned successfully"
            );
        }
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAddress(Address newAddress)
    {
        var createdAddress = await _addressService.CreateAddressService(newAddress);

        if (createdAddress != null)
        {
            return ApiResponse.Created<Address>(createdAddress, "Address is created successfully");
        }
        else
        {
            return ApiResponse.ServerError("Error when creating new address");

        }
    }


    [Authorize]
    [HttpPut("{addressId}")]
    public async Task<IActionResult> UpdateAddress(string addressId, Address updateAddress)
    {
        if (!Guid.TryParse(addressId, out Guid addressIdGuid))
        {
            return ApiResponse.BadRequest("Invalid Address ID Format");
        }
        var address = await _addressService.UpdateAddressService(addressIdGuid, updateAddress);
        if (address == null)
        {
            return ApiResponse.NotFound("No Address Founded To Update");
        }
        return ApiResponse.Success<Address>(
            address,
            "Address Is Updated Successfully"
        );
    }


    [Authorize]
    [HttpDelete("{addressId}")]
    public async Task<IActionResult> DeleteAddress(string addressId)
    {
        if (!Guid.TryParse(addressId, out Guid addressIdGuid))
        {
            return ApiResponse.BadRequest("Invalid address ID Format");
        }
        var result = await _addressService.DeleteAddressService(addressIdGuid);
        if (!result)
        {
            return ApiResponse.NotFound("The Address is not found to be deleted");
        }
        return ApiResponse.Success(" Address is deleted successfully");
    }
}
