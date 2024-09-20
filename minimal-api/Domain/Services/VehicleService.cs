
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
            // Iniciando a query
            var query = _context.Vehicles.AsQueryable();  // Torna a query filtrável
            
            // Filtrar por nome se fornecido
            if (!string.IsNullOrEmpty(name)) 
            {
                query = query.Where(v => v.Name.ToLower().Contains(name.ToLower()));
            }
            
            // Filtrar por marca se fornecido
            if (!string.IsNullOrEmpty(mark)) 
            {
                query = query.Where(v => v.Mark.Contains(mark, StringComparison.CurrentCultureIgnoreCase));
            }
            
            // Definir o número de itens por página
            int itemsPage = 10;

            // Aplicar paginação se a página for fornecida
            if (page != null) 
            {
                query = query.Skip((int)((page - 1) * itemsPage)).Take(itemsPage);
            }

            // Retornar a lista final filtrada e/ou paginada
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