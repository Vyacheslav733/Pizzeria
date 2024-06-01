using Microsoft.Extensions.Logging;
using Unity;
using Unity.Microsoft.Logging;

namespace PizzeriaContracts.DI
{
    public class UnityDependencyContainer : IDependencyContainer
    {
        private readonly UnityContainer _container;

        public UnityDependencyContainer()
        {
            _container = new UnityContainer();
        }

        public void AddLogging(Action<ILoggingBuilder> configure)
        {
            _container.AddExtension(new LoggingExtension(LoggerFactory.Create(configure)));
        }

        public void RegisterType<T, U>(bool isSingle) where U : class, T where T : class
        {
            if (isSingle)
            {
                _container.RegisterSingleton<T, U>();
            }
            else
            {
                _container.RegisterType<T, U>();
            }
        }

        public void RegisterType<T>(bool isSingle) where T : class
        {
            if (isSingle)
            {
                _container.RegisterSingleton<T>();
            }
            else
            {
                _container.RegisterType<T>();
            }
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
