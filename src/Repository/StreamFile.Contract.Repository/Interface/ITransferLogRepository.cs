using StreamFile.Contract.Repository.Infrastructure;
using StreamFile.Contract.Repository.Models.TransferLog;

namespace StreamFile.Contract.Repository.Interface
{
    public interface ITransferLogRepository : IRepository<TransferLogsEntity>
    {
        TransferLogsEntity GetlogByRefId(string refId);
        TransferLogsEntity GetPaymentedlogById(string id);
        TransferLogsEntity GetlogById(string Id);
        bool Edit(TransferLogsEntity entity);
        TransferLogsEntity GetLogFromCallback(string orderId);
    }
}
