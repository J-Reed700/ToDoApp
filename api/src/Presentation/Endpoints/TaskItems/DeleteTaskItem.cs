using FastEndpoints;
using MediatR;
using ToDoApp.Application.TaskItems.Commands.DeleteTaskItem;

namespace ToDoApp.Web.Endpoints.TaskItems;

public class DeleteTaskItem : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/task/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<long>("id");
        await Resolve<IMediator>().Send(new DeleteTaskItemCommand(id), ct);
        await Send.OkAsync();
    }
}
