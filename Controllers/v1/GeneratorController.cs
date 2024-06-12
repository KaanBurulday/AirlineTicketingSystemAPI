using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AirlineTicketingSystemAPI.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class GeneratorController : ControllerBase
    {
        private IAirportService _airportService;
        private IPlaneService _planeService;

        private ILogger<GeneratorController> _logger;

        public GeneratorController(IAirportService airportService, ILogger<GeneratorController> logger, IPlaneService planeService)
        {
            _airportService = airportService ?? throw new ArgumentNullException(nameof(airportService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _planeService = planeService ?? throw new ArgumentNullException(nameof(planeService));
        }

        [HttpPost("UpdateAvailableAirports")]
        public async Task<IActionResult> UpdateAvailableAirports()
        {
            try
            {
                await _airportService.UpdateAvailableAirportsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airport");
                return StatusCode(500, "Internal server error");
            }
            return Ok();
        }

        [HttpPost("GenerateEnv")]
        public async Task<ActionResult<APIResultDto>> GenerateEnv([FromBody] int airportCount)
        {
            APIResultDto result = new APIResultDto();
            try
            {
                result.Message += await CleanDBAsync();

                result.Message += await GenerateAirportsAsync(airportCount);

                Random random = new Random();
                result.Message += await GeneratePlanesAsync(airportCount * random.Next(5, 15));

                await _airportService.UpdateAvailableAirportsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airport");
                return StatusCode(500, "Internal server error");
            }
            return Ok(result);
        }

        [HttpDelete("CleanDB")]
        public async Task<ActionResult<APIResultDto>> CleanDBAsync([FromBody] int airportCount)
        {
            try
            {
                await CleanDBAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving airport");
                return StatusCode(500, "Internal server error");
            }
            return Ok();
        }

        private async Task<string> CleanDBAsync()
        {
            await _airportService.DeleteAirportsAsync();
            return "Database Cleaned.";
        }

        private async Task<string> GenerateAirportsAsync(int airportCount)
        {
            Random random = new Random();
            double latMax = 90.0;
            double latMin = -90.0;
            double longMax = 180.0;
            double longMin = -180.0;

            for (int i = 0; i < airportCount; i++)
            {
                AirportDto airportDto = new AirportDto()
                {
                    Code = String.Format("AP-{0}", i.ToString("00000")),
                    Name = String.Format("Airport-{0}", i.ToString("00000")),
                    City = String.Format("City-{0}", i.ToString("00000")),
                    Country = String.Format("Country-{0}", i.ToString("00000")),
                    DateCreated = DateTime.Now,
                    Latitude = latMin + (latMax - latMin) * random.NextDouble(),
                    Longitude = longMin + (longMax - longMin) * random.NextDouble()
                };
                await _airportService.InsertAirportAsync(airportDto);
            }
            await _airportService.UpdateAvailableAirportsAsync();
            return $"{airportCount} Airports Generated";
        }

        private async Task<string> GeneratePlanesAsync(int planeCount)
        {
            Random random = new Random();
            for (int i = 0; i < planeCount; i++)
            {
                PlaneDto planeDto = new PlaneDto()
                {
                    Code = String.Format("PL-{0}", i.ToString("00000")),
                    Capacity = random.Next(100, 800),
                    Speed = random.Next(100, 700)
                };
                Plane plane = await _planeService.InsertPlaneAsync(planeDto);
                await _airportService.AddPlaneToRandomAirport(plane.Code);
            }
            return $"{planeCount} Planes Generated";
        }
    }
}
