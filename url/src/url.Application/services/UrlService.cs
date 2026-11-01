using url.Application.Dtos;
using url.Application.Interface;
using url.Domain.Entities;

namespace url.Application.Services;

public class UrlService : IUrlService
{
    private readonly IUrlRepository _urlRepository;
    private readonly ShortCodeGenerator _codeGenerator;
    private const int MaxAttempts=5;
    public UrlService(IUrlRepository urlRepository, ShortCodeGenerator codeGenerator)
    {
        _urlRepository=urlRepository;
        _codeGenerator=codeGenerator;
    }

    public async Task<CreateShortUrlResponse> CreateAsync(CreateShortUrlRequest urlRequest, CancellationToken cancellationToken)
    {
        for(int i=0; i < MaxAttempts; i++)
        {
            var shortCode=_codeGenerator.Generate();

            var shortUrl=new ShortUrl(
                urlRequest.OriginalUrl,shortCode
            );

            var isSaved=await _urlRepository.TryAddAsync(shortUrl,cancellationToken);

            if (isSaved)
            {
                return new CreateShortUrlResponse
                {
                    OriginalUrl=shortUrl.OriginalUrl,
                    ShortCode=shortUrl.ShortCode
                };
            }
        }

        throw new InvalidOperationException("unable to generate short code after many attempts");
    }

    
}