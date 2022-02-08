using System;
using System.Collections.Generic;
using System.Text;

namespace StreamFile.Core.Constants
{
    public class ApplicationConstant
    {
        public const string FileContentType = "application/octet-stream";
    }
    public class StatusConstant
    {
        public const string NEW = "New";
        public const string SENT = "Sent";
        public const string SUCCESS = "Success";
        public const string DONE = "Done";
    }
    public class ResponseCodeConstants
    {
        public const string FILE_EXISTED = "FILE_IS_EXIST";
        public const string FILE_NOT_EXIST = "FILE_IS_NOT_EXIST";
        public const string FILE_NOT_FOUND = "FILE_IS_NOT_FOUND";
        public const string TRANSFER_ACTION_EXISTED = "TRANSFER_ACTION_IS_EXISTED";
        public const string TRANSFER_ACTION_NOT_EXIST = "TRANSFER_ACTION_IS_NOT_EXIST";

    }
}
