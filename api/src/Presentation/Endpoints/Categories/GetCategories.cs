using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using ToDoApp.Application.TaskManagement.Categories.Queries.GetCategories;
using ToDoApp.Application.TaskManagement.Categories;

namespace ToDoApp.Web.Endpoints.Categories;

public class GetCategories : EndpointWithoutRequest<List<CategoryDto>>
{
    public override void Configure()
    {
        Get("/api/categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await Resolve<IMediator>().Send(new GetCategoriesQuery(), ct);
        await Send.OkAsync(result, ct);
    }
}
