using System.Net.Http;
using System.Threading.Tasks;
using CristmassTree.Data.Models;

namespace CristmassTree.Services.Validator
{
    public class ExternalApiValidator : LightValidator
    {
        public override async Task<bool> ValidateLightAsync(Light light)
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://polygon.gsk567.com/?x={light.X}&y={light.Y}");

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return await this.ValidateNext(light);
        }
    }
}