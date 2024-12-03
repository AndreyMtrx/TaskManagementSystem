using AutoMapper;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.Queries;

public class TaskItemDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public TaskItemStatus Status { get; set; }

    public string AssignedTo { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TaskItem, TaskItemDto>();
        }
    }
}
