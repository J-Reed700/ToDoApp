using FastEndpoints;

namespace ToDoApp.Web.Endpoints.Categories;

public class CategoriesOptions : EndpointWithoutRequest
{
    public override void Configure()
    {
        Options("/api/categories");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await Send.OkAsync(ct);
    }
}
