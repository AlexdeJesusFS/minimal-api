
using minimal_api.Domain.Enums;

namespace minimal_api.Domain.ModelViews
{
    public record  AdminLogged
    {
        public string Email { get; set; } = default!;
        public string Rule { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}