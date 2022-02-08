using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using StreamFile.Contract.Repository.Interface;
using StreamFile.Contract.Repository.Models.TransferLog;
using StreamFile.Contract.Service;
using StreamFile.Core.Constants;
using StreamFile.Core.Models.Documents;
using StreamFile.Core.Utils;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StreamFile.Service
{
    [ScopedDependency(ServiceType = typeof(ITransferLogService))]
    public class TransferLogService : Base.Service, ITransferLogService
    {
        private readonly ITransferLogRepository _transferLogRepository;
        public TransferLogService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _transferLogRepository = serviceProvider.GetRequiredService<ITransferLogRepository>();
        }

        public TransferLogEntity GetlogByRefId(string refId)
        {
            var result = _transferLogRepository.GetSingle(w => w.RefId == refId);
            return result;
        }

        public Task<string> CreateAsync(TransferLogEntity model, CancellationToken cancellationToken = default)
        {

            _transferLogRepository.Add(model);
            UnitOfWork.SaveChanges();
            return Task.FromResult(model.Id);
        }

        public Task<string> CreateTransferLog(RequestDownloadDocModel request,
                                             CancellationToken cancellationToken = default)
        {
            var code = CoreHelper.RandomTokenString();
            var entity = new TransferLogEntity()
            {
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Status = StatusConstant.NEW,
                DocumentId = request.DocumentId,
                RefId = request.RefId,
                OtpToken = code,
            };
            CreateAsync(entity, cancellationToken);
            return Task.FromResult(entity.Id);
        }
    }
}
