using Homework.ProcessClasses.Configuration;
using Homework.ProcessClasses.Reflection;
using Microsoft.Extensions.Configuration;
using Moravia.Homework.Interfaces;
using Moravia.Homework.ProcessClasses.Document;
using McMaster.Extensions.CommandLineUtils; // NuGet https://github.com/natemcmaster/CommandLineUtils


// Use Console application with command lines by CommandLineUtils
var app = new CommandLineApplication();

// Load configuration from config.json
var builder = new ConfigurationBuilder()
      .SetBasePath(Environment.CurrentDirectory)
      .AddJsonFile("config.json", optional: false);
var config = builder.Build().GetSection("DataChangers").Get<Configuration>();

// Preparation of the dictionary of available IOLS classes
using var cf = new ClassFinder();
cf.Initialize();

#region Console input arguments

app.HelpOption(); // Support for "--help" command

// DataSource
var argDataSource = app.Option<string>("-src|--Source <DATASOURCE>", "Path/Uri... to the source", CommandOptionType.SingleValue);
argDataSource.DefaultValue = config.DataSource;

// DataInput
var dInputNames = cf.GetNamesForInterface<IDataInput>();
var argDataInput = app.Option<string>("-di|--DataInput <DATAINPUT>", $"Input data loader", CommandOptionType.SingleValue);
argDataInput.DefaultValue = config.DataInputName;
argDataInput.Accepts().Values(dInputNames);

// DataDeserializer
var dDeserializerNames = cf.GetNamesForInterface<IDataDeserializer>();
var argDataDeserializer = app.Option<string>("-dd|--DataDeserializer <DATADESERIALIZER>", $"Data deserializer", CommandOptionType.SingleValue);
argDataDeserializer.DefaultValue = config.DataDeserializerName;
argDataDeserializer.Accepts().Values(dDeserializerNames);

// DataTarget
var argDataTarget = app.Option<string>("-trg|--Target <DATASOURCE>", "Path/Uri... for saving the output", CommandOptionType.SingleValue);
argDataTarget.DefaultValue = config.DataTarget;

// DataSerializer
var dSerializerNames = cf.GetNamesForInterface<IDataSerializer>();
var argDataSerializer = app.Option<string>("-ds|--DataSerializer <DATASERIALIZER>", $"Data serializer", CommandOptionType.SingleValue);
argDataSerializer.DefaultValue = config.DataSerializerName;
argDataSerializer.Accepts().Values(dSerializerNames);

// DataOutput
var dOutputNames = cf.GetNamesForInterface<IDataOutput>();
var argDataOutput = app.Option<string>("-do|--DataOutput <DATAOUTPUT>", $"Output data storer", CommandOptionType.SingleValue);
argDataOutput.DefaultValue = config.DataOutputName;
argDataOutput.Accepts().Values(dOutputNames);

#endregion


// Main program
app.OnExecuteAsync(async cancellationToken =>
{
    // Preparation of worker
    var dw = new DocumentWorker
    {
        DataSource = argDataSource.Value(),
        DataInput = cf.GetInstanceByName<IDataInput>(argDataInput.Value()),
        DataDeserializer = cf.GetInstanceByName<IDataDeserializer>(argDataDeserializer.Value()),
        DataTarget = argDataTarget.Value(),
        DataSerializer = cf.GetInstanceByName<IDataSerializer>(argDataSerializer.Value()),
        DataOutput = cf.GetInstanceByName<IDataOutput>(argDataOutput.Value()),
    };

    // Launch the whole process
    try
    {
        await dw.LoadAndSave();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: {0}", ex.Message);
        throw;
    }

});

// Run the application
return app.Execute(args);