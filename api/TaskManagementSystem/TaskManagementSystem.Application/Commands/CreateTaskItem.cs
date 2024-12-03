using MediatR;
using TaskManagementSystem.Application.Common.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Commands;

public record CreateTaskItemCommand : IRequest<int>
{
    public string Name { get; set; }

    public string Description { get; set; }

    public TaskItemStatus Status { get; set; }

    public string AssignedTo { get; set; }
}

public class CreateTaskItemCommandHandler(IApplicationDbContext context, IServiceBusHandler serviceBusHandler) : IRequestHandler<CreateTaskItemCommand, int>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IServiceBusHandler _serviceBusHandler = serviceBusHandler;

    public async Task<int> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TaskItem
        {
            Name = request.Name,
            Description = request.Description,
            Status = request.Status,
            AssignedTo = request.AssignedTo
        };

        _context.TaskItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        if (entity.Id > 0)
        {
            await _serviceBusHandler.SendMessage(new
            {
                TaskId = entity.Id,
                Name = entity.Name,
                AssignedTo = entity.AssignedTo,
                Status = entity.Status.ToString()
            }, "TaskCreated");
        }

        return entity.Id;
    }
}