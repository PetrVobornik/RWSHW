using Moravia.Homework.Interfaces;

namespace Moravia.Homework.Implementation.Doc;

public class DocumentWorker : IDocumentWorker
{
    #region Properties

    public string DataSource { get; set; }
    public IDataInput DataInput { get; set; }
    public IDataDeserializer DataDeserializer { get; set; }

    public string DataTarget { get; set; }
    public IDataSerializer DataSerializer { get; set; }
    public IDataOutput DataOutput { get; set; }

    void CheckParams(bool forGet = false, bool forSet = false)
    {
        if (forGet)
        {
            if (string.IsNullOrWhiteSpace(DataSource)) throw new ArgumentNullException(nameof(DataSource));
            if (DataInput is null) throw new ArgumentNullException(nameof(DataInput));
            if (DataDeserializer is null) throw new ArgumentNullException(nameof(DataDeserializer));
        }

        if (forSet)
        {
            if (string.IsNullOrWhiteSpace(DataTarget)) throw new ArgumentNullException(nameof(DataSource));
            if (DataSerializer is null) throw new ArgumentNullException(nameof(DataSerializer));
            if (DataOutput is null) throw new ArgumentNullException(nameof(DataOutput));
        }
    }

    #endregion

    #region Get and Set

    public async Task<IDataDocument> Load()
    {
        CheckParams(forGet: true);
        using (var sourceStream = await DataInput.OpenSourceStream(DataSource))
            return await DataDeserializer.Deserialize(sourceStream);
    }

    public async Task Save(IDataDocument doc)
    {
        CheckParams(forSet: true);
        using (var targetStream = await DataOutput.OpenTargetStream(DataTarget))
            await DataSerializer.Serialize(doc, targetStream);
    }

    public async Task LoadAndSave()
    {
        CheckParams(forSet: true); // Don't load if it won't save
        var doc = await Load();
        await Save(doc);
    }

    #endregion

    #region static versions of methods

    public static async Task<IDataDocument> Load(
        string dataSource,
        IDataInput dataLoader,
        IDataDeserializer dataReader)
    {
        var worker = new DocumentWorker
        {
            DataSource = dataSource,
            DataInput = dataLoader,
            DataDeserializer = dataReader,
        };
        return await worker.Load();
    }

    public static async Task Save(
        IDataDocument doc,
        string dataTarget,
        IDataSerializer dataWriter,
        IDataOutput dataSaver)
    {
        var worker = new DocumentWorker
        {
            DataTarget = dataTarget,
            DataSerializer = dataWriter,
            DataOutput = dataSaver,
        };
        await worker.Save(doc);
    }

    public static async Task LoadAndSave(
        string dataSource,
        IDataInput dataLoader,
        IDataDeserializer dataReader,
        string dataTarget,
        IDataSerializer dataWriter,
        IDataOutput dataSaver)
    {
        var worker = new DocumentWorker
        {
            DataSource = dataSource,
            DataInput = dataLoader,
            DataDeserializer = dataReader,
            DataTarget = dataTarget,
            DataSerializer = dataWriter,
            DataOutput = dataSaver,
        };
        await worker.LoadAndSave();
    }

    #endregion
}
