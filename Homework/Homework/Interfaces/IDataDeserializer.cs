namespace Moravia.Homework.Interfaces;

public interface IDataDeserializer
{
    Task<IDataDocument> Deserialize(Stream source);
}
