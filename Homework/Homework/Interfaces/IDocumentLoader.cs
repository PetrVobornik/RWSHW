namespace Moravia.Homework.Interfaces;

/// <summary>
/// Interface for the class that will load the data
/// </summary>
public interface IDocumentLoader
{
    string DataSource { get; set; }
    IDataInput DataInput { get; set; }
    IDataDeserializer DataDeserializer { get; set; }
    Task<IDataDocument> Load();
}
