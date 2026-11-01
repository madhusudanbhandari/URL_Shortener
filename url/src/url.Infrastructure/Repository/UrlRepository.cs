using Microsoft.EntityFrameworkCore;
using Npgsql;
using url.Application.Interface;
using url.Domain.Entities;
using url.Infrastructure.Persistence.Configurations;

namespace url.Infrastructure.Repository;

public class UrlRepository : IUrlRepository
{
    private readonly urlDbContext _context;
    public UrlRepository(urlDbContext context)
    {
        _context=context;
    }

    public async  Task<bool> TryAddAsync(ShortUrl shortUrl, CancellationToken cancellationToken)
    {
        _context.shortUrls.Add(shortUrl);

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }catch(DbUpdateException ex)
        when(ex.InnerException is PostgresException postgresException 
            && postgresException.SqlState=="23505" //for unique constraint violation
            && postgresException.ConstraintName == "IX_ShortUrls_ShortCode")
        {
            _context.Entry(shortUrl).State=EntityState.Detached;
            return false;
        }
    }

    public async Task<ShortUrl?> GetUrlByCode(string shortCode,CancellationToken cancellationToken)
    {
        return await _context.shortUrls.FirstOrDefaultAsync(u=>u.ShortCode==shortCode,cancellationToken);
    }
}