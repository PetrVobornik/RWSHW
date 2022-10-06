using System.Xml.Linq;
using Moravia.Homework.Implementation.Doc;
using Moravia.Homework.Implementation.IO;
using Moravia.Homework.Implementation.LS;
using Newtonsoft.Json;

namespace Moravia.Homework;

class Program
{
    static void Main(string[] args)
    {
        var sourceFileName = args.Length > 0 ? args[0] : Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\Document1.xml");
        var targetFileName = args.Length > 1 ? args[1] : Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Document1.json");

        var dw = new DocumentWorker() {
            DataSource = sourceFileName,
            DataInput = new FileIO(),
            //DataSource = "https://programko.net/rwshw.xml",
            //DataInput = new HttpI(),
            DataDeserializer = new XmlRW(),
            DataTarget = targetFileName,
            DataSerializer = new JsonRW(),
            DataOutput = new FileIO(),
        };

        try
        {
            dw.LoadAndSave().Wait();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: {0}", ex.Message);
            throw;
        }
    }
}