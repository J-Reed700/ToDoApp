using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Application.TaskItems.Queries.GetTaskItems;

public class TaskItemDto
{
    public long Id { get; init; }

    public long CategoryId { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    public Status Status { get; init; }

    public Priority Priority { get; init; }

    public DateTime? DueDate { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TaskItem, TaskItemDto>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
        }
    }
}
