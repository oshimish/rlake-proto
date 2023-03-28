using System.ComponentModel.DataAnnotations;

namespace Rlake.Api.Options
{
    public class RabbitMqOptions
    {
        [Required]
        public Uri Connection { get; set; }
    }
}