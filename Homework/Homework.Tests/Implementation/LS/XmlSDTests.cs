using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moravia.Homework.Interfaces;
using Moravia.Homework.Tests.TestsUtils;
using System.Xml.Linq;

namespace Moravia.Homework.Implementation.LS.Tests;

[TestClass]
public class XmlSDTests
{
    [TestMethod]
    public async Task DeserializeTest()
    {
        var deserializer = new XmlSD();

        string title = "Document title";
        string text = "Document text";
        string strData = $"<data><title>{title}</title><text>{text}</text></data>";

        using (var stream = StreamUtils.StringToStream(strData))
        {
            var doc = await deserializer.Deserialize(stream);
            Assert.IsNotNull(doc);
            Assert.AreEqual(doc.Title, title);
            Assert.AreEqual(doc.Text, text);
        }
    }

    [TestMethod]
    public async Task SerializeTest()
    {
        var doc = new DocumentHelper()
        {
            Title = "Document title",
            Text = "Document text",
        };

        string serializedData;
        var serializer = new XmlSD();
        using (var ms = new MemoryStream())
        {
            await serializer.Serialize(doc, ms);
            ms.Seek(0, SeekOrigin.Begin);
            serializedData = StreamUtils.StreamToString(ms);
        }
        var xdoc = XDocument.Parse(serializedData);

        Assert.IsNotNull(xdoc);
        Assert.IsNotNull(xdoc.Root);
        Assert.AreEqual(doc.Title, xdoc.Root.Element("title")?.Value);
        Assert.AreEqual(doc.Text, xdoc.Root.Element("text")?.Value);
    }
}