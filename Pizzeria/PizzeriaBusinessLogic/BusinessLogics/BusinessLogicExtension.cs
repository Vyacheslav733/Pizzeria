using PizzeriaBusinessLogic.MailWorker;
using PizzeriaBusinessLogic.OfficePackage.Implements;
using PizzeriaBusinessLogic.OfficePackage;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.DI;

namespace PizzeriaBusinessLogic.BusinessLogics
{
    public class BusinessLogicExtension : IBusinessLogicExtension
    {
        public int Priority => 0;

        public void RegisterServices()
        {
            DependencyManager.Instance.RegisterType<IClientLogic, ClientLogic>();
            DependencyManager.Instance.RegisterType<IComponentLogic, ComponentLogic>();
            DependencyManager.Instance.RegisterType<IOrderLogic, OrderLogic>();
            DependencyManager.Instance.RegisterType<IPizzaLogic, PizzaLogic>();
            DependencyManager.Instance.RegisterType<IReportLogic, ReportLogic>();
            DependencyManager.Instance.RegisterType<IImplementerLogic, ImplementerLogic>();
            DependencyManager.Instance.RegisterType<IMessageInfoLogic, MessageInfoLogic>();
            DependencyManager.Instance.RegisterType<IBackUpLogic, BackUpLogic>();
            DependencyManager.Instance.RegisterType<IShopLogic, ShopLogic>();


            DependencyManager.Instance.RegisterType<AbstractSaveToWord, SaveToWord>();
            DependencyManager.Instance.RegisterType<AbstractSaveToExcel, SaveToExcel>();
            DependencyManager.Instance.RegisterType<AbstractSaveToPdf, SaveToPdf>();
            DependencyManager.Instance.RegisterType<AbstractMailWorker, MailKitWorker>(true);

            DependencyManager.Instance.RegisterType<IWorkProcess, WorkModeling>();
        }
    }
}
