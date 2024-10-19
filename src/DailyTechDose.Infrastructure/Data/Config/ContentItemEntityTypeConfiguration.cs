namespace DailyTechDose.Infrastructure.Data.Config;

internal sealed class ContentItemEntityTypeConfiguration : IEntityTypeConfiguration<ContentItem>
{
    public void Configure(EntityTypeBuilder<ContentItem> builder)
    {
        builder.ToTable("ContentItems");

        builder.HasOne(c => c.Source)
            .WithMany(s => s.ContentItems)
            .HasForeignKey(c => c.SourceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(c => c.Summary)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.Link)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.PublishDate)
            .IsRequired();

        builder.Property(c => c.IsPublished)
            .IsRequired();
    }
}