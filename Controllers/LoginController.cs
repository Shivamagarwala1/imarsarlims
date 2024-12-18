using iMARSARLIMS.Interface;
using iMARSARLIMS.Request_Model;
using iMARSARLIMS.Response_Model;
using Microsoft.AspNetCore.Mvc;

namespace iMARSARLIMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IempMasterServices _empMasterServices;

        // Constructor for dependency injection
        public LoginController(IempMasterServices empMasterServices)
        {
            _empMasterServices = empMasterServices;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ServiceStatusResponseModel>> EmpLogin([FromBody] LoginRequestModel loginRequestModel)
        {
            if (loginRequestModel == null)
                return BadRequest("Invalid login request model.");

            try
            {
                var result = await _empMasterServices.EmpLogin(loginRequestModel);

                if (result == null)
                    return Unauthorized("Invalid username or password.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (if logging is set up in your project)
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        
    }
}
