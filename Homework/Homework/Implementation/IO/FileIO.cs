using Moravia.Homework.Interfaces;

namespace Moravia.Homework.Implementation.IO;

public class FileIO : IDataInput, IDataOutput
{
    public Task<Stream> OpenSourceStream(string source)
    {
        if (!File.Exists(source))
            throw new FileNotFoundException($"File '{source}' not found", source);

        return Task.FromResult<Stream>(File.Open(source, FileMode.Open, FileAccess.Read));
    }

    public Task<Stream> OpenTargetStream(string target)
        => Task.FromResult<Stream>(new FileStream(target, FileMode.Create, FileAccess.Write));
}
