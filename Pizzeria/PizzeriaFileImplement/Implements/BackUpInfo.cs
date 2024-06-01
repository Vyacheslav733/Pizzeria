using PizzeriaContracts.StoragesContracts;
using System.Reflection;

namespace PizzeriaFileImplement.Implements
{
    public class BackUpInfo : IBackUpInfo
    {
        private readonly DataFileSingleton source;
        private readonly PropertyInfo[] sourceProperties;

        public BackUpInfo()
        {
            source = DataFileSingleton.GetInstance();
            sourceProperties = source.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        public List<T>? GetList<T>() where T : class, new()
        {
            var requredType = typeof(T);
            return (List<T>?)sourceProperties.FirstOrDefault(x => x.PropertyType.IsGenericType && x.PropertyType.GetGenericArguments()[0] == requredType)
                ?.GetValue(source);

        }

        public Type? GetTypeByModelInterface(string modelInterfaceName)
        {
            var assembly = typeof(BackUpInfo).Assembly;
            var types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsClass && type.GetInterface(modelInterfaceName) != null)
                {
                    return type;
                }
            }
            return null;
        }
    }
}
