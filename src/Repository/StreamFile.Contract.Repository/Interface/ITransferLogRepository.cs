using StreamFile.Contract.Repository.Infrastructure;
using StreamFile.Contract.Repository.Models.TransferLog;

namespace StreamFile.Contract.Repository.Interface
{
    public interface ITransferLogRepository : IRepository<TransferLogEntity>
    {
    }
}
