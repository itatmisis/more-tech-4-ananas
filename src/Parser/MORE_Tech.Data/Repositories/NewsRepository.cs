using MORE_Tech.Data.Models;

namespace MORE_Tech.Data.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private readonly NewsDbContext _context;

        public NewsRepository(NewsDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(News news)
        {
            if(!_context.News.Any(x => x.Id == news.Id))
            {
                await _context.News.AddAsync(news);
            }

        }

        public bool IsExists(News news)
        {
            return _context.News.Any(x => x.Id == news.Id);
        }
    }
}
