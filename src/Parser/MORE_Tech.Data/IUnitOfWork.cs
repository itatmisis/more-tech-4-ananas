using MORE_Tech.Data.Repositories;

namespace MORE_Tech.Data
{
    public interface IUnitOfWork
    {
        INewsRepository NewsRepository { get; }
        IAttachmentsRepository AttachmentsRepository { get; }
        INewsSourceRespository NewsSourceRespository { get; }

        Task<int> SaveChanges();
    }
}
