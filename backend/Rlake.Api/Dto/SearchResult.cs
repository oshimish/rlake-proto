namespace Rlake.Api.Dto
{
    public class SearchResult
    {
        public string SearchText { get; set; }


        [Required]
        public List<Location> Items { get; set; } = new();
    }
}