using System.Xml.Linq;
using Homework.Implementation.Reflection;
using Moravia.Homework.Implementation.Doc;
using Moravia.Homework.Implementation.IO;
using Moravia.Homework.Implementation.LS;
using Moravia.Homework.Interfaces;
using Newtonsoft.Json;

namespace Moravia.Homework;

class Program
{
    static void Main(string[] args)
    {
        var sourceFileName = args.Length > 0 ? args[0] : Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\Document1.xml");
        var targetFileName = args.Length > 1 ? args[1] : Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Document1.json");

        using var cf = new ClassFinder();
        cf.Initialize();        

        var dw = new DocumentWorker() {
            DataSource = sourceFileName,
            DataInput = cf.GetInstanceByName<IDataInput>("FILE"),
            //DataSource = "https://programko.net/rwshw.xml",
            //DataInput = cf.GetInstanceByName<IDataInput>("HTTP"),
            DataDeserializer = cf.GetInstanceByName<IDataDeserializer>("XML"),
            DataTarget = targetFileName,
            DataSerializer = cf.GetInstanceByName<IDataSerializer>("JSON"),
            DataOutput = cf.GetInstanceByName<IDataOutput>("FILE"),
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