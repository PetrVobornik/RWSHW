using Moravia.Homework.Interfaces;

namespace Moravia.Homework.Implementation.IO;

public class FileIO : IDataInput, IDataOutput
{
    public Stream OpenSourceStream(string source)
    {
        if (!File.Exists(source))
            throw new FileNotFoundException($"File '{source}' not found", source);

        return File.Open(source, FileMode.Open, FileAccess.Read);
    }

    public Stream OpenTargetStream(string target)
        => new FileStream(target, FileMode.Create, FileAccess.Write);
}
