using StreamFile.Contract.Repository.Infrastructure;
using StreamFile.Contract.Repository.Models.TransferLog;

namespace StreamFile.Contract.Repository.Interface
{
    public interface ITransferLogRepository : IRepository<TransferLogEntity>
    {
        TransferLogEntity GetlogByRefId(string refId);
        TransferLogEntity GetPaymentedlogById(string id);
        TransferLogEntity GetlogById(string Id);
        bool Edit(TransferLogEntity entity);
        TransferLogEntity GetLogFromCallback(string numberPhone, string otpCode);
    }
}
