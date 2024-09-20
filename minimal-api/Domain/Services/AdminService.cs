
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;
using minimal_api.Infrastructure.Db;

namespace minimal_api.Domain.Services
{
    public class AdminService : IAdminService
    {
        private readonly DataBaseContext _context;

        public AdminService(DataBaseContext context)
        {
            _context = context;
        }

        public Admin Add(Admin admin)
        {
            _context.Adimins.Add(admin);
            _context.SaveChanges();
            return admin;
        }

        public List<Admin> GetAll(int? page)
        {
            var query = _context.Adimins.AsQueryable();
            
            int itemsPage = 10;

            if (page != null) {
                return query.Skip((int)((page - 1) * itemsPage)).Take(itemsPage).ToList();
            }
            return query.ToList();
        }

        public Admin? GetById(int id)
        {
            return _context.Adimins.Where(a => a.Id == id).FirstOrDefault();
        }

        public Admin? Login(LoginDTO loginDTO)
        {
        var adm = _context.Adimins.Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password).FirstOrDefault();
        return adm;        
        }
    }
}