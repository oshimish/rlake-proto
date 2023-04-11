using RlakeFunctionApp.Entities;

namespace RlakeFunctionApp.Dtos
{
    public class LocationDto
    {

        [Required]
        public List<Point> Items { get; set; } = new();
    }
}