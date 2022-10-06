using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Moravia.Homework
{
    public class Document
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var sourceFileName = args.Length > 0 ? args[0] : Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\Document1.xml");
            var targetFileName = args.Length > 1 ? args[1] : Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Document1.json");

            if (!File.Exists(sourceFileName))
            {
                Console.WriteLine($"File '{sourceFileName}' not found");
                return;
            }

            string errorMessage = "Error";
            try
            {
                errorMessage = "An error occurred while reading the source data";
                string input;
                using (var reader = new StreamReader(sourceFileName))
                    input = reader.ReadToEnd();

                errorMessage = "An error occurred during the processing of the retrieved data";
                var xdoc = XDocument.Parse(input);
                var doc = new Document
                {
                    Title = xdoc.Root.Element("title")?.Value,
                    Text = xdoc.Root.Element("text")?.Value
                };

                var serializedDoc = JsonConvert.SerializeObject(doc);

                errorMessage = "An error occurred while writing the result";
                using (var sw = new StreamWriter(targetFileName))
                    sw.Write(serializedDoc);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{0}: {1}", errorMessage, ex.Message);
                throw;
            }
        }
    }
}