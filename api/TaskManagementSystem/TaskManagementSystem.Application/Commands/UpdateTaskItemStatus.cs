using MediatR;
using TaskManagementSystem.Application.Common.Exceptions;
using TaskManagementSystem.Application.Common.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Commands;

public record UpdateTaskItemStatusCommand : IRequest<Unit>
{
    public int Id { get; init; }
    public TaskItemStatus Status { get; init; }
}

public class UpdateTaskItemStatusCommandHandler(IApplicationDbContext context, IServiceBusHandler serviceBusHandler) : IRequestHandler<UpdateTaskItemStatusCommand, Unit>
{
    private readonly IApplicationDbContext _context = context;
    private readonly IServiceBusHandler _serviceBusHandler = serviceBusHandler;

    public async Task<Unit> Handle(UpdateTaskItemStatusCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TaskItems.FindAsync([request.Id], cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TaskItem), request.Id);
        }

        entity.Status = request.Status;

        await _context.SaveChangesAsync(cancellationToken);

        if (entity.Status == request.Status)
        {
            await _serviceBusHandler.SendMessage(new
            {
                TaskId = entity.Id,
                Name = entity.Name,
                AssignedTo = entity.AssignedTo,
                Status = entity.Status.ToString()
            }, "TaskStatusUpdated");
        }

        return Unit.Value;
    }
}
