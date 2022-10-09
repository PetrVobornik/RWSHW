using Moravia.Homework.Interfaces;
using System.Data;
using System.Reflection;

namespace Moravia.Homework.ProcessClasses.Reflection;

/// <summary>
/// Helper class for work with reflection
/// </summary>
internal class ClassFinder : IDisposable
{
    /// <summary>
    /// A dictionary for translating the names specified by an attribute 
    /// to the type (class) that operates under that name
    /// </summary>
    Dictionary<string, Type> dataChangerClasses;

    /// <summary>
    /// Cache instances of already created objects according to their 
    /// attribute names, so that a reusable instance does not have to be 
    /// created multiple times
    /// </summary>
    Dictionary<string, object> dataChangerCache;

    /// <summary>
    /// Find all classes that implement data interfaces and have 
    /// a processing name defined by the attribute <see cref="DataChangerAttribute"/>.
    /// The search is performed only on the first call to this method.
    /// </summary>
    /// <param name="assembly">Assembly to be searched, NULL = Use assembly this running applications</param>
    public void Initialize(Assembly assembly = null)
    {
        // Run only once
        if (dataChangerClasses is not null)
            return;

        // Auxiliary list of data interfaces
        string[] dataChangerInterfaces = {
            typeof(IDataInput).FullName,
            typeof(IDataOutput).FullName,
            typeof(IDataDeserializer).FullName,
            typeof(IDataSerializer).FullName,
        };

        // If assembly is not specified, use it from the startup proejcts of this application
        if (assembly == null)
            assembly = Assembly.GetExecutingAssembly();

        // Load the dictionary of data classes according to their attribute names by LINQ searching the specified assembly
        dataChangerClasses = assembly.GetTypes()
            .Where(t =>
                t.IsClass &&
                t.IsDefined(typeof(DataChangerAttribute)) &&
                t.GetInterfaces().Any(i =>
                    dataChangerInterfaces.Contains(i.FullName))
            ).ToDictionary(
                k => k.GetCustomAttribute<DataChangerAttribute>().Name,
                v => v
            );

        // Preparation of the caching dictionary
        dataChangerCache = new Dictionary<string, object>();
    }

    /// <summary>
    /// Returns the type (class) for processing data according to its unique attribute name
    /// </summary>
    /// <param name="name">Attribute name of the class we want to get</param>
    /// <returns>Data processing class requested by the entered attribute name</returns>
    /// <exception cref="KeyNotFoundException">The exception will be raised if the class of the specified name is not found</exception>
    public Type GetTypeByName(string name)
    {
        if (dataChangerClasses is null)
            Initialize();

        if (dataChangerClasses.TryGetValue(name.ToUpper(), out Type type))
            return type;

        throw new KeyNotFoundException($"Data changer class with name '{name}' not found");
    }

    /// <summary>
    /// Returns the instance of class for processing data according to its unique attribute name, casted by the T interface
    /// </summary>
    /// <typeparam name="T">Interface for working with data to be implemented by the class found by the specified attribute name</typeparam>
    /// <param name="name">Attribute name of the class whose instance we want to get</param>
    /// <returns>Instance of the requested class casted by the T interface</returns>
    /// <exception cref="KeyNotFoundException">Raised if the class of the specified name is not found</exception>
    /// <exception cref="Exception">Raised if the class found by the name parameter does not implement the T interface</exception>
    public T GetInstanceByName<T>(string name)
    {
        object obj;
        if (!dataChangerCache.TryGetValue(name, out obj))
        {
            var type = GetTypeByName(name);
            obj = Activator.CreateInstance(type);
        }
        if (obj is not T)
            throw new Exception($"Type '{obj.GetType().Name}' is not subclass of '{typeof(T).Name}'.");
        return (T)obj;
    }

    /// <summary>
    /// List of attribute names of all classes implementing the T interface
    /// </summary>
    /// <typeparam name="T">The interface for which we want to return a list of attribute names of the classes that implement it</typeparam>
    /// <returns>List (array) of attribute names of classes implementing the T interface</returns>
    public string[] GetNamesForInterface<T>()
    {
        if (dataChangerClasses is null)
            Initialize();

        string fullName = typeof(T).FullName;

        return dataChangerClasses
            .Where(t => t.Value.GetInterfaces().Any(i => fullName == i.FullName))
            .Select(x => x.Key)
            .ToArray();
    }

    /// <summary>
    /// Release dictionary references
    /// </summary>
    public void Dispose()
    {
        dataChangerClasses = null;
        dataChangerCache = null;
    }
}
