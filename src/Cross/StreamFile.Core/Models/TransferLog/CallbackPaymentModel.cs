using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamFile.Core.Models.TransferLog
{
    public class CallbackPaymentModel
    {
        public string PhoneNumber { get; set; }
        public string OTP { get; set; }
        public string Status { get; set; }


    }
}
