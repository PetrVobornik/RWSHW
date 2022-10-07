using Moravia.Homework.Interfaces;
using Moravia.Homework.ProcessClasses.Reflection;

namespace Moravia.Homework.Implementation.IO;

/// <summary>
/// Class for reading data from a web source
/// </summary>
[DataChanger("HTTP")]
internal class HttpI : IDataInput
{
    public Task<Stream> OpenSourceStream(string source)
    {
        var hc = new HttpClient();
        return hc.GetStreamAsync(source);
    }
}
