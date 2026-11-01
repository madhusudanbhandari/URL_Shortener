using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using url.Domain.Entities;

namespace url.Infrastructure.Persistence.Configurations;

public class ShortCodeConfiguration: IEntityTypeConfiguration<ShortUrl>
{
    public void Configure(EntityTypeBuilder<ShortUrl> builder)
    {
        builder.ToTable("ShortUrls");

        builder.HasKey(x=>x.Id);

        builder.Property(x=>x.OriginalUrl)
                .IsRequired()
                .HasMaxLength(2048);
        
        builder.Property(x=>x.ShortCode)
                .IsRequired()
                .HasMaxLength(10);
        
        builder.HasIndex(x=>x.ShortCode)
                .IsUnique()
                .HasDatabaseName("IX_ShortUrls_ShortCode");

        builder.Property(x=>x.CreatedAt)
                .IsRequired();

        builder.Property(x=>x.IsActive)
                .IsRequired();


    }
    
}