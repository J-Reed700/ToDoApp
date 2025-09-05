using FastEndpoints;
using MediatR;
using ToDoApp.Application.Categories.Commands.CreateCategory;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Web.Endpoints.Categories;

public class CreateCategory : Endpoint<CreateCategoryCommand, TaskCategory>
{
    public override void Configure()
    {
        Post("/api/categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateCategoryCommand req, CancellationToken ct)
    {
        var result = await Resolve<IMediator>().Send(req, ct);
        await Send.OkAsync(result);
    }
}
