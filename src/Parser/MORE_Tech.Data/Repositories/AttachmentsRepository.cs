using MORE_Tech.Data.Models;

namespace MORE_Tech.Data.Repositories
{
    public class AttachmentsRepository : IAttachmentsRepository
    {
        private readonly NewsDbContext _context;

        public AttachmentsRepository(NewsDbContext context)
        {
            _context = context ??
                throw new ArgumentNullException(nameof(context));
        }
        public async Task AddAsync(Attachments attachments)
        {
            await _context.Attachments.AddAsync(attachments);
        }
    }
}
