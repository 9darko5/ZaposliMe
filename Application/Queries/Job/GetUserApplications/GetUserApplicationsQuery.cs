using MediatR;
using ZaposliMe.Domain.ViewModels.Job;

namespace ZaposliMe.Application.Queries.Job.GetAllJobs
{
    public record GetUserApplicationsQuery(string employeeId) : IRequest<List<UserApplicationView>>
    {
    }
}
