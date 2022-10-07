namespace Moravia.Homework.Interfaces;

/// <summary>
/// Interface for classes that will allow data serialization
/// </summary>
public interface IDataSerializer
{
    Task Serialize(IDataDocument document, Stream target);
}
