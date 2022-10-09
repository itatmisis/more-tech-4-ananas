using MORE_Tech.Data.Models;

namespace MORE_Tech.Data.Repositories
{
    public interface IAttachmentsRepository
    {
        Task AddAsync(Attachments attachments);
    }
}
