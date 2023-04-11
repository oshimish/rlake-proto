
namespace RlakeFunctionApp.Settings
{
    public class CosmosDbSettings 
    {
        public const string CosmosDbSectionKey = "CosmosDb";

        [Required]
        public string ConnectionString { get; set; } = default!;
        
        [Required]
        public string DatabaseId { get; set; } = default!;
        
        [Required]
        public string ContainerId { get; set; } = default!;
    }
}
