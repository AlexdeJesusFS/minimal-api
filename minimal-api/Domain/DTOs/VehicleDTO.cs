
namespace minimal_api.Domain.DTOs
{
    public record VehicleDTO
    {
        public string Name { get; set; } = default!;
        public string Mark { get; set; } = default!;
        public int Year { get; set; } = default!;

    }
}