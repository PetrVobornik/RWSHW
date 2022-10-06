﻿using Moravia.Homework.Interfaces;
using Moravia.Homework.ProcessClasses.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Moravia.Homework.Implementation.IO;

[DataChanger("HTTP")]
internal class HttpI : IDataInput
{
    public Task<Stream> OpenSourceStream(string source)
    {
        var hc = new HttpClient();
        return hc.GetStreamAsync(source);
    }
}
