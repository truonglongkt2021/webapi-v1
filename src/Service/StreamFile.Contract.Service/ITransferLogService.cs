using StreamFile.Contract.Repository.Models.TransferLog;
using StreamFile.Core.Models.Documents;
using System.Threading;
using System.Threading.Tasks;

namespace StreamFile.Contract.Service
{
    public interface ITransferLogService :
        Base.ICreateable<TransferLogEntity, string>
    {
        public TransferLogEntity GetlogByRefId(string refId);
        public Task<string> CreateTransferLog(RequestDownloadDocModel request,
                                     CancellationToken cancellationToken = default);
    }
}
