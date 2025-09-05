using FastEndpoints;
using MediatR;
using ToDoApp.Application.TaskItems.Queries.GetTaskItems;

namespace ToDoApp.Web.Endpoints.TaskItems;

public class GetTaskItems : EndpointWithoutRequest<List<TaskItemDto>>
{
    public override void Configure()
    {
        Get("/api/task");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await Resolve<IMediator>().Send(new GetTaskItemsQuery(), ct);
        await Send.OkAsync(result, ct);
    }
}
