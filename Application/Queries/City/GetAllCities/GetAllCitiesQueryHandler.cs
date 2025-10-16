using MediatR;
using Microsoft.EntityFrameworkCore;
using ZaposliMe.Application.Queries.City.GetAllCities;
using ZaposliMe.Domain.ViewModels.City;
using ZaposliMe.Domain.ViewModels.Job;
using ZaposliMe.Persistance;

namespace ZaposliMe.Application.Queries.Job.GetAllJobs
{
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, List<CityView>>
    {
        private readonly ZaposliMeQueryDbContext _context;

        public GetAllCitiesQueryHandler(ZaposliMeQueryDbContext context)
        {
            _context = context;
        }

        public async Task<List<CityView>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            var cities = await _context.CityViews.ToListAsync();
            return cities;
        }
    }
}
