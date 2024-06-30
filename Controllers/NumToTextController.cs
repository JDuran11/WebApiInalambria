using Microsoft.AspNetCore.Mvc;
using WebApiInalambria.Ports;
using WebApiInalambria.DTOs.NumToText;
using Microsoft.AspNetCore.Authorization;

namespace WebApiInalambria.Controllers
{
    [ApiController]
    [Route("api/numToText")]
    public class NumToTextController : ControllerBase
    {
        private readonly INumToTextRepositoryPort _repository;

        public NumToTextController(INumToTextRepositoryPort repository)
        {
            _repository = repository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var prueba = _repository.GetMessage();
            return Ok(new { message = prueba});
        }

        [Authorize]
        [HttpPost]
        public IActionResult NumToText(NumDTO number)
        {
            try
            {
                var text = _repository.NumberToWords(number);
                return Ok(text);
            } catch (Exception ex)
            {
                return Ok(new { message = ex.Message });
            }
            
        }
    }
    
}
