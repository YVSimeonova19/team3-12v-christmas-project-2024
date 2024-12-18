using System.Diagnostics;
using CristmassTree.Data.Models;
using CristmassTree.Presentation.Models;
using CristmassTree.Services;
using CristmassTree.Services.Contracts;
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
        private readonly LightFactory lightFactory;
        private readonly ILightValidator validator;
        private readonly ILogger<LightsController> logger;

        public LightsController(
            LightFactory lightFactory,
            ILightValidator validationChain, // Change this line
            ILogger<LightsController> logger)
        {
            this.lightFactory = lightFactory;
            this.validator = validationChain;
            this.logger = logger;
        }

        // GET
        [HttpGet]
        public async Task<string> Get()
        {
            var light = await this.lightFactory.CreateLight("Test Light", "Bi8P6uenVgX8bj6R1VBe0qNG1ukbbDtWHPA3G5L1ciHfK14rgePL6M3EFGYRxi9O");
            return JsonSerializer.Serialize(light);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LightPostViewModel model)
        {
            Console.WriteLine("here");
            if (!this.Request.Headers.TryGetValue("Christmas-Token", out var ct))
            {
                this.logger.LogError("No christmas token provided");
                return this.BadRequest();
            }

            if (model.desc != null)
            {
                // suppressed stylecop error ct possible null
                var light = await this.lightFactory.CreateLight(model.desc, ct!);
                if (await this.validator.ValidateLightAsync(light))
                {
                    //_lights.Add(light);
                    this.logger.LogInformation($"Created light: {JsonSerializer.Serialize(light)}");
                    return this.Ok();
                }
            }

            this.logger.LogError("Error adding light");
            return this.BadRequest();
        }
    }
}
