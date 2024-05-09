using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api.Services;
using api.Helpers;

namespace api.Controllers;


[ApiController]
[Route("/api/address")]
public class AddressController : ControllerBase
{
    private readonly AddressService _dbContext;
    public AddressController(AddressService addressService)
    {
        _dbContext = addressService;
    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllAddress([FromQuery] int currentPage = 1, [FromQuery] int pageSize = 3)
    {
        try
        {
            var addresses = await _dbContext.GetAllAddressService(currentPage , pageSize);
            if (addresses.TotalCount < 1)
            {
                return ApiResponse.NotFound("No Addresses To Display");
            }
            return ApiResponse.Success<IEnumerable<Address>>(
                addresses.Items,
                "Addresses are returned successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Address list");
            return ApiResponse.ServerError(ex.Message);
        }
    }



    [Authorize]
    [HttpGet("{addressId}")]
    public async Task<IActionResult> GetAddress(string addressId)
    {
        try
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid))
            {
                return ApiResponse.BadRequest("Invalid address ID Format");
            }
            var address = await _dbContext.GetAddressById(addressIdGuid);
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
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Address");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAddress(Address newAddress)
    {
        try
        {
            var createdAddress = await _dbContext.CreateAddressService(newAddress);

            if (createdAddress != null)
            {
                return ApiResponse.Created<Address>(createdAddress, "Address is created successfully");
            }
            else
            {
                return ApiResponse.ServerError("Error when creating new address");

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot create new Address");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [Authorize]
    [HttpPut("{addressId}")]
    public async Task<IActionResult> UpdateAddress(string addressId, Address updateAddress)
    {
        try
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid))
            {
                return ApiResponse.BadRequest("Invalid Address ID Format");
            }
            var address = await _dbContext.UpdateAddressService(addressIdGuid, updateAddress);
            if (address == null)
            {
                return ApiResponse.NotFound("No Address Founded To Update");
            }
            return ApiResponse.Success<Address>(
                address,
                "Address Is Updated Successfully"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot create new Address");
            return ApiResponse.ServerError(ex.Message);

        }
    }


    [Authorize]
    [HttpDelete("{addressId}")]
    public async Task<IActionResult> DeleteAddress(string addressId)
    {
        try
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid))
            {
                return ApiResponse.BadRequest("Invalid address ID Format");
            }
            var result = await _dbContext.DeleteAddressService(addressIdGuid);
            if (!result)
            {
                return ApiResponse.NotFound("The Address is not found to be deleted");
            }
            return ApiResponse.Success(" Address is deleted successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, the Address cannot deleted");
            return ApiResponse.ServerError(ex.Message);

        }
    }
}
