using FastEndpoints;
using MediatR;
using ToDoApp.Application.TaskItems.Commands.UpdateTaskItem;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Web.Endpoints.TaskItems;

public class UpdateTaskItemRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public long CategoryId { get; set; }
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }
}

public class UpdateTaskItem : Endpoint<UpdateTaskItemRequest>
{
    public override void Configure()
    {
        Put("/api/task/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateTaskItemRequest req, CancellationToken ct)
    {
        var id = Route<long>("id");
        
        var command = new UpdateTaskItemCommand
        {
            Id = id,
            Title = req.Title,
            Description = req.Description,
            CategoryId = req.CategoryId,
            Status = req.Status,
            Priority = req.Priority,
            DueDate = req.DueDate
        };
        
        var result = await Resolve<IMediator>().Send(command, ct);
        await Send.OkAsync(result, ct);
    }
}
