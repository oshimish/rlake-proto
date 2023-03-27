using System.ComponentModel.DataAnnotations;

namespace Rlake.Services
{
    public class RabbitMqOptions
    {
        [Required]
        public Uri Connection { get; set; }
    }
}