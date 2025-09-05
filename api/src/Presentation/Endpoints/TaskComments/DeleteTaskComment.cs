using FastEndpoints;
using MediatR;
using ToDoApp.Application.TaskManagement.TaskComments.Commands.DeleteTaskComment;

namespace ToDoApp.Web.Endpoints.TaskComments;

public class DeleteTaskComment : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/taskcomments/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var commentId = Route<long>("id");
        await Resolve<IMediator>().Send(new DeleteTaskItemCommentCommand(commentId), ct);
        await Send.OkAsync();
    }
}
