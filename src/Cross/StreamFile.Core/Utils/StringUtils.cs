using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamFile.Core.Utils
{
    public static class StringUtils
    {
        public static string PlainTextToBase64(string value) => BytesToBase64(Encoding.UTF8.GetBytes(value));

        public static string BytesToBase64(byte[] bytes) => Convert.ToBase64String(bytes);

        public static string BytesToPlainText(byte[] bytes) => Encoding.UTF8.GetString(bytes);

        public static string Base64ToPlainText(string value) => Encoding.UTF8.GetString(Base64ToBytes(value));

        public static byte[] Base64ToBytes(string value) => Convert.FromBase64String(value);
    }
}
