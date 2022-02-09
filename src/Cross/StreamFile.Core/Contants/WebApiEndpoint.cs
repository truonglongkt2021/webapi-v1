using System;
using System.Collections.Generic;
using System.Text;

namespace StreamFile.Core.Constants
{
    public static class WebApiEndpoint
    {
        public const string AreaName = "api";

        public class Documents
        {
            private const string BaseEndpoint = "~/" + AreaName + "/document";
            public const string RequestDownload = BaseEndpoint + "/download-request";
            public const string DownloadFile = BaseEndpoint + "/download-file";
            public const string Upload = BaseEndpoint + "/upload";
        }

        public class Payment
        {
            private const string BaseEndpoint = "~/" + AreaName + "/payment";
            public const string Callback = BaseEndpoint + "/callback";
        }
    }
}