namespace CristmassTree.Data;

using Microsoft.EntityFrameworkCore;

public class EntityContext : DbContext
{
    public EntityContext(DbContextOptions<EntityContext> options)
        : base(options)
    {
    }
}