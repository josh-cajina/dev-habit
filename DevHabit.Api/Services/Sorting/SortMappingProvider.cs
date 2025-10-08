using System.Linq.Dynamic.Core;

namespace DevHabit.Api.Services.Sorting;

public sealed class SortMappingProvider(IEnumerable<ISortMappingDefinition> sortMappingDefinitions)
{
    public SortMapping[] GetMappings<TSource, TDestination>()
    {
        SortMappingDefinition<TSource, TDestination>? sortMappingDefinition = sortMappingDefinitions
            .OfType<SortMappingDefinition<TSource, TDestination>>()
            .FirstOrDefault();

        if (sortMappingDefinition is null)
        {
            throw new InvalidOperationException($"The mapping from '{typeof(TSource).Name}' into '{typeof(TDestination).Name}' isn't defined");
        }

        return sortMappingDefinition.Mappings;
    }

    public bool ValidateMappings<TSource, TDestination>(string? sort) 
    {
        if (string.IsNullOrWhiteSpace(sort))
        {
            return true;
        }

        var sortFields = sort
            .Split(',')
            .Select(sortField => sortField.Trim().Split(' ')[0])
            .Where(sortField => !string.IsNullOrWhiteSpace(sortField))
            .ToList();

        SortMapping[] mapping = GetMappings<TSource, TDestination>();

        return sortFields.All(sortField => mapping.Any(m => m.SortField.Equals(sortField, StringComparison.OrdinalIgnoreCase)));
    }
}
