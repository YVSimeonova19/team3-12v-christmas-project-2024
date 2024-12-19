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
    public class LightsController(
        LightFactory lightFactory,
        ILogger<LightsController> logger,
        LightService lightService,
        ICurrentToken currentToken)
        : ControllerBase
    {
        private readonly LightFactory lightFactory = lightFactory;
        private readonly ILogger<LightsController> logger = logger;
        private readonly LightService lightService = lightService;
        private readonly ICurrentToken currentToken = currentToken;

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
            if (currentToken.GetToken() == string.Empty)
            {
                this.logger.LogError("No christmas token provided");
                return this.BadRequest(new { succeeded = false });
            }

            if (model.desc == null)
            {
                this.logger.LogError("Error adding light: description is null");
                return this.BadRequest(new { succeeded = false });
            }

            // var sanitizer = new HtmlSanitizer();
            // model.desc = sanitizer.Sanitize(model.desc);

            // invisible character bypasses JavaScript injection
            model.desc = HttpUtility.HtmlEncode("\u200e" + model.desc);
            var light = await this.lightFactory.CreateLight(model.desc, currentToken.GetToken());

            await lightService.AddAsync(light);
            this.logger.LogInformation($"Created light: {JsonSerializer.Serialize(light)}");
            return this.Ok(new { succeeded = true });
        }
    }
}
