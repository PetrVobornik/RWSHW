using Moravia.Homework.Interfaces;
using System.Xml.Linq;

namespace Moravia.Homework.Implementation.LS;

public class XmlRW : IDataDeserializer, IDataSerializer
{
    public async Task<IDataDocument> Deserialize(Stream source)
    {
        var xdoc = await XDocument.LoadAsync(source, LoadOptions.None, CancellationToken.None);
        return new DocumentHelper {
            Title = xdoc?.Root?.Element("title")?.Value,
            Text = xdoc?.Root?.Element("text")?.Value
        };

        // It is also possible to use the XmlReader
    }

    public async Task Serialize(IDataDocument document, Stream target)
    {
        var xdoc = new XDocument(
            new XElement("data",
                new XElement("title", document.Title),
                new XElement("text", document.Text)
            )
        );
        await xdoc.SaveAsync(target, SaveOptions.None, CancellationToken.None);

        // It is also possible to use the XML serializer
        //XmlSerializer serializer = new XmlSerializer(typeof(IDataDocument));
        //serializer.Serialize(target, document);
    }
}