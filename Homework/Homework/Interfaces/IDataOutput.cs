namespace Moravia.Homework.Interfaces;

/// <summary>
/// Interface for classes that will allow data to be stored in 
/// the storage of the specified path (target)
/// </summary>
public interface IDataOutput
{
    Task<Stream> OpenTargetStream(string target);
}
