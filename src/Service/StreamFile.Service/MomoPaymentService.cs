using Invedia.Core.SecurityUtils;
using Invedia.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using StreamFile.Contract.Repository.Interface;
using StreamFile.Contract.Service;
using StreamFile.Core.Configs;
using StreamFile.Core.Constants;
using StreamFile.Core.Models.MomoPayment;
using StreamFile.Core.Utils;
using System;

namespace StreamFile.Service
{
    [ScopedDependency(ServiceType = typeof(IMomoPaymentService))]
    public class MomoPaymentService : Base.Service, IMomoPaymentService
    {
        private readonly ILogger _logger;
        private readonly ITransferLogRepository _transferLogRepository;

        public MomoPaymentService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _logger = Log.Logger;
            _transferLogRepository = serviceProvider.GetRequiredService<ITransferLogRepository>();
        }

        public string PaymentRequireService()
        {
            var DataInfo = new ProductInfo
            {
                Name = "test"
            };
            var JsonData = JsonConvert.SerializeObject(DataInfo);
            var extraData = StringUtils.PlainTextToBase64(JsonData);

            var signatureValue = $"accessKey={MomoSettingModel.Instance.AccessKey}&" +
                                 $"amount=10000&" +
                                 $"extraData={extraData}&" +
                                 $"ipnUrl={MomoSettingModel.Instance.IpnUrl}&" +
                                 $"orderId=98350e6b12934849887443547be96ecf&" +
                                 $"orderInfo=Test order&" +
                                 $"partnerCode={MomoSettingModel.Instance.PartnerCode}&" +
                                 $"redirectUrl ={MomoSettingModel.Instance.RedirectUrl}&" +
                                 $"requestId=98350e6b12934849887443547be96ecf&" +
                                 $"requestType =payWithMethod";
            var signature = SecurityHelper.EncryptHmacSha256(signatureValue, MomoSettingModel.Instance.SecretKey);

            var model = new RequirePaymentModel
            {
                partnerCode = MomoSettingModel.Instance.PartnerCode,
                partnerName = MomoSettingModel.Instance.PartnerName,
                storeId = MomoSettingModel.Instance.StoreId,
                requestId = "98350e6b12934849887443547be96ecf",
                amount = 10000,
                orderId = "98350e6b12934849887443547be96ecf",
                requestType = "payWithMethod",
                redirectUrl = MomoSettingModel.Instance.RedirectUrl,
                ipnUrl = MomoSettingModel.Instance.IpnUrl,
                lang = LanguageCode.Vietname,
                orderInfo = "Test order",
                extraData = extraData,
                signature = signature
            };

            var Client = new RestClient(MomoSettingModel.Instance.BaseUrl);
            var request = new RestRequest("v2/gateway/api/create", Method.POST)
                                    .AddHeader("Content-Type", "application/json; charset=UTF-8")
                                    .AddJsonBody(model);
            var response = Client.Execute(request);
            //if (!response.IsSuccessful)
            //{
            //    return response.ErrorMessage;
            //}
            return response.Content;
        }
    }
}
