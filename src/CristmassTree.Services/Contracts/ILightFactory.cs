using CristmassTree.Data.Models;

namespace CristmassTree.Services.Contracts;

public interface ILightFactory
{
    Task<Light> CreateLight(string description, string ct);
}