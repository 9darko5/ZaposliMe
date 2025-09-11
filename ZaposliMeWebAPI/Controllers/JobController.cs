using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZaposliMe.Application.Commands.Job.CreateJob;
using ZaposliMe.Application.Commands.Job.DeleteJob;
using ZaposliMe.Application.Commands.Job.UpdateJob;
using ZaposliMe.Application.DTOs.Job;
using ZaposliMe.Application.Queries.Job.GetAllJobs;

namespace ZaposliMe.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly ISender _sender;
        public JobController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateJob(JobDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            var createdJobCommand = new CreateJobCommand(userId, model.Title, model.Description, model.NumberOfWorkers);

            var id = await _sender.Send(createdJobCommand);

            return Ok(id);
        }

        [HttpDelete("delete/{id:guid}")]
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> DeleteJob(Guid id)
        {
            var deleteJobCommand = new DeleteJobCommand(id);

            await _sender.Send(deleteJobCommand);

            return Ok();
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin,Employer")]
        public async Task<IActionResult> UpdateJob(JobDto model)
        {
            var updateJobCommand = new UpdateJobCommand(model.Id, model.Title, model.Description, model.NumberOfWorkers);

            await _sender.Send(updateJobCommand);

            return Ok();
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetAllJobs()
        {
            var getAllJobsQuery = new GetAllJobsQuery();

            var jobs = await _sender.Send(getAllJobsQuery);

            return Ok(jobs);
        }

        [HttpGet("employer/all")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetAllEmployerJobs()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            var getAllEmployerJobsQuery = new GetAllEmployerJobsQuery(userId);

            var jobs = await _sender.Send(getAllEmployerJobsQuery);

            return Ok(jobs);
        }

        [HttpPost("apply")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> ApplyJob(JobDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            var createJobApplicationCommand = new CreateJobApplicationCommand(userId, model.Id);

            var id = await _sender.Send(createJobApplicationCommand);

            return Ok(id);
        }

        [HttpGet("userapplications")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GetAllUserApplications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            var getUserApplicationsQuery = new GetUserApplicationsQuery(userId);

            var applications = await _sender.Send(getUserApplicationsQuery);

            return Ok(applications);
        }
    }
}
