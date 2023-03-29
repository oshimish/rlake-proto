namespace Rlake.Api.Dto
{
    public class SearchResultDto
    {
        public string SearchText { get; set; }


        [Required]
        public List<Point> Items { get; set; } = new();
    }
    public class LocationDto
    {
        public string SearchText { get; set; }


        [Required]
        public List<Point> Items { get; set; } = new();
    }
}