using CristmassTree.Presentation.Models;
using CristmassTree.Services;
using CristmassTree.Services.Models;
using CristmassTree.Services.Validator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CristmassTree.Presentation.Controllers
{
    [Route("/")]
    [ApiController]
    public class LightsController : ControllerBase
    {
        LightFactory _lightFactory;
        LightValidator _validator;
        ILogger _logger;

        public LightsController(
            LightFactory lightFactory, 
            LightValidator validator,
            ILogger<LightsController> logger)
        {
            _lightFactory = lightFactory;
            _validator = validator;
            _logger = logger;
        }

        // GET 
        [HttpGet]
        public string Get()
        {
            return JsonSerializer.Serialize("");
        }


        // POST 
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LightPostViewModel model)
        {
            Console.WriteLine("here");
            if (!Request.Headers.TryGetValue("Christmas-Token", out var ct))
            {
                _logger.LogError("No christmas token provided");
                return BadRequest();
            }

            var light = _lightFactory.CreateLight(model.desc, ct);
            if (await _validator.ValidateLightAsync(light))
            {
                //_lights.Add(light);
                _logger.LogInformation($"Created light: {JsonSerializer.Serialize(light)}");
                return Ok();
            }
            _logger.LogError("Error adding light");
            return BadRequest();
        }

    }
}
