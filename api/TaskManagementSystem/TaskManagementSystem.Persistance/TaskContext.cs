using Microsoft.EntityFrameworkCore;

namespace TaskManagementSystem.Persistance;

public class TaskContext(DbContextOptions<TaskContext> options) : DbContext(options)
{
    public virtual DbSet<Task> Tasks { get; set; }
}
