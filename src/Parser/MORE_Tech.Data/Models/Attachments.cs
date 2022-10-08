using MORE_Tech.Data.Models.Enums;

namespace MORE_Tech.Data.Models
{
    public class Attachments
    {
        public  int Id { get; set; }
        public string Url { get; set; }
        public Guid NewsId { get; set; }
        public AttachmentTypes Type { get; set; }
        public virtual News News { get; set; }

        public Attachments()
        {

        }

        public Attachments(string url, AttachmentTypes type)
        {
            Url = url;
            Type = type;
        }
    }
}
