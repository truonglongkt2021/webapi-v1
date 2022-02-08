using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StreamFile.Contract.Service;
using StreamFile.Core.Constants;
using StreamFile.Core.Models;
using StreamFile.Core.Models.Documents;
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
        private readonly ILogger _logger;

        public DocumentController(IServiceProvider serviceProvider)
        {
            _documentStoreService = serviceProvider.GetRequiredService<IDocumentStoreService>();
            _transferLogService = serviceProvider.GetRequiredService<ITransferLogService>();
            _logger = Log.Logger;
        }

        [HttpPost]
        [Route(WebApiEndpoint.Documents.Download)]
        public IActionResult Download([FromBody] RequestDownloadDocModel request)
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

            _transferLogService.CreateTransferLog(request);
            return Ok(result);
        }

        [HttpPost]
        [Route(WebApiEndpoint.Documents.Upload)]
        public async Task<IActionResult> Upload([FromBody] AddDocumentModel request)
        {
            var result = new BaseModel<string>();
            var fileCheck = _documentStoreService.GetByDocumnetId(request.DocumentId);
            if (fileCheck != null)
            {
                result.Code = ResponseCodeConstants.FILE_EXISTED;
                result.Message = "Document is existed!";
                return BadRequest(result);
            }

            await _documentStoreService.UploadDocument(request);
            return Ok(result);
        }

        //[HttpGet("/download")]
        //public ActionResult Test()
        //{
        //    var a = "ssdsd";
        //    _documentStoreService.GetByDocumnetId(a);
        //    return Ok("s");
        //}
    }
}
