using Microsoft.Extensions.Options;
using MORE_Tech.Data;
using MORE_Tech.Data.Models;
using MORE_Tech.Parser.Configuration;
using MORE_Tech.Parser.Interfaces;
using MORE_Tech.Parser.ParserImplementations.VKParse;

namespace MORE_Tech.Parser.ParserImplementations
{
    public class VKParser: INewsParser
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly VKSettings _config;

        public VKParser(IUnitOfWork unitOfWork, IOptions<VKSettings> config)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _config = config?.Value ??
                throw new ArgumentNullException(nameof(config));

        }

        public async Task Parse(NewsSource source)
        {
            Dictionary<string, string> keys = new Dictionary<string, string>()
            {
                ["domain"] = source.Url,
                ["count"] = _config.NewsCount.ToString(),
                ["extended"] = "1"
            };
            JsonParse parsed = new JsonParse(await Requests.Send(keys,  _config));
            Post[] posts = parsed.MakePosts((uint)source.Id, $"{_config.NewsUrl}/{source.Url}?w=wall");

            foreach (var post in posts)
            {
                if (!_unitOfWork.NewsRepository.IsExists(post.News()))
                {
                    await saveNews(post.News());
                    foreach (Attachments attachment in post.GetAttachments())
                    {
                        if(attachment!=null)
                            await saveAttachment(attachment);
                    }
                }
            }

        }

        private async Task saveNews(News news)
        {
            await _unitOfWork.NewsRepository.AddAsync(news);
            await _unitOfWork.SaveChanges();
        }

        private async Task saveAttachment(Attachments attachment)
        {
            await _unitOfWork.AttachmentsRepository.AddAsync(attachment);
            await _unitOfWork.SaveChanges();
        }
    }
}
