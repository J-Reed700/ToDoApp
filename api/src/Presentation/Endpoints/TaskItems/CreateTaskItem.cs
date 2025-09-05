using FastEndpoints;
using MediatR;
using ToDoApp.Application.TaskItems.Commands.CreateTaskItem;
using ToDoApp.Application.TaskManagement.TaskItems;

namespace ToDoApp.Web.Endpoints.TaskItems;

public class CreateTaskItem : Endpoint<CreateTaskItemCommand, TaskItemDto>
{
    public override void Configure()
    {       
        Post("/api/task");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateTaskItemCommand req, CancellationToken ct)
    {
        var result = await Resolve<IMediator>().Send(req, ct);
        await Send.OkAsync(result, ct);
    }
}
