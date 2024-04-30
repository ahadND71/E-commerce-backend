using Microsoft.AspNetCore.Mvc;
using api.Services;
using api.Helpers;

namespace api.Controllers;

[ApiController]
[Route("/api/customers")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _dbContext;
    public CustomerController(CustomerService customerService)
    {
        _dbContext = customerService;
    }

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
                Message = "Customers are returned succeefully",
                Data = customers
            }
                );
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not return the Customer list");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [HttpGet("{customerId:guid}")]
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
                    Message = "Customer is returned succeefully",
                    Data = customer
                });
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not return the Customer");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }

    }


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
                Message = "Customer is created succeefully",
                Data = createdCustomer
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not create new Customer");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }


    [HttpPut("{customerId:guid}")]
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
                    Message = "No Customer To Founed To Update"
                });
            }
            return Ok(new SuccessMessage<Customer>
            {
                Message = "Customer Is Updated Succeefully",
                Data = customer
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , can not update the Customer ");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }

    }


    [HttpDelete("{customerId:guid}")]
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
            return Ok(new { success = true, message = " Customer is deleted succeefully" });
        }

        catch (Exception ex)
        {
            Console.WriteLine($"An error occured , the Customer can not deleted");
            return StatusCode(500, new ErrorMessage
            {
                Message = ex.Message
            });
        }
    }

}
