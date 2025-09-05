using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.TaskManagement.TaskComments;

public class TaskItemCommentDto
{
    public long Id { get; init; }
    public string? Comment { get; init; }
    public DateTimeOffset Created { get; init; }
    public string? CreatedBy { get; init; }
    public DateTimeOffset LastModified { get; init; }
    public string? LastModifiedBy { get; init; }
    
    public static TaskItemCommentDto MapFrom(TaskItemComment taskItemComment)
    {
        return new TaskItemCommentDto
        {
            Id = taskItemComment.Id,
            Comment = taskItemComment.Comment,
            Created = taskItemComment.Created,
            CreatedBy = taskItemComment.CreatedBy,
            LastModified = taskItemComment.LastModified,
            LastModifiedBy = taskItemComment.LastModifiedBy
        };
    }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TaskItemComment, TaskItemCommentDto>();
        }
    }
}