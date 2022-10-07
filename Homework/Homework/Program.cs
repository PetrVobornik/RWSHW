using Homework.ProcessClasses.Configuration;
using Homework.ProcessClasses.Reflection;
using Microsoft.Extensions.Configuration;
using Moravia.Homework.Interfaces;
using Moravia.Homework.ProcessClasses.Document;
using McMaster.Extensions.CommandLineUtils; // NuGet https://github.com/natemcmaster/CommandLineUtils
using System.Text;


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

// DataTarget
var argDataTarget = app.Option<string>("-trg|--Target <DATASOURCE>", "Path/Uri... for saving the output", CommandOptionType.SingleValue);
argDataTarget.DefaultValue = config.DataTarget;

#endregion


// Main program
app.OnExecuteAsync(async cancellationToken =>
{
    try
    {
        // Preparation of worker
        var dw = new DocumentWorker
        {
            DataSource = argDataSource.Value(),
            DataInput = cf.GetInstanceByName<IDataInput>(argDataInput.Value()),
            DataDeserializer = cf.GetInstanceByName<IDataDeserializer>(argDataDeserializer.Value()),
            DataSerializer = cf.GetInstanceByName<IDataSerializer>(argDataSerializer.Value()),
            DataOutput = cf.GetInstanceByName<IDataOutput>(argDataOutput.Value()),
            DataTarget = argDataTarget.Value(),
        };

        // Launch the whole process
        await dw.LoadAndSave();
    }
    catch (Exception ex)
    {
        // Compose of an error message
        var usedParams = new StringBuilder();
        usedParams.AppendLine($"Error: {ex.Message}");
        usedParams.AppendLine($"Input: {argDataInput.Value()}");
        usedParams.AppendLine($"Deserializer: {argDataDeserializer.Value()}");
        usedParams.AppendLine($"Serializer: {argDataSerializer.Value()}");
        usedParams.AppendLine($"Output: {argDataOutput.Value()}");
        usedParams.AppendLine($"Source: '{argDataSource.Value()}'");
        usedParams.AppendLine($"Target: '{argDataTarget.Value()}'");

        Console.WriteLine(usedParams);

        // Error messages, including ex.StackTrace should be logged here
        //throw;   // Uncoment if the program is to be used by another application via the command line
    }
});

// Run the application
return app.Execute(args);