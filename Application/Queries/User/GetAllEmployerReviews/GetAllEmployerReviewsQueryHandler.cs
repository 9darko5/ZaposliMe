using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Domain.ViewModels.User;
using ZaposliMe.Persistance;

namespace ZaposliMe.Application.Queries.User.GetAllEmployerReviews
{
    public class GetAllEmployerReviewsQueryHandler : IRequestHandler<GetAllEmployerReviewsQuery, List<EmployerReviewGridView>>
    {
        private readonly ZaposliMeQueryDbContext _context;

        public GetAllEmployerReviewsQueryHandler(ZaposliMeQueryDbContext context)
        {
            _context = context;
        }
        public async Task<List<EmployerReviewGridView>> Handle(GetAllEmployerReviewsQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _context.EmployerReviewGridViews.ToListAsync();
            return reviews;
        }
    }
}
