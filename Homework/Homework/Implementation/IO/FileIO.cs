using Moravia.Homework.Interfaces;
using Moravia.Homework.ProcessClasses.Reflection;

namespace Moravia.Homework.Implementation.IO;

/// <summary>
/// Class for reading and saving data from/to a local file
/// </summary>
[DataChanger("FILE")]
public class FileIO : IDataInput, IDataOutput
{
    public FileIO() { }

    public Task<Stream> OpenSourceStream(string source)
    {
        if (!File.Exists(source))
            throw new FileNotFoundException($"File '{source}' not found", source);

        return Task.FromResult<Stream>(File.Open(source, FileMode.Open, FileAccess.Read));
    }

    public Task<Stream> OpenTargetStream(string target)
        => Task.FromResult<Stream>(new FileStream(target, FileMode.Create, FileAccess.Write));
}
