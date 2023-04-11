namespace RlakeFunctionApp.Entities
{
    /// <summary>
    /// An file uploaded to the blob.
    /// </summary>

    public class Upload
    {
        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;


        public string FileName { get; set; }
        public string ContentType { get; set; }

        [Required]
        public string BlobPath { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}