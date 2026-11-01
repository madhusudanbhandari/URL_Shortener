using url.Application.Dtos;

namespace url.Application.Interface;

public interface IUrlService
{
    Task<CreateShortUrlResponse> CreateAsync(CreateShortUrlRequest request,CancellationToken cancellationToken);
    Task<string> GetOriginalUrl(string shortCode,CancellationToken cancellationToken);
}