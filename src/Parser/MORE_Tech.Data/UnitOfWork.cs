using MORE_Tech.Data.Repositories;

namespace MORE_Tech.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsDbContext _context;
        public INewsRepository NewsRepository { get; }
        public IAttachmentsRepository AttachmentsRepository { get; }
        public INewsSourceRespository NewsSourceRespository { get; }

        public UnitOfWork(NewsDbContext context, INewsRepository newsRepository,
            IAttachmentsRepository attachmentsRepository,
            INewsSourceRespository newsSourceRespository)
        {
            _context = context;
            NewsRepository = newsRepository;
            AttachmentsRepository = attachmentsRepository;
            NewsSourceRespository = newsSourceRespository;
        }

        public async Task<int> SaveChanges()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
