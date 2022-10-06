using Moravia.Homework.Interfaces;
using Moravia.Homework.ProcessClasses.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Homework.ProcessClasses.Reflection;

internal class ClassFinder : IDisposable
{
    Dictionary<string, Type> dataChangerClasses;
    Dictionary<string, object> dataChangerCache;

    public void Initialize(Assembly assembly = null)
    {
        if (dataChangerClasses is not null)
            return;

        string[] dataChangerInterfaces = {
            typeof(IDataInput).FullName,
            typeof(IDataOutput).FullName,
            typeof(IDataDeserializer).FullName,
            typeof(IDataSerializer).FullName,
        };

        if (assembly == null)
            assembly = Assembly.GetExecutingAssembly();

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

        dataChangerCache = new Dictionary<string, object>();
    }

    public Type GetTypeByName(string name)
    {
        if (dataChangerClasses is null)
            Initialize();

        if (dataChangerClasses.TryGetValue(name.ToUpper(), out Type type))
            return type;

        throw new Exception($"Data changer class with name '{name}' not found");
    }

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

    public void Dispose()
    {
        dataChangerClasses = null;
        dataChangerCache = null;
    }
}
