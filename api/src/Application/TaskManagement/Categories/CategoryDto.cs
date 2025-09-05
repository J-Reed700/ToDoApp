using ToDoApp.Domain.Entities;
using ToDoApp.Application.TaskManagement.Categories.Queries.GetCategories;

namespace ToDoApp.Application.TaskManagement.Categories;

public class CategoryDto
{
    public CategoryDto()
    {
        Tasks = Array.Empty<ItemDto>();
    }

    public long Id { get; init; }

    public string? CategoryName { get; init; }


    public IReadOnlyCollection<ItemDto> Tasks { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TaskCategory, CategoryDto>();
        }
    }
}
