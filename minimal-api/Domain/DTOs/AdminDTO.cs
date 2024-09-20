
using minimal_api.Domain.Enums;

namespace minimal_api.Domain.DTOs
{
    public class AdminDTO
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public Rule? Rule { get; set; } = default!;
    }
}