namespace CristmassTree.Data.Data;

using CristmassTree.Data.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Light> Lights { get; set; }

    public DbSet<Light> Effects { get; set; }

    public DbSet<Light> Colors { get; set; }
}