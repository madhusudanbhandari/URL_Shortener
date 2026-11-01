using Microsoft.AspNetCore.Mvc;
using url.Application.Dtos;
using url.Application.Interface;
using url.Application.Services;

namespace url.Api.Controller;

[ApiController]
[Route("")]
public class UrlController : ControllerBase
{
    private readonly IUrlService _urlService;

    public UrlController(IUrlService urlService)
    {
        _urlService=urlService;
    }

    [HttpPost("add-url")]
    public async Task<IActionResult> AddUrl([FromBody]CreateShortUrlRequest request,CancellationToken cancellationToken)
    {
        var url=await _urlService.CreateAsync(request,cancellationToken);

        var baseUrl=$"{Request.Scheme}://{Request.Host}";
        
        url.ShortUrl=$"{baseUrl}/{url.ShortCode}";

        return Created(url.ShortUrl,url);

    }

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginal(string shortCode, CancellationToken cancellationToken)
    {
        var originalUrl=await _urlService.GetOriginalUrl(shortCode,cancellationToken);
        return Redirect(originalUrl);
    }
}