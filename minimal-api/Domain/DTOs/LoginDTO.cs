
namespace minimal_api.Domain.DTOs
{
    public class LoginDTO
    {
        public string email { get; set; } = default!;
        public string password { get; set; } = default!;
    }
}