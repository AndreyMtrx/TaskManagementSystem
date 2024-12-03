using TaskManagementSystem.Domain.Common;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Domain.Entities;

public class TaskItem : BaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public TaskItemStatus Status { get; set; }

    public string AssignedTo { get; set; }
}
