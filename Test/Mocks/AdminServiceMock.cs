
using minimal_api.Domain.DTOs;
using minimal_api.Domain.Entities;
using minimal_api.Domain.Interfaces;

namespace Test.Mocks
{
    public class AdminServiceMock : IAdminService
    {
        private static List<Admin> admins = [
            new Admin{
                Id = 1,
                Email = "adm@test.com",
                Password = "123",
                Rule = "Adm"
            },
            new Admin{
                Id = 2,
                Email = "editor@test.com",
                Password = "123",
                Rule = "Editor"
            }
        ];

        public Admin Add(Admin admin)
        {
            admin.Id = admins.Count + 1;
            admins.Add(admin);
            return admin;
        }

        public List<Admin> GetAll(int? page)
        {
            return admins;
        }

        public Admin? GetById(int id)
        {
            return admins.Find(a => a.Id == id);
        }

        public Admin? Login(LoginDTO loginDTO)
        {
            return admins.Find(a => a.Email == loginDTO.Email && a.Password == loginDTO.Password);
        }
    }
}