using FluentValidation;

namespace DevHabit.Api.DTOs.Tags;

public sealed class CreateTagDtoValidator : AbstractValidator<CreateTagDto>
{
    public CreateTagDtoValidator()
    {
        RuleFor(createTagDto => createTagDto.Name).NotEmpty().MinimumLength(3);

        RuleFor(createTagDto => createTagDto.Description).MaximumLength(50);
    }
}
