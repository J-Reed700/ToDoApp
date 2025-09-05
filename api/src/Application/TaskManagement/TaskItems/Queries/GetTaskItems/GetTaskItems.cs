using ToDoApp.Application.TaskManagement.TaskItems;

namespace ToDoApp.Application.TaskItems.Queries.GetTaskItems;

public record GetTaskItemsQuery : IRequest<List<TaskItemDto>>
{
    public int? categoryId { get; init; }
}

public class GetTaskItemsQueryHandler : IRequestHandler<GetTaskItemsQuery, List<TaskItemDto>>
{
    private readonly ITaskItemRepository _toDoTaskRepository;
    private readonly IMapper _mapper;

    public GetTaskItemsQueryHandler(ITaskItemRepository toDoTaskRepository, IMapper mapper)
    {
        _toDoTaskRepository = toDoTaskRepository;
        _mapper = mapper;
    }

    public async Task<List<TaskItemDto>> Handle(GetTaskItemsQuery request, CancellationToken cancellationToken)
    {
        var query = request.categoryId.HasValue 
            ? _toDoTaskRepository.GetAllByCategoryIdQuery(request.categoryId.Value)
            : _toDoTaskRepository.GetAllQuery();

        return await query
            .OrderBy(x => x.Title)
            .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
