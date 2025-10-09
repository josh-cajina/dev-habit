using DevHabit.Api.Database;
using DevHabit.Api.DTOs.HabitTags;
using DevHabit.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("habits/{habitId}/tags")]
public class HabitTagsController(ApplicationDbContext dbContext) : ControllerBase
{
    public static readonly string Name = nameof(HabitTagsController).Replace("Controller", string.Empty);

    [HttpPut]
    public async Task<ActionResult> UpsertHabitTags(string habitId, UpsertHabitTagsDto upsertHabitTagsDto)
    {
        Habit? habit = await dbContext.Habits
            .Include(habit => habit.HabitTags)
            .FirstOrDefaultAsync(habit => habit.Id == habitId);

        if (habit is null)
        {
            return NotFound();
        }

        var currentTagIds = habit.HabitTags.Select(habitTag => habitTag.TagId).ToHashSet();

        if (currentTagIds.SetEquals(upsertHabitTagsDto.TagIds))
        {
            return NoContent();
        }

        List<string> existingTagIds = await dbContext
            .Tags
            .Where(tag => upsertHabitTagsDto.TagIds.Contains(tag.Id))
            .Select(tag => tag.Id)
            .ToListAsync();

        if (existingTagIds.Count != upsertHabitTagsDto.TagIds.Count)
        {
            return BadRequest("One or more tag IDs is invalid");
        }

        habit.HabitTags.RemoveAll(habitTag => !upsertHabitTagsDto.TagIds.Contains(habitTag.TagId));

        string[] tagsIdsToAdd = [.. upsertHabitTagsDto.TagIds.Except(currentTagIds)];

        DateTime now = DateTime.UtcNow;

        habit.HabitTags.AddRange(tagsIdsToAdd.Select(tagId => new HabitTag 
        {
            HabitId = habitId,
            TagId = tagId,
            CreatedAtUtc = now
        }));

        habit.UpdatedAtUtc = now;

        await dbContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{tagId}")]
    public async Task<ActionResult> DeleteHabitTag(string habitId, string tagId)
    {
        HabitTag? habitTag = await dbContext.HabitTags.SingleOrDefaultAsync(habitTag => habitTag.HabitId == habitId && habitTag.TagId == tagId);

        if (habitTag is null)
        {
            return NotFound();
        }

        dbContext.HabitTags.Remove(habitTag);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }
}
