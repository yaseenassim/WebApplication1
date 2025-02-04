namespace WebApplication1.Models
{
    public class Attachment
    {
        public int Id { get; set; }

        public string OriginalFileName { get; set; } = "";

        public string StorageFileName { get; set; } = "";

        //Navigation to contacts
        public int ContactId { get; set; }
    }
}
