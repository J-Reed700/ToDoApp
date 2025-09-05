using FastEndpoints;
using MediatR;
using ToDoApp.Application.TaskManagement.TaskComments.Commands.CreateTaskComment;
using ToDoApp.Application.TaskManagement.TaskComments;

namespace ToDoApp.Web.Endpoints.TaskComments;

public class CreateTaskItemComment : Endpoint<CreateTaskItemCommentCommand, TaskItemCommentDto>
{
    private readonly IMediator _mediator;

    public CreateTaskItemComment(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override void Configure()
    {
        Post("/api/taskcomments");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateTaskItemCommentCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        await Send.OkAsync(result, cancellationToken);
    }
}