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

            string input;
            try
            {
                using (var reader = new StreamReader(sourceFileName))
                    input = reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the source data: {0}", ex.Message);
                throw;
            }

            var xdoc = XDocument.Parse(input);
            var doc = new Document
            {
                Title = xdoc.Root.Element("title")?.Value,
                Text = xdoc.Root.Element("text")?.Value
            };

            var serializedDoc = JsonConvert.SerializeObject(doc);

            using (var sw = new StreamWriter(targetFileName))
                sw.Write(serializedDoc);
        }
    }
}