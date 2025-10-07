using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.ViewModels.Job;
using ZaposliMe.Persistance;

namespace ZaposliMe.Application.Queries.Job.GetAllJobs
{
    public class GetGetEmployerApplicationsHandler : IRequestHandler<GetEmployerApplicationsQuery, List<EmployerApplicationView>>
    {
        private readonly ZaposliMeQueryDbContext _context;

        public GetGetEmployerApplicationsHandler(ZaposliMeQueryDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployerApplicationView>> Handle(GetEmployerApplicationsQuery request, CancellationToken cancellationToken)
        {
            var applications = await _context.EmployerApplicationViews.Where(x => x.EmployerId == request.employeeId).ToListAsync();
            return applications;
        }
    }
}
