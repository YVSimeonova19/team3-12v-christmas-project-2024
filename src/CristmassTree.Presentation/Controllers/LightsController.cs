using CristmassTree.Presentation.Models;
using CristmassTree.Services;
using CristmassTree.Services.Models;
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
        List<Light> _lights;

        public LightsController(LightFactory lightFactory)
        {
            _lightFactory = lightFactory;
            _lights = new List<Light>();
        }

        // GET: api/<LightsController>
        [HttpGet]
        public string Get()
        {
            Console.WriteLine(JsonSerializer.Serialize(_lights));
            return JsonSerializer.Serialize(_lights);
        }


        // POST api/<LightsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            var light = JsonSerializer.Deserialize<LightPostViewModel>(value);
            _lightFactory.CreateLight(light.desc, light.ct);
        }

    }
}
