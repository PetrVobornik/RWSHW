namespace Moravia.Homework.Interfaces;

public interface IDataInput
{
    Task<Stream> OpenSourceStream(string source);
}


