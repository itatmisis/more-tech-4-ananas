

using System.Security.Cryptography;
using System.Text;

namespace MORE_Tech.Data.Models
{
    public class News
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string ShortText { get; set; }
        public int Views { get; set; }
        public string SourceUrl { get; set; }
        public int SourceId { get; set; }
        public virtual NewsSource Source { get; set; }
        public DateTimeOffset Date { get; set; }

        public virtual ICollection<Attachments> Attachments { get; set; }

        public News()
        {
            Attachments = new List<Attachments>();
        }

        public News(string name, string text, string sourceUrl, int views, DateTimeOffset date, int sourceId)
        {
            Id = getId(text);
            Text = text ??
                throw new ArgumentNullException(nameof(text));
            ShortText = String.Empty;
            Views = views;
            SourceId = sourceId;
            Date = date;
            SourceUrl = sourceUrl;
            Attachments = new List<Attachments>();
        }

        /// <summary>
        /// Получение Guid-а, которые основан на
        /// тексте статьи. Сделано для первичного отсеивания дубликатов.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private Guid getId(string text)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(text));
                return new Guid(hash);
            }
        }

    }
}
