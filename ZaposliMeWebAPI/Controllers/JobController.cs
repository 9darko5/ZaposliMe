using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZaposliMe.Application.Commands.Job.CreateJob;
using ZaposliMe.Application.Commands.Job.DeleteJob;
using ZaposliMe.Application.Commands.Job.UpdateJob;
using ZaposliMe.Application.DTOs.Job;
using ZaposliMe.Application.Queries.Job.GetAllJobs;

namespace ZaposliMe.WebAPI.Controllers
{
    [ApiController]
    [Route("job/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly ISender _sender;
        public JobController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("/create")]
        public async Task<IActionResult> CreateJob(JobDto model)
        {
            var createdJobCommand = new CreateJobCommand(model.Title, model.Description, model.NumberOfWorkers);

            var id = await _sender.Send(createdJobCommand);

            return Ok(id);
        }

        [HttpPost("/delete")]
        public async Task<IActionResult> DeleteJob(string id)
        {
            var deleteJobCommand = new DeleteJobCommand(id);

            await _sender.Send(deleteJobCommand);

            return Ok();
        }

        [HttpPost("/update")]
        public async Task<IActionResult> UpdateJob(JobDto model)
        {
            var updateJobCommand = new UpdateJobCommand(model.Id, model.Title, model.Description, model.NumberOfWorkers);

            await _sender.Send(updateJobCommand);

            return Ok();
        }

        [HttpGet("/all")]
        public async Task<IActionResult> GetAllJobs()
        {
            var getAllJobsQuery = new GetAllJobsQuery();

            var jobs = await _sender.Send(getAllJobsQuery);

            return Ok(jobs);
        }
    }
}
