using ToDoApp.Application.Common.Models;
using ToDoApp.Application.TaskManagement.Categories;

namespace ToDoApp.Application.Categories.Queries.GetCategories;

public class CategoriesVm
{
    public IReadOnlyCollection<LookupDto> PriorityLevels { get; init; } = Array.Empty<LookupDto>();

    public IReadOnlyCollection<LookupDto> StatusLevels { get; init; } = Array.Empty<LookupDto>();

    public IReadOnlyCollection<CategoryDto> Categories { get; init; } = Array.Empty<CategoryDto>();
}
