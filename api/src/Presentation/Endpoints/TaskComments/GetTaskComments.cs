using FastEndpoints;
using MediatR;
using ToDoApp.Application.TaskManagement.TaskComments.Queries.GetTaskComments;
using ToDoApp.Application.TaskManagement.TaskComments;

namespace ToDoApp.Web.Endpoints.TaskComments;

public class GetTaskComments : EndpointWithoutRequest<List<TaskItemCommentDto>>
{
    public override void Configure()
    {
        Get("/api/taskcomments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var taskId = Query<long>("taskId");
        var result = await Resolve<IMediator>().Send(new GetTodoCommentsQuery { TaskId = taskId }, ct);
        await Send.OkAsync(result, ct);
    }
}
