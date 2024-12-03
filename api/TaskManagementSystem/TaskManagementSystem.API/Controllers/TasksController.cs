using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.Commands;
using TaskManagementSystem.Application.Queries;

namespace TaskManagementSystem.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController(IMediator mediator) : ApiControllerBase(mediator)
    {
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetTaskItemsQuery(), HttpContext.RequestAborted));
        }

        [HttpPost]
        public async Task<ActionResult> Add(CreateTaskItemCommand command)
        {
            var taskId = await Mediator.Send(command, HttpContext.RequestAborted);

            return Ok(taskId);
        }

        [HttpPatch("{id}/status")]
        public async Task<ActionResult> UpdateStatus(int id, UpdateTaskItemStatusCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest("The task ID in the URL does not match the task ID in the body.");
            }

            await Mediator.Send(command, HttpContext.RequestAborted);

            return NoContent();
        }
    }
}
