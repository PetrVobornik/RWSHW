namespace Moravia.Homework.Interfaces;

public interface IDataOutput
{
    Task<Stream> OpenTargetStream(string target);
}
