using MORE_Tech.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORE_Tech.Data.Repositories
{
    public class NewsSourceRepository : INewsSourceRespository
    {

        private readonly NewsDbContext _context;
        public NewsSourceRepository(NewsDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }
        public IQueryable<NewsSource> Get(int count, int offset)
        {
            return _context.NewsSources
                .Skip(offset)
                .Take(count);
        }

        public IQueryable<NewsSource> GetAllActive()
        {
            return _context.NewsSources
                .Where(x => x.IsActive);
        }
    }
}
