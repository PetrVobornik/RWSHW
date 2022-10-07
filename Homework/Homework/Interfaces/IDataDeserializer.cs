namespace Moravia.Homework.Interfaces;

/// <summary>
/// Interface for classes that will allow data deserialization
/// </summary>
public interface IDataDeserializer
{
    Task<IDataDocument> Deserialize(Stream source);
}
