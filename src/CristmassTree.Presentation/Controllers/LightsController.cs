using System.Diagnostics;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web;
using CristmassTree.Data.Models;
using CristmassTree.Presentation.Models;
using CristmassTree.Services;
using CristmassTree.Services.Contracts;
using CristmassTree.Services.Factory;
using CristmassTree.Services.Services;
using CristmassTree.Services.Validator;
using Ganss.Xss;
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

            // tried to add regex, but code is evaluated in the description field, thus making it legitimate in any way
            // regex didn't work for this reason, but ive left the solution also (see Program.cs)

            // if (Regex.IsMatch(model.desc!, @"[`]|<[^>]*>|[]]"))
            // {
            //     this.logger.LogError("XSS Detected");
            //     return this.BadRequest();
            // }

            if (model.desc != null)
            {
                // also tried html sanitizer package, but to no avail
                // var sanitizer = new HtmlSanitizer();
                // model.desc = sanitizer.Sanitize(model.desc);

                // invisible character bypasses XSS though
                model.desc = HttpUtility.HtmlEncode("\u200e" + model.desc);
                var light = await this.lightFactory.CreateLight(model.desc, ct!);
                if (await this.validator.ValidateLightAsync(light))
                {
                    await lightService.AddAsync(light);
                    this.logger.LogInformation($"Created light: {JsonSerializer.Serialize(light)}");
                    return this.Ok();
                }
            }

            this.logger.LogError("Error adding light");
            return this.BadRequest();
        }
    }
}
