using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.ViewModels.User;
using ZaposliMe.Persistance;

namespace ZaposliMe.Application.Queries.User.GetAllEmployerReviewsByEmployerId
{
    public class GetAllEmployerReviewsByEmployerIdQueryHandler : IRequestHandler<GetAllEmployerReviewsByEmployerIdQuery, List<EmployerReviewView>>
    {
        private readonly ZaposliMeQueryDbContext _context;

        public GetAllEmployerReviewsByEmployerIdQueryHandler(ZaposliMeQueryDbContext context)
        {
            _context = context;
        }
        public async Task<List<EmployerReviewView>> Handle(GetAllEmployerReviewsByEmployerIdQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _context.EmployerReviewViews.Where(x=>x.EmployerId.Equals(request.EmployerId)).ToListAsync();
            return reviews;
        }
    }
}
