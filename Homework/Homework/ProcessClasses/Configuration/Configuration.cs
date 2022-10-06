using Moravia.Homework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework.ProcessClasses.Configuration
{
    internal class Configuration
    {
        public string DataSource { get; set; }
        public string DataInputName { get; set; }
        public string DataDeserializerName { get; set; }

        public string DataSerializerName { get; set; }
        public string DataOutputName { get; set; }
        public string DataTarget { get; set; }
    }
}
