using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moravia.Homework.Implementation.Reflection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DataChangerAttribute : Attribute
{
    public string Name { get; set; }
    public DataChangerAttribute(string name) => Name = name?.ToUpper();
}
