using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Application.Common.Interfaces;

namespace TaskManagementSystem.Application.Queries;

public record GetTaskItemsQuery : IRequest<IReadOnlyCollection<TaskItemDto>>;

public class GetTaskItemsQueryHandler(IApplicationDbContext context, IMapper mapper) : IRequestHandler<GetTaskItemsQuery, IReadOnlyCollection<TaskItemDto>>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    public async Task<IReadOnlyCollection<TaskItemDto>> Handle(GetTaskItemsQuery request, CancellationToken cancellationToken)
    {
        return await _context.TaskItems
                .AsNoTracking()
                .ProjectTo<TaskItemDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
    }
}

