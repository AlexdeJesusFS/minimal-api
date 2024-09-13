
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

        public Admin? Login(LoginDTO loginDTO)
        {
            return (_context.Adimins.Where(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password).FirstOrDefault());
        }
    }
}