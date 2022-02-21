using Microsoft.EntityFrameworkCore;
using StreamFile.Contract.Repository.Models.TransferLog;
using StreamFile.Contract.Repository.Models.DocumentStore;

namespace StreamFile.Repository.Infrastructure
{
    public sealed partial class AppDbContext
    {
        public DbSet<DocumentStoresEntity> DocumentStore { get; set; }
        public DbSet<TransferLogsEntity> TransferLog { get; set; }

    }
}