using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StreamFile.Contract.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamFile.Service.Job
{
    public interface IDemoJob
    {
        void Execute();
    }
    public class DemoJob : IDemoJob
    {
        private readonly ITransferLogService _transferLogService;
        private readonly ILogger _logger;
        public DemoJob(IServiceProvider serviceProvider)
        {
            _logger = Log.Logger;
            _transferLogService = serviceProvider.GetRequiredService<ITransferLogService>();
        }
        public void Execute()
        {
            _logger.Information($"START {nameof(DemoJob)} at {DateTimeOffset.UtcNow}");
            _transferLogService.DemoJob();
        }
    }
}
