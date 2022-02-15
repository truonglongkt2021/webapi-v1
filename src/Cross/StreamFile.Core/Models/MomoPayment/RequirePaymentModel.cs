using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamFile.Core.Models.MomoPayment
{
    public class RequirePaymentModel
    {
        public string partnerCode { get; set; }
        public string partnerName { get; set; }
        public string storeId { get; set; }
        public string requestId { get; set; }
        public long amount { get; set; }
        public string orderId { get; set; }
        public string orderInfo { get; set; }
        public string redirectUrl { get; set; }
        public string ipnUrl { get; set; }
        public string partnerClientId { get; set; }
        public string requestType { get; set; }
        public string extraData { get; set; }
        public ProductInfo productInfo { get; set; }
        public UserInfo userInfo { get; set; }
        public bool autoCapture { get; set; }
        public string lang { get; set; }
        public string signature { get; set; }
    }

    public class ProductInfo
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }

    public class UserInfo
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
