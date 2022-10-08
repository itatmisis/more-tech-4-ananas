

using MORE_Tech.Data.Models;

namespace MORE_Tech.Data.Repositories
{
    public interface INewsRepository
    {
        Task AddAsync(News news);

        bool IsExists(News news);
    }
}
