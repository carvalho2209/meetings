using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Meeting.Persistence;

public sealed class ApplicationDbContext : DbContext 
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
}
