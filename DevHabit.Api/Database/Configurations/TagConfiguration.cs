using DevHabit.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevHabit.Api.Database.Configurations;

public sealed class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(tag => tag.Id);

        builder.Property(tag => tag.Id).HasMaxLength(500);

        builder.Property(tag => tag.Name).IsRequired().HasMaxLength(50);

        builder.Property(tag => tag.Description).HasMaxLength(500);

        builder.HasIndex(tag => new { tag.Name }).IsUnique();
    }
}

