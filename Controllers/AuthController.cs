using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiInalambria.Ports;
using WebApiInalambria.DTOs.Authentication;


namespace WebApiInalambria.Controllers
{
    [Route("api/Autenticacion")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationRepositoryPort _repository;

        public AuthController(IAuthenticationRepositoryPort repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDTO login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest(new { message = "Invalid login request" });
            }

            try
            {
                var token = _repository.GenerateJwtToken(login.Username);
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }
                return Ok(new { token });
            }
            catch (Exception)
            {
                
                return StatusCode(500, new { message = "An error occurred while processing your request." });
            }

        }
    }
}
