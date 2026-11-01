using Microsoft.Extensions.DependencyInjection;
using url.Application.Interface;
using url.Application.Services;

namespace url.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services
    )
    {
        services.AddScoped<ShortCodeGenerator>();
        services.AddScoped<IUrlService,UrlService>();

        return services;
    }
}