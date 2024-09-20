
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;

namespace minimal_api.Domain.Interfaces
{
    public interface IVehicleService
    {
        List<Vehicle> GetAll(int? page, string? name=null, string? mark=null);

        Vehicle? GetById(int id);

        Vehicle Add(Vehicle vehicle);

        void Update(Vehicle vehicle);

        int Delete(Vehicle vehicle);
    }
}