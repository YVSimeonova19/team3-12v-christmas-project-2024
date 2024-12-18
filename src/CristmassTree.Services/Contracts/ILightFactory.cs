using CristmassTree.Services.Models;

namespace CristmassTree.Services.Contracts;

public interface ILightFactory
{
    Light CreateLight(string description, string ct);
}