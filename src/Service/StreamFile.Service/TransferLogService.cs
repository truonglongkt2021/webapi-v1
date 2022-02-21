using Invedia.DI.Attributes;
using Microsoft.AspNetCore.Http;
//using Invedia.Web.Middlewares.HttpContextMiddleware;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using StreamFile.Contract.Repository.Interface;
using StreamFile.Contract.Repository.Models.TransferLog;
using StreamFile.Contract.Service;
using StreamFile.Core.Configs;
using StreamFile.Core.Constants;
using StreamFile.Core.Exceptions;
using StreamFile.Core.Models.Documents;
using StreamFile.Core.Models.TransferLog;
using StreamFile.Core.Utils;
using StreamFile.Service.Hub;
using System;
using System.Net.NetworkInformation;

namespace StreamFile.Service
{
    [ScopedDependency(ServiceType = typeof(ITransferLogService))]
    public class TransferLogService : Base.Service, ITransferLogService
    {
        private readonly ITransferLogRepository _transferLogRepository;
        private readonly IHubContext<DocHub, IDocHub> _hub;
        private readonly ILogger _logger;
        public TransferLogService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _transferLogRepository = serviceProvider.GetRequiredService<ITransferLogRepository>();
            _hub = serviceProvider.GetRequiredService<IHubContext<DocHub, IDocHub>>();
            _logger = Log.Logger;
        }

        #region GetlogByRefId
        // Get Transaction log follow refID(Id được map với Id client)
        public TransferLogsEntity GetlogByRefId(string refId)
        {
            var result = _transferLogRepository.GetlogByRefId(refId);
            return result;
        }
        #endregion GetlogByRefId

        #region CreateTransferLog
        // Create new transaction log
        public ResponseDownloadModel CreateTransferLog(RequestDownloadDocModel request)
        {
            var code = CoreHelper.RandomTokenString();
            var entity = new TransferLogsEntity()
            {
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Status = StatusConstant.NEW,
                DocumentId = request.DocumentId,
                RefId = request.RefId,
                OtpToken = code,
            };
            var addDB = _transferLogRepository.Insert(entity);
            var result = new ResponseDownloadModel
            {
                OtpToken = addDB ? code : null
            };
            //_hub.Clients.All.SendAsync("linkdoc", request.PhoneNumber, "");
            return result;
        }
        #endregion CreateTransferLog

        #region GetDocument
        //check all policy before download file
        public string GetDocument(string id)
        {
            var transLog = _transferLogRepository.GetPaymentedlogById(id);
            if (transLog == null) throw new AppException("Cannot found history transaction!");

            if (transLog.TotalDownload > SystemSettingModel.Instance.MaxDownload)
                throw new AppException("over limited!");

            if (transLog.TimeExpired < CoreHelper.SystemTimeNow)
                throw new AppException("over expire time!");

            return transLog.DocumentId;
        }
        #endregion GetDocument

        #region CallbackPayment
        // hookup from SMS payment
        public void CallbackPayment(CallbackPaymentModel model)
        {
            var transLog = _transferLogRepository.GetLogFromCallback(model.orderId);
            if (transLog == null)
            {
                // làm cai service ghi log lại sau
                _logger.Information("Callback Info: Can not found order in DB");
                return;
            }
            transLog.Status = StatusConstant.PAYMENT;

            //var link = CreateLinkDownload("ss");//transLog.Id
            transLog.Email = JsonConvert.SerializeObject(model);
            _transferLogRepository.Edit(transLog);

        }

        private string CreateLinkDownload(string key) // key là id cua transferLog
        {
            var domain = SystemSettingModel.Instance.Domain;
            var link = $"{domain}/api/document/download-file?key={key}";
            return link;
        }
        #endregion CallbackPayment

        public void DemoJob()
        {
            _hub.Clients.All.LinkDoc("phu", "phu oc cho");
        }

        public void LogHub(TransferLogsEntity entity)
        {
            _transferLogRepository.Insert(entity);
        }
    }
}
