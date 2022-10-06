namespace Moravia.Homework.Interfaces;

public interface IDocumentSetter
{
    string DataTarget { get; set; }
    IDataOutput DataOutput { get; set; }
    IDataSerializer DataSerializer { get; set; }
    Task Save(IDataDocument doc);
}
