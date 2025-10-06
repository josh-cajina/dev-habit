using DevHabit.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevHabit.Api.Database.Configurations;

public sealed class HabitConfiguration : IEntityTypeConfiguration<Habit>
{
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        builder.HasKey(habit => habit.Id);

        builder.Property(habit => habit.Id).HasMaxLength(500);
        builder.Property(habit => habit.Name).HasMaxLength(100);
        builder.Property(habit => habit.Description).HasMaxLength(500);

        builder.OwnsOne(habit => habit.Frequency);
        builder.OwnsOne(habit => habit.Target, targetBuilder => 
        {
            targetBuilder.Property(target => target.Unit).HasMaxLength(100);
        });

        builder.OwnsOne(habit => habit.Milestone);
    }
}
