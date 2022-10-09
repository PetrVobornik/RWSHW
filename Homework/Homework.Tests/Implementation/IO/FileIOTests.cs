using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moravia.Homework.Implementation.IO;
using Moravia.Homework.Tests.TestsUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moravia.Homework.Implementation.IO.Tests;

[TestClass]
public class FileIOTests
{
    [TestMethod]
    public async Task OpenSourceStreamTest()
    {
        string testData = "Test string";
        string path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        File.WriteAllText(path, testData);
        try
        {
            var loader = new FileIO();
            string loadedData;
            using (var stream = await loader.OpenSourceStream(path))
                loadedData = StreamUtils.StreamToString(stream);
            Assert.AreEqual(testData, loadedData);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [TestMethod]
    public async Task OpenTargetStreamTest()
    {
        string testData = "Test string";
        string path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        
        var saver = new FileIO();
        using (var stream = await saver.OpenTargetStream(path))
        using (var data = StreamUtils.StringToStream(testData))
            data.CopyTo(stream);
        
        try
        {
            Assert.IsTrue(File.Exists(path));
            string loadedData = File.ReadAllText(path);
            Assert.AreEqual(testData, loadedData);
        }
        finally
        {
            File.Delete(path);
        }
    }
}