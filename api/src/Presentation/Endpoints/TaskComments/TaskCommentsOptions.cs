using FastEndpoints;

namespace ToDoApp.Web.Endpoints.TaskComments;

public class TaskCommentsOptions : EndpointWithoutRequest
{
    public override void Configure()
    {
        Options("/api/taskcomments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync(ct);
    }
}

public class TaskCommentsIdOptions : EndpointWithoutRequest
{
    public override void Configure()
    {
        Options("/api/taskcomments/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync(ct);
    }
}
