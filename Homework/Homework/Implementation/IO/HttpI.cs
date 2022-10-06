using Moravia.Homework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Moravia.Homework.Implementation.IO;

internal class HttpI : IDataInput
{
    public Task<Stream> OpenSourceStream(string source)
    {
        var hc = new HttpClient();
        return hc.GetStreamAsync(source);
    }
}
