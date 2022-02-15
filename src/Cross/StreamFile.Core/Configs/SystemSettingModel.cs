using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace StreamFile.Core.Configs
{
    public class SystemSettingModel
    {
        private string _assetsRootUrl;
        public static SystemSettingModel Instance { get; set; }
        public static IConfiguration Configs { get; set; }
        public string ApplicationName { get; set; } = Assembly.GetEntryAssembly()?.GetName().Name;

        public Guid EncryptKey { get; set; }

        public string Domain { get; set; }

        // public DateTimeOffset StartUpTime { get; set; } = CoreHelper.SystemTimeNow;

        public string VersionName { get; set; }

        public int AuthorizeCodeStorageSeconds { get; set; } = 600;

        public int AccessTokenJwtExpirationSeconds { get; set; } = 1800;

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string CookieAccessTokenKey { get; set; } = "AccessToken";

        public string CookieRefreshTokenKey { get; set; } = "RefreshToken";

        public string CookieEncryptedKey { get; set; } = "b1a4e68f03c54740bbd7b49d6b993d46";

        public string AssetsDomain
        {
            get => _assetsRootUrl;
            set => _assetsRootUrl = value?.Trim(' ', '/', '\\');
        }

        public bool IsEnforceHttps { get; set; }

        public string SecretKey { get; set; }
        public int MaxDownload { get; set; }

        //public QueueInfo ActionTransLogQueue { get; set; }

        //public QueueInfo FundsTransferQueue { get; set; }
        //public QueueInfo UserQueue { get; set; }
        //public QueueInfo BetsSummaryQueue { get; set; }
        //public QueueInfo CommissionQueue { get; set; }
        //public QueueInfo CommissionLogsQueue { get; set; }
        //public QueueInfo CalcCommissionQueue { get; set; }
    }

    public class QueueInfo
    {
        public string QueueName { get; set; }

        public string QueueTopic { get; set; }

        public int MaxWorker { get; set; }
    }

    public class MomoSettingModel
    {
        public static MomoSettingModel Instance { get; set; }
        public string BaseUrl { get; set; }
        public string AccessKey { get; set; }
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string StoreId { get; set; }
        public string IpnUrl { get; set; }
        public string RedirectUrl { get; set; }
        public string SecretKey { get; set; }

    }


}