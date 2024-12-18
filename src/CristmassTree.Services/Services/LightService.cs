using CristmassTree.Common.Models;
using CristmassTree.Data.Data;
using CristmassTree.Data.Models;
using CristmassTree.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CristmassTree.Services.Services;

public class LightService : ILightsService
{
    private readonly ApplicationDbContext context;

    public LightService(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<IEnumerable<LightDto>> GetAllAsync()
    {
        var lights = await context.Lights.ToListAsync();

        return lights.Select(light => new LightDto
        {
            Id = light.Id,
            X = light.X,
            Y = light.Y,
            Radius = light.Radius,
            Color = light.Color,
            Effect = light.Effect,
            Desc = light.Description ?? string.Empty,
            CT = light.CT,
        }).ToList();
    }

    public async Task<LightDto?> GetByIdAsync(int id)
    {
       Light? light = await context.Lights.FindAsync(id);

       return (light == null) ? null : new LightDto
       {
           Id = light.Id,
           X = light.X,
           Y = light.Y,
           Radius = light.Radius,
           Color = light.Color,
           Effect = light.Effect,
           Desc = light.Description ?? string.Empty,
           CT = light.CT,
       };
    }

    public async Task AddAsync(Light entity)
    {
        context.Set<Light>().Add(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Set<Light>().FindAsync(id);
        if (entity != null)
        {
            context.Set<Light>().Remove(entity);
            await context.SaveChangesAsync();
        }
    }
}