namespace ToDoApp.Application.TaskManagement.TaskComments.Queries.GetTaskComments;

public class GetTodoCommentsQuery : IRequest<List<TaskItemCommentDto>>
{
    public long TaskId { get; init; }
}

public class GetTodoCommentsQueryHandler : IRequestHandler<GetTodoCommentsQuery, List<TaskItemCommentDto>>
{
    private readonly ITaskCommentRepository _toDoTaskCommentRepository;
    private readonly IMapper _mapper;
    public GetTodoCommentsQueryHandler(ITaskCommentRepository toDoTaskCommentRepository, IMapper mapper)
    {
        _toDoTaskCommentRepository = toDoTaskCommentRepository;
        _mapper = mapper;
    }

    public async Task<List<TaskItemCommentDto>> Handle(GetTodoCommentsQuery request, CancellationToken cancellationToken)
    {
        var comments = await _toDoTaskCommentRepository
        .GetAllByTaskIdQuery(request.TaskId)
        .ProjectTo<TaskItemCommentDto>(_mapper.ConfigurationProvider)
        .ToListAsync(cancellationToken);
        
        return comments.OrderBy(x => x.Created).ToList();
    }
}