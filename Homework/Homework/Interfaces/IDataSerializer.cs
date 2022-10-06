namespace Moravia.Homework.Interfaces;

public interface IDataSerializer
{
    Task Serialize(IDataDocument document, Stream target);
}
