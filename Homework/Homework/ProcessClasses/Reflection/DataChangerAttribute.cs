namespace Moravia.Homework.ProcessClasses.Reflection;

/// <summary>
/// An attribute to identify and specify a unique name for classes 
/// that process data in one of four phases
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DataChangerAttribute : Attribute
{
    public string Name { get; set; }
    public DataChangerAttribute(string name) => Name = name?.ToUpper();
}
