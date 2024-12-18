using System.Web;
using CristmassTree.Presentation.Models;
using CristmassTree.Services.Contracts;
using CristmassTree.Services.Factory;
using CristmassTree.Services.Services;
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
        private readonly LightService lightService;

        public LightsController(
            LightFactory lightFactory,
            ILightValidator validationChain, // Change this line
            ILogger<LightsController> logger, LightService lightService)
        {
            this.lightFactory = lightFactory;
            this.validator = validationChain;
            this.logger = logger;
            this.lightService = lightService;
        }

        // GET
        [HttpGet]
        public async Task<string> Get()
        {
            return JsonSerializer.Serialize(await lightService.GetAllAsync());
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LightPostViewModel model)
        {
            if (!this.Request.Headers.TryGetValue("Christmas-Token", out var ct))
            {
                this.logger.LogError("No christmas token provided");
                return this.BadRequest();
            }

            if (model.desc == null)
            {
                this.logger.LogError("Error adding light: description is null");
                return this.BadRequest();
            }

            // var sanitizer = new HtmlSanitizer();
            // model.desc = sanitizer.Sanitize(model.desc);

            // invisible character bypasses JavaScript injection
            model.desc = HttpUtility.HtmlEncode("\u200e" + model.desc);
            var light = await this.lightFactory.CreateLight(model.desc, ct!);

            await lightService.AddAsync(light);
            this.logger.LogInformation($"Created light: {JsonSerializer.Serialize(light)}");
            return this.Ok();
        }
    }
}
