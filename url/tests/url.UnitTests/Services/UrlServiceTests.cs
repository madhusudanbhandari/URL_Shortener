using url.Application.Interface;
using Moq;
using url.Application.Services;
using Xunit;
using url.Domain.Entities;
using url.Application.Exceptions;

namespace url.UnitTests;

public class UrlServiceTests
{
    private readonly Mock<IUrlRepository> _repositoryMoq;
    public UrlServiceTests()
    {
        _repositoryMoq=new Mock<IUrlRepository>();
    }

    private UrlService CreateService(DateTimeOffset currentTime)
    {
        return new UrlService(
            _repositoryMoq.Object,
            new ShortCodeGenerator(),
            new TestTimeProvider(currentTime)

        );
    }

    [Fact]
    public async Task GetOriginalUrl_ShouldReturnOriginalUrl_WhenUrlIsValid()
    {
        var shortUrl=new ShortUrl
        (
            "https://www.linkedin.com/in/madhusudan-bhandari-70a392259/",
            "zqxNBFUaRyC"
        );

        _repositoryMoq
            .Setup(r=>r.GetUrlByCode("zqxNBFUaRyC",
            It.IsAny<CancellationToken>()
            )).ReturnsAsync(shortUrl);

        var service=CreateService(DateTimeOffset.UtcNow);


        var result=await service.GetOriginalUrl("zqxNBFUaRyC",CancellationToken.None);

        Assert.Equal("https://www.linkedin.com/in/madhusudan-bhandari-70a392259/",result);
    }

    [Fact]
    public async Task GetOriginalUrl_ShouldThrowNotFoundException_WhenUrlDoesNotExist()
    {
        _repositoryMoq
            .Setup(r => r.GetUrlByCode(
                "InvalidCode",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((ShortUrl?)null);

        var service = CreateService(DateTimeOffset.UtcNow);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetOriginalUrl(
                "InvalidCode",
                CancellationToken.None
            )
        );
    }

        [Fact]
    public async Task GetOriginalUrl_ShouldThrowGoneException_WhenUrlIsInactive()
    {
        var shortUrl = new ShortUrl(
            "https://www.linkedin.com/in/madhusudan-bhandari-70a392259/",
            "zqxNBFUaRyC"
        );

        shortUrl.Deactivate();

        _repositoryMoq
            .Setup(r => r.GetUrlByCode(
                "zqxNBFUaRyC",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(shortUrl);

        var service = CreateService(DateTimeOffset.UtcNow);

        await Assert.ThrowsAsync<GoneException>(
            () => service.GetOriginalUrl(
                "zqxNBFUaRyC",
                CancellationToken.None
            )
        );
    }


    [Fact]
    public async Task GetOriginalUrl_ShouldThrowGoneException_WhenUrlIsExpired()
    {
        var expirationDate = DateTime.UtcNow.AddDays(1);

        var shortUrl = new ShortUrl(
            "https://www.linkedin.com/in/madhusudan-bhandari-70a392259/",
            "zqxNBFUaRyC",
            expirationDate
        );

        var futureTime = DateTimeOffset.UtcNow.AddDays(2);

        _repositoryMoq
            .Setup(r => r.GetUrlByCode(
                "zqxNBFUaRyC",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(shortUrl);

        var service = CreateService(futureTime);

        await Assert.ThrowsAsync<GoneException>(
            () => service.GetOriginalUrl(
                "zqxNBFUaRyC",
                CancellationToken.None
            )
        );
    }


    private sealed class TestTimeProvider : TimeProvider
    {
        private readonly DateTimeOffset _currentTime;

        public TestTimeProvider(DateTimeOffset currentTime)
        {
            _currentTime=currentTime;
        }

        public override DateTimeOffset GetUtcNow()
        {
            return _currentTime;
        }
    }
}