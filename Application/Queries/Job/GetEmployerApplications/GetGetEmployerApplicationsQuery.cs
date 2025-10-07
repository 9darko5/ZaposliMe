using MediatR;
using ZaposliMe.Domain.ViewModels.Job;

namespace ZaposliMe.Application.Queries.Job.GetAllJobs
{
    public record GetEmployerApplicationsQuery(string employeeId) : IRequest<List<EmployerApplicationView>>
    {
    }
}
