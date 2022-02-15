using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StreamFile.Contract.Service;
using StreamFile.Core.Constants;
using StreamFile.Core.Models;
using StreamFile.Core.Models.Documents;
using StreamFile.Core.Models.MomoPayment;
using StreamFile.Core.Models.TransferLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace StreamFile.WebApi.Controllers
{
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentStoreService _documentStoreService;
        private readonly ITransferLogService _transferLogService;
        private readonly IMomoPaymentService _momoPaymentService;
        private readonly ILogger _logger;

        public DocumentController(IServiceProvider serviceProvider)
        {
            _documentStoreService = serviceProvider.GetRequiredService<IDocumentStoreService>();
            _transferLogService = serviceProvider.GetRequiredService<ITransferLogService>();
            _momoPaymentService = serviceProvider.GetRequiredService<IMomoPaymentService>();
            _logger = Log.Logger;
        }

        #region RequestDownload
        [HttpPost]
        [Route(WebApiEndpoint.Documents.RequestDownload)]
        public IActionResult RequestDownload([FromBody] RequestDownloadDocModel request)
        {
            var result = new BaseModel<string>();
            var action = _transferLogService.GetlogByRefId(request.RefId);
            if (action != null)
            {
                result.Code = ResponseCodeConstants.TRANSFER_ACTION_EXISTED;
                result.Data = "Request download is existed!";
                return BadRequest(result);
            }

            var document = _documentStoreService.GetByDocumnetId(request.DocumentId);
            if (document == null)
            {
                result.Code = ResponseCodeConstants.FILE_NOT_EXIST;
                result.Data = "File is not existed!";
                return BadRequest(result);
            }

            var files = Path.Combine(document.FilePath);
            if (!System.IO.File.Exists(files))
            {
                result.Code = ResponseCodeConstants.FILE_NOT_FOUND;
                result.Data = "File is not found!";
                return BadRequest(result);
            }

            var data = _transferLogService.CreateTransferLog(request);
            var response = new BaseModel<ResponseDownloadModel> { Data = data };
            return Ok(response);
        }
        #endregion RequestDownload

        #region Upload New Doc
        [HttpPost]
        [Route(WebApiEndpoint.Documents.Upload)]
        public IActionResult Upload([FromBody] AddDocumentModel request)
        {
            var result = new BaseModel<string>();
            var fileCheck = _documentStoreService.GetByDocumnetId(request.DocumentId);
            if (fileCheck != null)
            {
                result.Code = ResponseCodeConstants.FILE_EXISTED;
                result.Message = "Document is existed!";
                return BadRequest(result);
            }

            _documentStoreService.UploadDocument(request);
            return Ok(result);
        }
        #endregion Upload New Doc

        #region Download file
        [HttpGet]
        [Route(WebApiEndpoint.Documents.DownloadFile)]
        public  IActionResult Download(string key)
        {
            var docID = _transferLogService.GetDocument(key);
            var file = _documentStoreService.DownloadFile(docID);

            File(file.FilePath, "application/octet-stream", file.FileName);
            var content = System.IO.File.ReadAllBytes(file.FilePath);
            //var JobId = BackgroundJob.Enqueue(() => File(System.IO.File.ReadAllBytes(file.FilePath), "application/octet-stream", file.FileName));
            
            return File(content, "application/octet-stream", file.FileName);
        }
        #endregion Download file

        #region CallbackPayment
        [HttpPost]
        [Route(WebApiEndpoint.Payment.Callback)]
        public IActionResult CallbackPayment(CallbackPaymentModel request)
        {
            _transferLogService.CallbackPayment(request);
            return Ok();
        }
        #endregion CallbackPayment

        [HttpGet("/test-signalr")]
        public IActionResult Test()
        {
            return Ok(_momoPaymentService.PaymentRequireService());
            //_transferLogService.CallbackPayment(new CallbackPaymentModel()); ;
            //_documentStoreService.TestSignalR();
        }
    }
}
