using CristmassTree.Services.Models;

namespace CristmassTree.Services.Contracts;

public interface ILightValidator
{
    Task<bool> ValidateLightAsync(Light light);
}