using PizzeriaContracts.BindingModels;
using PizzeriaContracts.ViewModels;

namespace PizzeriaContracts.BusinessLogicsContracts
{
    public interface IReportLogic
    {
        List<ReportPizzaComponentViewModel> GetPizzaComponents();
        List<ReportOrdersViewModel> GetOrders(ReportBindingModel model);
        List<ReportShopsViewModel> GetShops();
        List<ReportGroupOrdersViewModel> GetGroupedOrders();
        void SavePizzasToWordFile(ReportBindingModel model);
        void SavePizzaComponentToExcelFile(ReportBindingModel model);
        void SaveOrdersToPdfFile(ReportBindingModel model);
        void SaveShopsToWordFile(ReportBindingModel model);
        void SaveShopsToExcelFile(ReportBindingModel model);
        void SaveGroupedOrdersToPdfFile(ReportBindingModel model);
    }
}