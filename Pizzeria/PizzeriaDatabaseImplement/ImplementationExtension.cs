using PizzeriaContracts.DI;
using PizzeriaContracts.StoragesContracts;
using PizzeriaDatabaseImplement.Implements;

namespace PizzeriaDatabaseImplement
{
    public class ImplementationExtension : IImplementationExtension
    {
        public int Priority => 3;

        public void RegisterServices()
        {
            DependencyManager.Instance.RegisterType<IClientStorage, ClientStorage>();
            DependencyManager.Instance.RegisterType<IComponentStorage, ComponentStorage>();
            DependencyManager.Instance.RegisterType<IImplementerStorage, ImplementerStorage>();
            DependencyManager.Instance.RegisterType<IMessageInfoStorage, MessageInfoStorage>();
            DependencyManager.Instance.RegisterType<IShopStorage, ShopStorage>();

            DependencyManager.Instance.RegisterType<IOrderStorage, OrderStorage>();
            DependencyManager.Instance.RegisterType<IPizzaStorage, PizzaStorage>();
            DependencyManager.Instance.RegisterType<IBackUpInfo, BackUpInfo>();
        }
    }
}
