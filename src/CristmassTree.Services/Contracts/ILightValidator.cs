using CristmassTree.Data.Models;

namespace CristmassTree.Services.Contracts;

public partial interface ILightValidator
{
    Task<bool> ValidateLightAsync(Light light);
    ILightValidator SetNext(ILightValidator validator);
}
