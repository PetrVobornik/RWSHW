using System.Xml.Linq;
using Homework.ProcessClasses.Configuration;
using Homework.ProcessClasses.Reflection;
using Microsoft.Extensions.Configuration;
using Moravia.Homework.Implementation.IO;
using Moravia.Homework.Implementation.LS;
using Moravia.Homework.Interfaces;
using Moravia.Homework.ProcessClasses.Document;
using Newtonsoft.Json;

namespace Moravia.Homework;

class Program
{
    static void Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
              .SetBasePath(Environment.CurrentDirectory)
              .AddJsonFile("config.json", optional: false);

        var config = builder.Build().GetSection("DataChangers").Get<Configuration>();

        using var cf = new ClassFinder();
        cf.Initialize();        

        var dw = new DocumentWorker {
            DataSource = config.DataSource,
            DataInput = cf.GetInstanceByName<IDataInput>(config.DataInputName),
            DataDeserializer = cf.GetInstanceByName<IDataDeserializer>(config.DataDeserializerName),
            DataTarget = config.DataTarget,
            DataSerializer = cf.GetInstanceByName<IDataSerializer>(config.DataSerializerName),
            DataOutput = cf.GetInstanceByName<IDataOutput>(config.DataOutputName),
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