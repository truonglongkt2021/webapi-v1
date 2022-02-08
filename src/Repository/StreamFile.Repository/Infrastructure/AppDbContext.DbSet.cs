using Microsoft.EntityFrameworkCore;
using StreamFile.Contract.Repository.Models.TransferLog;
using StreamFile.Contract.Repository.Models.DocumentStore;

namespace StreamFile.Repository.Infrastructure
{
    public sealed partial class AppDbContext
    {
        public DbSet<DocumentStoreEntity> DocumentStore { get; set; }
        public DbSet<TransferLogEntity> TransferLog { get; set; }

    }
}