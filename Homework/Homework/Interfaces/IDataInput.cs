namespace Moravia.Homework.Interfaces;

/// <summary>
/// Interfaces for classes that will be able to retrieve data from 
/// the storage of the specified path (source)
/// </summary>
public interface IDataInput
{
    Task<Stream> OpenSourceStream(string source);
}


