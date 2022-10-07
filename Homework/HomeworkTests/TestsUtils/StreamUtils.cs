using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeworkTests.TestsUtils
{
    internal static class StreamUtils
    {
        public static Stream StringToStream(string s)
        {
            var ms = new MemoryStream();
            var sw = new StreamWriter(ms);
            sw.Write(s);
            sw.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public static string StreamToString(Stream stream)
        {
            var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }
    }
}
