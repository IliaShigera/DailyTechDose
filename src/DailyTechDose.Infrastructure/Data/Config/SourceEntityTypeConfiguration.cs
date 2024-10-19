namespace DailyTechDose.Infrastructure.Data.Config;

internal sealed class SourceEntityTypeConfiguration : IEntityTypeConfiguration<Source>
{
    public void Configure(EntityTypeBuilder<Source> builder)
    {
        builder.ToTable("Sources");
        
        builder.HasMany(s => s.ContentItems)
            .WithOne(c => c.Source)
            .HasForeignKey(c => c.SourceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(s => s.SourceName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(s => s.SourceUrl)
            .IsRequired()
            .HasMaxLength(500);
        
        builder.Property(s => s.FeedUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(s => s.IsRssSupported)
            .IsRequired();
        
        builder.Property(s => s.LastFetchedDate)
            .HasDefaultValue(null);
    }
}