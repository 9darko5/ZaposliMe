using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZaposliMe.Application.Queries.City.GetAllCities;
using ZaposliMe.Application.Queries.Job.GetAllJobs;

namespace ZaposliMe.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ISender _sender;
        public CityController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCities()
        {
            var getAllCitiesQuery = new GetAllCitiesQuery();

            var cities = await _sender.Send(getAllCitiesQuery);

            return Ok(cities);
        }
    }
}
