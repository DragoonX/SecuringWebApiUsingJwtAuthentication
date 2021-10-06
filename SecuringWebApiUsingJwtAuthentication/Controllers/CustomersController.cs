using Microsoft.AspNetCore.Mvc;
using SecuringWebApiUsingJwtAuthentication.Interfaces;
using SecuringWebApiUsingJwtAuthentication.Requests;
using System.Threading.Tasks;

namespace SecuringWebApiUsingJwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomersController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Missing login details");
            }

            LoginResponse loginResponse = await customerService.Login(loginRequest);

            if (loginResponse == null)
            {
                return BadRequest($"Invalid credentials");
            }

            return Ok(loginResponse);
        }
    }
}
