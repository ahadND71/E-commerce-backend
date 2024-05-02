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


    [HttpGet]
    public async Task<IActionResult> GetAllAddress()
    {
        try
        {
            var addresses = await _dbContext.GetAllAddressService();
            if (addresses.ToList().Count < 1)
            {
                return NotFound(new ErrorMessage
                {
                    Message = "No Addresses To Display"
                });
            }
            return Ok(new SuccessMessage<IEnumerable<Address>>
            {
                Message = "Addresses are returned succeefully",
                Data = addresses
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not return the Address list");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [HttpGet("{addressId}")]
    public async Task<IActionResult> GetAddress(string addressId)
    {
        try
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid))
            {
                return BadRequest("Invalid address ID Format");
            }
            var address = await _dbContext.GetAddressById(addressIdGuid);
            if (address == null)

            {
                return NotFound(new ErrorMessage
                {
                    Message = $"No Address Found With ID : ({addressIdGuid})"
                });
            }
            else
            {
                return Ok(new SuccessMessage<Address>
                {
                    Message = "Address is returned succeefully",
                    Data = address
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not return the Address");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });

        }
    }


    [HttpPost]
    public async Task<IActionResult> CreateAddress(Address newAddress)
    {
        try
        {
            var createdAddress = await _dbContext.CreateAddressService(newAddress);

            if (createdAddress != null)
            {
                return CreatedAtAction(nameof(GetAddress), new { addressId = createdAddress.AddressId }, createdAddress);
            }
            return Ok(new SuccessMessage<Address>
            {
                Message = "Address is created succeefully",
                Data = createdAddress
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not create new Address");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }


    }


    [HttpPut("{addressId}")]
    public async Task<IActionResult> UpdateAddress(string addressId, Address updateAddress)
    {
        try
        {


            if (!Guid.TryParse(addressId, out Guid addressIdGuid))
            {
                return BadRequest("Invalid Address ID Format");
            }
            var address = await _dbContext.UpdateAddressService(addressIdGuid, updateAddress);
            if (address == null)
            {
                return NotFound(new ErrorMessage
                {
                    Message = "No Address To Founed To Update"
                });
            }
            return Ok(new SuccessMessage<Address>
            {
                Message = "Address Is Updated Succeefully",
                Data = address
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not create new Address");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [HttpDelete("{addressId}")]
    public async Task<IActionResult> DeleteAddress(string addressId)
    {
        try
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid))
            {
                return BadRequest("Invalid address ID Format");
            }
            var result = await _dbContext.DeleteAddressService(addressIdGuid);
            if (!result)


            {
                return NotFound(new ErrorMessage
                {
                    Message = "The Address is not found to be deleted"
                });
            }
            return Ok(new { success = true, message = " Address is deleted succeefully" });
        }

        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , the Address can not deleted");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }
}
