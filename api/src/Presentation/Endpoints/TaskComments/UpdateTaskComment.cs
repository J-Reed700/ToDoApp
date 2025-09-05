using FastEndpoints;
using MediatR;
using ToDoApp.Application.TaskManagement.TaskComments.Commands.UpdateTaskComment;

namespace ToDoApp.Web.Endpoints.TaskComments;

public class UpdateTaskCommentRequest
{
    public string? Comment { get; set; }
}

public class UpdateTaskComment : Endpoint<UpdateTaskCommentRequest>
{
    public override void Configure()
    {
        Put("/api/taskcomments/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateTaskCommentRequest req, CancellationToken ct)
    {
        var id = Route<long>("id");
        var comment = req?.Comment ?? string.Empty;
        var result = await Resolve<IMediator>().Send(new UpdateTaskItemCommentCommand(id, comment), ct);
        await Send.OkAsync(result, ct);
    }
}
