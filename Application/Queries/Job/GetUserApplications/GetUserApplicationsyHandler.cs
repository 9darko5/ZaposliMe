using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.ViewModels.Job;
using ZaposliMe.Persistance;

namespace ZaposliMe.Application.Queries.Job.GetAllJobs
{
    public class GetUserApplicationsyHandler : IRequestHandler<GetUserApplicationsQuery, List<UserApplicationView>>
    {
        private readonly ZaposliMeQueryDbContext _context;

        public GetUserApplicationsyHandler(ZaposliMeQueryDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserApplicationView>> Handle(GetUserApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applications = await _context.UserApplicationViews.Where(x => x.EmployeeId == request.employeeId).ToListAsync();
            return applications;
        }
    }
}
