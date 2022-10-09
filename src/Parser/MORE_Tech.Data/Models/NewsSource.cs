using MORE_Tech.Data.Models.Enums;

namespace MORE_Tech.Data.Models
{
    /// <summary>
    /// Источник новости (новостной ресурс)
    /// </summary>
    public class NewsSource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public SourceTypes Type { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<News> News { get; set; }

        public NewsSource()
        {
            News = new List<News>();
        }
    }
}
