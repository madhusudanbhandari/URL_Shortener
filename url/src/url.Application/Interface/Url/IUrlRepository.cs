using url.Application.Dtos;
using url.Domain.Entities;

namespace url.Application.Interface;

public interface IUrlRepository
{
    Task<bool> TryAddAsync(ShortUrl shortUrl, CancellationToken cancellationToken);
}