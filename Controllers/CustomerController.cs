using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using api.Services;
using api.Helpers;
using api.Authentication.Identity;

namespace api.Controllers;


[Authorize]
[RequiresClaim(IdentityData.AdminUserClaimName, "true")]
[ApiController]
[Route("/api/customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _dbContext;
    public CustomerController(CustomerService customerService)
    {
        _dbContext = customerService;
    }


    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        try
        {
            var customers = await _dbContext.GetAllCustomersService();
            if (customers.ToList().Count < 1)
            {
                return NotFound(new ErrorMessage
                {
                    Message = "No Customers To Display"
                });
            }
            return Ok(new SuccessMessage<IEnumerable<Customer>>
            {
                Message = "Customers are returned successfully",
                Data = customers
            }
                );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Customer list");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [Authorize]
    [HttpGet("{customerId}")]
    public async Task<IActionResult> GetCustomer(string customerId)
    {
        try
        {
            if (!Guid.TryParse(customerId, out Guid customerIdGuid))
            {
                return BadRequest("Invalid customer ID Format");
            }
            var customer = await _dbContext.GetCustomerById(customerIdGuid);
            if (customer == null)
            {
                return NotFound(new ErrorMessage
                {
                    Message = $"No Customer Found With ID : ({customerIdGuid})"
                });
            }
            else
            {
                return Ok(new SuccessMessage<Customer>
                {
                    Success = true,
                    Message = "Customer is returned successfully",
                    Data = customer
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot return the Customer");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [Authorize]
    [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
    [HttpPost]
    public async Task<IActionResult> CreateCustomer(Customer newCustomer)
    {
        try
        {
            var createdCustomer = await _dbContext.CreateCustomerService(newCustomer);
            if (createdCustomer != null)
            {
                return CreatedAtAction(nameof(GetCustomer), new { customerId = createdCustomer.CustomerId }, createdCustomer);
            }
            return Ok(new SuccessMessage<Customer>
            {
                Message = "Customer is created successfully",
                Data = createdCustomer
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot create new Customer");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [Authorize]
    [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
    [HttpPut("{customerId}")]
    public async Task<IActionResult> UpdateCustomer(string customerId, Customer updateCustomer)
    {
        try
        {
            if (!Guid.TryParse(customerId, out Guid customerIdGuid))
            {
                return BadRequest("Invalid customer ID Format");
            }
            var customer = await _dbContext.UpdateCustomerService(customerIdGuid, updateCustomer);
            if (customer == null)
            {
                return NotFound(new ErrorMessage
                {
                    Message = "No Customer To Founded To Update"
                });
            }
            return Ok(new SuccessMessage<Customer>
            {
                Message = "Customer Is Updated Successfully",
                Data = customer
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, cannot update the Customer ");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [Authorize]
    [RequiresClaim(IdentityData.AdminUserClaimName, "true")]
    [HttpDelete("{customerId}")]
    public async Task<IActionResult> DeleteCustomer(string customerId)
    {
        try
        {
            if (!Guid.TryParse(customerId, out Guid customerIdGuid))
            {
                return BadRequest("Invalid customer ID Format");
            }
            var result = await _dbContext.DeleteCustomerService(customerIdGuid);
            if (!result)
            {
                return NotFound(new ErrorMessage
                {
                    Message = "The Customer is not found to be deleted"
                });
            }
            return Ok(new { success = true, message = " Customer is deleted successfully" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred, the Customer can not deleted");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }
}
