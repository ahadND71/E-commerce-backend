using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("/api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        public CustomerController()
        {
            _customerService = new CustomerService();
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = _customerService.GetAllCustomersService();
            return Ok(customers);
        }

        [HttpGet("{customerId}")]
        public IActionResult GetCustomer(string customerId)
        {
            if (!Guid.TryParse(customerId, out Guid customerIdGuid))
            {
                return BadRequest("Invalid customer ID Format");
            }
            var customer = _customerService.GetCustomerById(customerIdGuid);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(customer);
            }

        }

        [HttpPost]
        public IActionResult CreateCustomer(Customer newCustomer)
        {
            var createdCustomer = _customerService.CreateCustomerService(newCustomer);
            return CreatedAtAction(nameof(GetCustomer), new { customerId = createdCustomer.CustomerId }, createdCustomer);
        }


        [HttpPut("{customerId}")]
        public IActionResult UpdateCustomer(string customerId, Customer updateCustomer)
        {
            if (!Guid.TryParse(customerId, out Guid customerIdGuid))
            {
                return BadRequest("Invalid customer ID Format");
            }
            var customer = _customerService.UpdateCustomerService(customerIdGuid, updateCustomer);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }


        [HttpDelete("{customerId}")]
        public IActionResult DeleteCustomer(string customerId)
        {
            if (!Guid.TryParse(customerId, out Guid customerIdGuid))
            {
                return BadRequest("Invalid customer ID Format");
            }
            var result = _customerService.DeleteCustomerService(customerIdGuid);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

    }
}