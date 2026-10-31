namespace url.Domain.Entities;

public class ShortUrl
{
    public long Id{get;private set;}
    public string OriginalUrl{get;private set;}="";
    public string ShortCode{get;private set;}="";
    public DateTime CreatedAt{get;private set;}
    public DateTime? ExpiresAt{get;private set;}
    public bool IsActive{get;private set;}

    public long? UserId{get;private set;}


    private ShortUrl(){} //For Ef Core

    public ShortUrl(string originalUrl, string shortCode, DateTime? expiresAt=null, long? userId=null)  //for application code validation
    {
        if (string.IsNullOrWhiteSpace(originalUrl))
        {
            throw new ArgumentException("Original url cannot be empty");
        }

        if(!Uri.TryCreate(originalUrl,UriKind.Absolute,out var uri)||
        (uri.Scheme!=Uri.UriSchemeHttp &&
        uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("Invalid url");
        }

        if(string.IsNullOrWhiteSpace(shortCode))
            throw new ArgumentException("Short code cannot be empty");

        if(expiresAt.HasValue && expiresAt.Value<=DateTime.UtcNow)
            throw new ArgumentException("Expiration date must be in future");


        OriginalUrl=originalUrl.Trim();
        ShortCode=shortCode;
        CreatedAt=DateTime.UtcNow;
        ExpiresAt=expiresAt;
        IsActive=true;
        UserId=userId;

    }

    public bool IsExpired()
    {
        return ExpiresAt.HasValue && ExpiresAt.Value<=DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive=false;
    }

    public bool CanRedirect()
    {
        return IsActive && !IsExpired();
    }

}