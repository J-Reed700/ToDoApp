using ToDoApp.Application.Common.Models;
using ToDoApp.Domain.Enums;
using ToDoApp.Application.TaskManagement.Categories;

namespace ToDoApp.Application.TaskManagement.Categories.Queries.GetCategories;


public record GetCategoriesQuery : IRequest<List<CategoryDto>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly ITaskCategoryRepository _repository;
    private readonly IMapper _mapper;

    public GetCategoriesQueryHandler(ITaskCategoryRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllQuery()
        .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
        .OrderBy(t => t.CategoryName)
        .ToListAsync(cancellationToken);
    }
}
