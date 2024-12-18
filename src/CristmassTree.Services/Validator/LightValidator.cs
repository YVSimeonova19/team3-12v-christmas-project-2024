using CristmassTree.Services.Contracts;
using CristmassTree.Services.Models;

namespace CristmassTree.Services.Validator;

public class LightValidator : ILightValidator
{
    public async Task<bool> ValidateLightAsync(Light light)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync($"https://polygon.gsk567.com/?x={light.X}&y={light.Y}");
        return response.IsSuccessStatusCode;
    }
}