using DevHabit.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevHabit.Api.Database.Configurations;

public sealed class HabitTagConfiguration : IEntityTypeConfiguration<HabitTag>
{
    public void Configure(EntityTypeBuilder<HabitTag> builder)
    {
        builder.HasKey(habitTag => new { habitTag.HabitId, habitTag.TagId });

        builder.HasOne<Tag>()
            .WithMany()
            .HasForeignKey(habitTag => habitTag.TagId);

        builder.HasOne<Habit>()
            .WithMany()
            .HasForeignKey(habitTag => habitTag.HabitId);
    }
}
