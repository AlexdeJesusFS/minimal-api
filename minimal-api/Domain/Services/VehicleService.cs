
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using minimal_api.Infrastructure.Db;

namespace minimal_api.Domain.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly DataBaseContext _context;

        public VehicleService(DataBaseContext context)
        {
            _context = context;
        }

        public Vehicle Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
            return vehicle;
        }

        public List<Vehicle> GetAll(int? page, string? name = null, string? mark = null)
        {
            var query = _context.Vehicles;
            List<Vehicle> result = [];
            if (!string.IsNullOrEmpty(name)) {
                result = query.Where(v => v.Name.Contains(name.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(mark)) {
                //forma mais atual de realizar a busca ignorando Case e retornando uma Lista
                result = [.. query.Where(v => v.Mark.Contains(mark, StringComparison.CurrentCultureIgnoreCase))]; 
            }
            int itemsPage = 10;

            if (page != null) {
                return result.Skip((int)((page - 1) * itemsPage)).Take(itemsPage).ToList();
            }
            return query.ToList();
        }

        public Vehicle? GetById(int id)
        {
            return _context.Vehicles.Where(v => v.Id == id).FirstOrDefault();
        }

        public void Update(Vehicle vehicle)
        {
            _context.Update(vehicle);
            _context.SaveChanges();
        }

         public int Delete(Vehicle vehicle)
        {
            _context.Remove(vehicle);
            _context.SaveChanges();
            return vehicle.Id;
        }
    }
}