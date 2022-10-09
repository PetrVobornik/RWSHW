using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moravia.Homework.Interfaces;
using Moravia.Homework.Tests.TestsUtils;
using Newtonsoft.Json;

namespace Moravia.Homework.Implementation.LS.Tests;

[TestClass]
public class JsonSDTests
{
    [TestMethod]
    public async Task DeserializeTest()
    {
        var deserializer = new JsonSD();

        string title = "Document title";
        string text = "Document text";
        string strData = "{\"Title\":\"" + title + "\",\"Text\":\"" + text + "\"}";

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
        var doc = new DocumentHelper() {
            Title = "Document title",
            Text = "Document text",
        };

        string serializedData;
        var serializer = new JsonSD();
        using (var ms = new MemoryStream())
        {
            await serializer.Serialize(doc, ms);
            ms.Seek(0, SeekOrigin.Begin);
            serializedData = StreamUtils.StreamToString(ms);
        }
        var doc2 = JsonConvert.DeserializeObject<DocumentHelper>(serializedData);

        Assert.IsNotNull(doc2);
        Assert.AreEqual(doc.Title, doc2.Title);
        Assert.AreEqual(doc.Text, doc2.Text);
    }
}