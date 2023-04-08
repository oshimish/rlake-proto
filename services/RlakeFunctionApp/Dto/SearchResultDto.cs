namespace Rlake.Api.Dto
{
    public class SearchResultDto
    {
        public string SearchText { get; set; } = "";

        [Required]
        public Conversation Conversation { get; set; } = default!;

        [Required]
        public List<Point> Items { get; set; } = new();
    }
}