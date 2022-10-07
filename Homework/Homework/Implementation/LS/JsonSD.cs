using Moravia.Homework.Interfaces;
using Moravia.Homework.ProcessClasses.Reflection;
using Newtonsoft.Json;

namespace Moravia.Homework.Implementation.LS;

/// <summary>
/// Class for serialization and deserialization of data from/to JSON format
/// </summary>
[DataChanger("JSON")]
public class JsonSD : IDataDeserializer, IDataSerializer
{
    public async Task<IDataDocument> Deserialize(Stream source)
    {
        using (var sr = new StreamReader(source))
        using (var jtr = new JsonTextReader(sr))
        {
            var serializer = new JsonSerializer();
            return serializer.Deserialize<DocumentHelper>(jtr);
        }
    }

    public async Task Serialize(IDataDocument document, Stream target)
    {
        using (var sw = new StreamWriter(target))
        using (JsonTextWriter jtw = new JsonTextWriter(sw))
        {
            var ser = new JsonSerializer();
            ser.Serialize(jtw, document);
            jtw.Flush();
        }
    }
}
