using AirlineTicketingSystemAPI.Model;
using AirlineTicketingSystemAPI.Model.Dto;

namespace AirlineTicketingSystemAPI.Source.Svc.Interfaces
{
    public interface IPlaneService
    {
        public Task<ICollection<PlaneDto>> GetPlanesAsync();
        public Task<PlaneDto> GetPlaneAsync(int Id);
        public Task<PlaneDto> GetPlaneByCodeAsync(string Code);
        public Task<Plane> InsertPlaneAsync(PlaneDto planeDto);
        public Task UpdatePlaneAsync(int id, PlaneDto plane);
        public Task DeletePlanesAsync();
        public Task DeletePlaneAsync(int Id);
    }
}
