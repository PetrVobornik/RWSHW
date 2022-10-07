using Moravia.Homework.Interfaces;

namespace Moravia.Homework.ProcessClasses.Document;

/// <summary>
/// Processing the conversion from the source storage and format to the target format and storage
/// </summary>
internal class DocumentWorker : IDocumentWorker
{
    #region Properties

    public string DataSource { get; set; }
    public IDataInput DataInput { get; set; }
    public IDataDeserializer DataDeserializer { get; set; }

    public string DataTarget { get; set; }
    public IDataSerializer DataSerializer { get; set; }
    public IDataOutput DataOutput { get; set; }

    /// <summary>
    /// Checking that the values are filled in for the required properties
    /// </summary>
    /// <param name="forLoad">Check the values needed to load the data</param>
    /// <param name="forSave">Check the values needed to save the data</param>
    /// <exception cref="ArgumentNullException">Value not filled in</exception>
    void CheckParams(bool forLoad = false, bool forSave = false)
    {
        if (forLoad)
        {
            if (string.IsNullOrWhiteSpace(DataSource)) throw new ArgumentNullException(nameof(DataSource));
            if (DataInput is null) throw new ArgumentNullException(nameof(DataInput));
            if (DataDeserializer is null) throw new ArgumentNullException(nameof(DataDeserializer));
        }

        if (forSave)
        {
            if (string.IsNullOrWhiteSpace(DataTarget)) throw new ArgumentNullException(nameof(DataSource));
            if (DataSerializer is null) throw new ArgumentNullException(nameof(DataSerializer));
            if (DataOutput is null) throw new ArgumentNullException(nameof(DataOutput));
        }
    }

    #endregion

    #region Get and Set

    /// <summary>
    /// Retrieves data from the specified storage <see cref="DataSource"/> using 
    /// the specified loader <see cref="DataInput"/>, deserializes it using 
    /// the specified deserializer <see cref="DataDeserializer"/>, 
    /// and returns the retrieved document <see cref="IDataDocument"/>.
    /// </summary>
    /// <returns>Loaded document</returns>
    public async Task<IDataDocument> Load()
    {
        CheckParams(forLoad: true);
        using (var sourceStream = await DataInput.OpenSourceStream(DataSource))
            return await DataDeserializer.Deserialize(sourceStream);
    }

    /// <summary>
    /// Serializes the document using the specified serializer <see cref="DataSerializer"/>, 
    /// and saves it to the target store <see cref="DataTarget"/> using the specified 
    /// saver <see cref="DataOutput"/>.
    /// </summary>
    /// <param name="document"></param>
    public async Task Save(IDataDocument document)
    {
        CheckParams(forSave: true);
        using (var targetStream = await DataOutput.OpenTargetStream(DataTarget))
            await DataSerializer.Serialize(document, targetStream);
    }

    /// <summary>
    /// Retrieves data from the specified storage <see cref="DataSource"/> using 
    /// the specified loader <see cref="DataInput"/>, deserializes it using 
    /// the specified deserializer <see cref="DataDeserializer"/>, then 
    /// serializes the document using the specified serializer <see cref="DataSerializer"/>, 
    /// and saves it to the target store <see cref="DataTarget"/> using the specified 
    /// saver <see cref="DataOutput"/>.
    /// </summary>
    public async Task LoadAndSave()
    {
        CheckParams(forSave: true); // Don't load if it won't save
        var doc = await Load();
        await Save(doc);
    }

    #endregion

    #region Static versions of methods

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
