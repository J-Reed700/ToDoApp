using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.TaskManagement.Categories.Queries.GetCategories;

public class ItemDto
{
    public long Id { get; init; }
    public long CategoryId { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public Priority Priority { get; init; }
    public Status Status { get; init; }
    public DateTime? DueDate { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TaskItem, ItemDto>().ForMember(d => d.Priority, 
                opt => opt.MapFrom(s => (int)s.Priority));
        }
    }
}
