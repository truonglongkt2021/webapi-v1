using Invedia.Data.EF.Interfaces.DbContext;
using Invedia.DI.Attributes;
using StreamFile.Contract.Repository.Interface;
using StreamFile.Contract.Repository.Models.TransferLog;
using StreamFile.Repository.Infrastructure;

namespace StreamFile.Repository
{
    [ScopedDependency(ServiceType = typeof(ITransferLogRepository))]

    public class TransferLogRepository : Repository<TransferLogEntity>, ITransferLogRepository
    {
        public TransferLogRepository(IDbContext dbContext) : base(dbContext)
        {
        }
    }
}
