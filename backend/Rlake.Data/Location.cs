using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Rlake.Data
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


    }


}