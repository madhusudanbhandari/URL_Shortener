using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using url.Application.Interface;
using url.Infrastructure.Persistence.Configurations;
using url.Infrastructure.Repository;

namespace url.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString=configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Database connection string is missing");
        }

        services.AddDbContext<urlDbContext>(options=>
        options.UseNpgsql(connectionString));

        services.AddScoped<IUrlRepository,UrlRepository>();


        return services;
    }
}