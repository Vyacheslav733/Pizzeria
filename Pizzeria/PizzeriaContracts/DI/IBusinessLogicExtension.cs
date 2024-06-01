namespace PizzeriaContracts.DI
{
    public interface IBusinessLogicExtension
    {
        public int Priority { get; }

        public void RegisterServices();
    }
}
