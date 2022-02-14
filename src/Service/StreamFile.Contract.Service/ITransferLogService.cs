using StreamFile.Contract.Repository.Models.TransferLog;
using StreamFile.Core.Models.Documents;
using StreamFile.Core.Models.TransferLog;
using System.Threading;
using System.Threading.Tasks;

namespace StreamFile.Contract.Service
{
    public interface ITransferLogService
    {
        TransferLogEntity GetlogByRefId(string refId);
        ResponseDownloadModel CreateTransferLog(RequestDownloadDocModel request);
        string GetDocument(string id);
        void CallbackPayment(CallbackPaymentModel model);
        void DemoJob();
        void LogHub(TransferLogEntity entity);
    }
}
