using CristmassTree.Common.Models;
using CristmassTree.Data.Models;

namespace CristmassTree.Services.Contracts;

public interface ILightsService
{
    Task<IEnumerable<LightDto>> GetAllAsync();
    Task<LightDto?> GetByIdAsync(int id);
    Task AddAsync(Light entity);
    Task DeleteAsync(int id);
}