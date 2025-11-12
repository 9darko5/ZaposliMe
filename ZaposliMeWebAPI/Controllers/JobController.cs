using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZaposliMe.Application.Commands.Job.ApproveApplication;
using ZaposliMe.Application.Commands.Job.CreateJob;
using ZaposliMe.Application.Commands.Job.DeleteJob;
using ZaposliMe.Application.Commands.Job.UpdateJob;
using ZaposliMe.Application.DTOs;
using ZaposliMe.Application.DTOs.Job;
using ZaposliMe.Application.Queries.Job.GetAllJobs;

namespace ZaposliMe.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IValidator<JobDto> _jobValidator;

        public JobController(ISender sender, IValidator<JobDto> jobValidator)
        {
            _sender = sender;
            _jobValidator = jobValidator;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> CreateJob([FromBody] JobDto model, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();

            var result = await _jobValidator.ValidateAsync(model, ct);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => new ValidationErrorDto { PropertyName = e.PropertyName, ErrorKey = e.ErrorMessage }));

            var cmd = new CreateJobCommand(
                userId,
                model.Title!,
                model.Description!,
                model.NumberOfWorkers!.Value,
                model.CityId!.Value);

            var id = await _sender.Send(cmd, ct);
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
        public async Task<IActionResult> UpdateJob([FromBody] JobDto model, CancellationToken ct)
        {
            var result = await _jobValidator.ValidateAsync(model, ct);
            if (!result.IsValid)
                return BadRequest(result.Errors.Select(e => new ValidationErrorDto { PropertyName = e.PropertyName, ErrorKey = e.ErrorMessage }));

            var cmd = new UpdateJobCommand(
                model.Id,
                model.Title!,
                model.Description!,
                model.NumberOfWorkers!.Value,
                model.CityId!.Value);

            await _sender.Send(cmd, ct);
            return Ok();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllJobs(
        [FromQuery] Guid? cityId,
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to,
        CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new GetAllJobsQuery(cityId, from, to, userId);
            var jobs = await _sender.Send(query, ct);
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

        [HttpGet("employerapplications")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetAllEmployerApplications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null)
                return Unauthorized();

            var getEmployerApplicationsQuery = new GetEmployerApplicationsQuery(userId);

            var applications = await _sender.Send(getEmployerApplicationsQuery);

            return Ok(applications.ToList());
        }

        [HttpPost("approveapplication")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> ApproveApplication(ChangeApplicationStatusDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            var approveApplicationCommand = new ApproveApplicationCommand(model.ApplicationId, model.JobId);

            await _sender.Send(approveApplicationCommand);

            return Ok();
        }

        [HttpPost("rejectapplication")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> RejectApplication(ChangeApplicationStatusDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            var rejectApplicationCommand = new RejectApplicationCommand(model.ApplicationId, model.JobId);

            await _sender.Send(rejectApplicationCommand);

            return Ok();
        }

        [HttpPost("withdrawapplication")]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> WithdrAwapplication(ChangeApplicationStatusDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null)
                return Unauthorized();

            var withdrawApplicationCommand = new WithdrawApplicationCommand(model.ApplicationId, model.JobId);

            await _sender.Send(withdrawApplicationCommand);

            return Ok();
        }
    }
}
