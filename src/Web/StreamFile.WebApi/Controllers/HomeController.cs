using Invedia.Core.EnvUtils;
using Invedia.Data.IO;
using Invedia.Data.IO.DirectoryUtils;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using StreamFile.Core.Constants;
using System;
using System.IO;

namespace StreamFile.WebApi.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ILogger _logger;

        public HomeController(IServiceProvider serviceProvider)
        {
            _logger = Log.Logger;
        }

        public IActionResult Index()
        {
            var info = $"Service is running normally on {EnvHelper.CurrentEnvironment}...";
            return Ok(info);
        }

        [HttpGet("/version")]
        public IActionResult GetVersion()
        {
            var version = typeof(Program).Assembly.GetName().Version?.ToString();
            _logger.Information($"Version = {version}");
            return Ok(version);
        }

        [HttpGet("/test")]
        public ActionResult Download()
        {
            var pathRoot = DirectoryHelper.SpecialFolder.GetCurrentWindowUserFolder();
            var folderLocation = Path.Combine(pathRoot,Locations.SavePath);
            DirectoryHelper.CreateIfNotExist(PathHelper.GetFullPath(folderLocation));
            var file = Path.Combine(folderLocation, "LaptopList.xlsx");
            return File(System.IO.File.ReadAllBytes(file),"application/octet-stream", "FileDownload.xlsx");
        }


    }
}