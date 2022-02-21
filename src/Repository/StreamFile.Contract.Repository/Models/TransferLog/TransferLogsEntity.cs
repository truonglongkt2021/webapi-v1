using StreamFile.Core.Utils;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamFile.Contract.Repository.Models.TransferLog
{
    [Table("TransferLogs")]
    public class TransferLogsEntity : Entity
    {
        public TransferLogsEntity()
        {
            TimeExpired = CoreHelper.SystemTimeNow.AddHours(6);
        }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string DocumentId { get; set; }
        public string OtpToken { get; set; }
        public string Status { get; set; }
        public DateTimeOffset? TimeExpired { get; set; }
        public string RefId { get; set; }
        public int TotalDownload { get; set; }
    }
}
