using Moravia.Homework.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Moravia.Homework.Implementation.LS
{
    public class JsonRW : IDataDeserializer, IDataSerializer
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
}
