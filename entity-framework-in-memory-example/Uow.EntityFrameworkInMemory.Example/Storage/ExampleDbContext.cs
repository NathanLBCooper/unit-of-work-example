using Microsoft.EntityFrameworkCore;
using Uow.EntityFrameworkInMemory.Example.Application;

namespace Uow.EntityFrameworkInMemory.Example.Storage;

public class ExampleDbContext : DbContext
{
    public ExampleDbContext(DbContextOptions<ExampleDbContext> options) : base(options)
    {
    }

    public DbSet<Entity> Entities => Set<Entity>();
}
