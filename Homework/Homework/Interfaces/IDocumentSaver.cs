namespace Moravia.Homework.Interfaces;

/// <summary>
/// Interface for the class that will save data
/// </summary>
public interface IDocumentSaver
{
    string DataTarget { get; set; }
    IDataOutput DataOutput { get; set; }
    IDataSerializer DataSerializer { get; set; }
    Task Save(IDataDocument doc);
}
