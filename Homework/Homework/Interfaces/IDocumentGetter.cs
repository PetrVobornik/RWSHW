namespace Moravia.Homework.Interfaces;

public interface IDocumentGetter
{
    string DataSource { get; set; }
    IDataInput DataInput { get; set; }
    IDataDeserializer DataDeserializer { get; set; }
    Task<IDataDocument> Load();
}
