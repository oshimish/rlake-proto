namespace Rlake.Api.Dto
{
    public class LocationDto
    {

        [Required]
        public List<Point> Items { get; set; } = new();
    }
}