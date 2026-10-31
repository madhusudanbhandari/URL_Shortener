using Microsoft.EntityFrameworkCore;
using url.Domain.Entities;

namespace url.Infrastructure.Persistence.Configurations;

public class urlDbContext : DbContext
{
    public urlDbContext(DbContextOptions<urlDbContext> options)
                    : base(options)
    {
        
    }

    public DbSet<ShortUrl> shortUrls=> Set<ShortUrl>();
    public DbSet<User> users=>Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(urlDbContext).Assembly);  //ef core automatically discovers all the IEntityTypeConfiguration form the assembly
    }

}