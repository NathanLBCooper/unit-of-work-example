using Microsoft.EntityFrameworkCore;
using Uow.EntityFramework.Example.Application;

namespace Uow.EntityFramework.Example.Storage;

public class ExampleDbContext : DbContext
{
    public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options)
    {
    }

    public DbSet<Entity> Entities => Set<Entity>();
}
