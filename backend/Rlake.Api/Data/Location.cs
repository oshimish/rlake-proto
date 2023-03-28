using System.ComponentModel.DataAnnotations;

namespace Rlake.Api.Data
{
    public class Location
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;


        //[Required]
        public string? Point { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }



    public class Geodata
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}