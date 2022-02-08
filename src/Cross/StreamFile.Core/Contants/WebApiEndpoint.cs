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
            public const string Download = BaseEndpoint + "/download";
            public const string Upload = BaseEndpoint + "/upload";

        }

        public class App
        {
            private const string BaseEndpoint = "~/" + AreaName + "/app";
            public const string AddEndpoint = BaseEndpoint + "/add";
        }
    }
}