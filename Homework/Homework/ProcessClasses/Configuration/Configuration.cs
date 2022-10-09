namespace Moravia.Homework.ProcessClasses.Configuration;

/// <summary>
/// Data object for loading configuration from config.json
/// </summary>
internal class Configuration
{
    public string DataSource { get; set; }
    public string DataInputName { get; set; }
    public string DataDeserializerName { get; set; }

    public string DataSerializerName { get; set; }
    public string DataOutputName { get; set; }
    public string DataTarget { get; set; }
}
