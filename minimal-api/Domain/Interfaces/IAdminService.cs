
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;

namespace minimal_api.Domain.Interfaces
{
    public interface IAdminService
    {
        Admin? Login(LoginDTO loginDTO);
        Admin Add(Admin admin);
        List<Admin> GetAll(int? page);
        Admin? GetById(int id);
    }
}