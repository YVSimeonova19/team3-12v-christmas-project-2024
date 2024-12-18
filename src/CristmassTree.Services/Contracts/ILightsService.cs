using CristmassTree.Common.Models;
using CristmassTree.Data.Models;

namespace CristmassTree.Services.Contracts;

public interface ILightsService
{
    Task<IEnumerable<LightDto>> GetAllAsync();
    Task AddAsync(Light entity);
    Task DeleteOldAsync(string currentChristmasToken);
}