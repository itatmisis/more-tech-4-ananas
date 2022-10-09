using MORE_Tech.Data.Models;

namespace MORE_Tech.Data.Repositories
{
    public interface INewsSourceRespository
    {
        IQueryable<NewsSource> GetAllActive();

        IQueryable<NewsSource> Get(int count, int offset);
    }
}
