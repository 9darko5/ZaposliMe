using MediatR;
using ZaposliMe.Domain.ViewModels.City;

namespace ZaposliMe.Application.Queries.City.GetAllCities
{
    public record GetAllCitiesQuery : IRequest<List<CityView>>
    {
    }
}
