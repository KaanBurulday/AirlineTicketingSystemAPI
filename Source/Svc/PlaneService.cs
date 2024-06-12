using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;
using AirlineTicketingSystemAPI.Source.Db;
using AirlineTicketingSystemAPI.Source.Svc.Interfaces;

namespace AirlineTicketingSystemAPI.Source.Svc
{
    public class PlaneService : IPlaneService
    {
        private PlaneAccess _planeAccess;
        private ILogger<PlaneService> _logger;

        public PlaneService(PlaneAccess planeAccess, ILogger<PlaneService> logger)
        {
            _planeAccess = planeAccess ?? throw new ArgumentNullException(nameof(planeAccess));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ICollection<PlaneDto>> GetPlanesAsync()
        {
            try
            {
                var planes = await _planeAccess.GetPlanesAsync();
                List<PlaneDto> planeDtos = new List<PlaneDto>();
                foreach (var plane in planes)
                {
                    planeDtos.Add(CreatePlaneDto(plane));
                }
                return planeDtos.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving planes");
                throw;
            }
        }

        public async Task<PlaneDto> GetPlaneAsync(int Id)
        {
            try
            {
                var plane = await _planeAccess.GetPlaneAsync(Id);
                return CreatePlaneDto(plane);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving plane with ID {Id}", Id);
                throw;
            }
        }

        public async Task<PlaneDto> GetPlaneByCodeAsync(string Code)
        {
            try
            {
                var plane = await _planeAccess.GetPlaneByCodeAsync(Code);
                return CreatePlaneDto(plane);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving plane with ID {Code}", Code);
                throw;
            }
        }

        public async Task<Plane> InsertPlaneAsync(PlaneDto planeDto)
        {
            try
            {
                var plane = await _planeAccess.InsertPlaneAsync(CreatePlane(planeDto));
                return plane;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inserting plane");
                throw;
            }
        }

        public async Task DeletePlaneAsync(int Id)
        {
            try
            {
                await _planeAccess.DeletePlaneAsync(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting plane with ID {Id}", Id);
                throw;
            }
        }

        public async Task DeletePlanesAsync()
        {
            try
            {
                await _planeAccess.DeletePlanesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting planes");
                throw;
            }
        }

        public async Task UpdatePlaneAsync(int id, PlaneDto plane)
        {
            try
            { 
                Plane planeOld = await _planeAccess.GetPlaneAsync(id);
                if (planeOld != null)
                {
                    planeOld.Code = plane.Code;
                    planeOld.Capacity = plane.Capacity;
                    planeOld.Speed = plane.Speed;
                    await _planeAccess.UpdatePlaneAsync(planeOld);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating plane with ID {id}", id);
                throw;
            }
        }

        private PlaneDto CreatePlaneDto(Plane plane)
        {
            PlaneDto ret = new PlaneDto()
            {
                Code = plane.Code,
                Capacity = plane.Capacity,
                Speed = plane.Speed
            };
            return ret;
        }

        private Plane CreatePlane(PlaneDto planeDto)
        {
            Plane ret = new Plane()
            {
                Code = planeDto.Code,
                Capacity = planeDto.Capacity,
                Speed = planeDto.Speed
            };
            return ret;
        }
    }
}
