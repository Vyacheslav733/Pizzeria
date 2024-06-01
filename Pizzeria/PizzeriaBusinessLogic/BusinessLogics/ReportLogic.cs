using PizzeriaBusinessLogic.OfficePackage.HelperModels;
using PizzeriaBusinessLogic.OfficePackage;
using PizzeriaContracts.BindingModels;
using PizzeriaContracts.BusinessLogicsContracts;
using PizzeriaContracts.SearchModels;
using PizzeriaContracts.StoragesContracts;
using PizzeriaContracts.ViewModels;

namespace PizzeriaBusinessLogic.BusinessLogics
{
    public class ReportLogic : IReportLogic
    {
        private readonly IComponentStorage _componentStorage;
        private readonly IPizzaStorage _pizzaStorage;
        private readonly IOrderStorage _orderStorage;
        private readonly IShopStorage _shopStorage;
        private readonly AbstractSaveToExcel _saveToExcel;
        private readonly AbstractSaveToWord _saveToWord;
        private readonly AbstractSaveToPdf _saveToPdf;

        public ReportLogic(IPizzaStorage pizzaStorage, IComponentStorage componentStorage, IOrderStorage orderStorage, IShopStorage shopStorage,
            AbstractSaveToExcel saveToExcel, AbstractSaveToWord saveToWord, AbstractSaveToPdf saveToPdf)
        {
            _pizzaStorage = pizzaStorage;
            _componentStorage = componentStorage;
            _orderStorage = orderStorage;
            _shopStorage = shopStorage;

            _saveToExcel = saveToExcel;
            _saveToWord = saveToWord;
            _saveToPdf = saveToPdf;
        }

        public List<ReportPizzaComponentViewModel> GetPizzaComponents()
        {
            return _pizzaStorage.GetFullList().Select(x => new ReportPizzaComponentViewModel
            {
                PizzaName = x.PizzaName,
                Components = x.PizzaComponents.Select(x => (x.Value.Item1.ComponentName, x.Value.Item2)).ToList(),
                TotalCount = x.PizzaComponents.Select(x => x.Value.Item2).Sum()
            }).ToList();
        }

        public List<ReportOrdersViewModel> GetOrders(ReportBindingModel model)
        {
            return _orderStorage.GetFilteredList(new OrderSearchModel { DateFrom = model.DateFrom, DateTo = model.DateTo })
                    .Select(x => new ReportOrdersViewModel
                    {
                        Id = x.Id,
                        DateCreate = x.DateCreate,
                        PizzaName = x.PizzaName,
                        Sum = x.Sum,
                        Status = x.Status.ToString()
                    })
                    .ToList();
        }

        public void SavePizzasToWordFile(ReportBindingModel model)
        {
            _saveToWord.CreatePizzaDoc(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список пицц",
                Pizzas = _pizzaStorage.GetFullList()
            });
        }

        public void SavePizzaComponentToExcelFile(ReportBindingModel model)
        {
            _saveToExcel.CreateReport(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список пицц",
                PizzaComponents = GetPizzaComponents()
            });
        }

        public void SaveOrdersToPdfFile(ReportBindingModel model)
        {
            _saveToPdf.CreateDoc(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Список заказов",
                DateFrom = model.DateFrom!.Value,
                DateTo = model.DateTo!.Value,
                Orders = GetOrders(model)
            });
        }

        public List<ReportShopsViewModel> GetShops()
        {
            return _shopStorage.GetFullList().Select(x => new ReportShopsViewModel
            {
                ShopName = x.ShopName,
                Pizzas = x.ShopPizzas.Select(x => (x.Value.Item1.PizzaName, x.Value.Item2)).ToList(),
                TotalCount = x.ShopPizzas.Select(x => x.Value.Item2).Sum()
            }).ToList();
        }

        public List<ReportGroupOrdersViewModel> GetGroupedOrders()
        {
            return _orderStorage.GetFullList().GroupBy(x => x.DateCreate.Date).Select(x => new ReportGroupOrdersViewModel
            {
                Date = x.Key,
                OrdersCount = x.Count(),
                OrdersSum = x.Select(y => y.Sum).Sum()
            }).ToList();
        }

        public void SaveShopsToWordFile(ReportBindingModel model)
        {
            _saveToWord.CreateShopsDoc(new WordShopInfo
            {
                FileName = model.FileName,
                Title = "Список магазинов",
                Shops = _shopStorage.GetFullList()
            });
        }

        public void SaveShopsToExcelFile(ReportBindingModel model)
        {
            _saveToExcel.CreateShopPizzasReport(new ExcelShop
            {
                FileName = model.FileName,
                Title = "Наполненость магазинов",
                ShopPizzas = GetShops()
            });
        }

        public void SaveGroupedOrdersToPdfFile(ReportBindingModel model)
        {
            _saveToPdf.CreateGroupedOrdersDoc(new PdfGroupedOrdersInfo
            {
                FileName = model.FileName,
                Title = "Список заказов сгруппированных по дате заказов",
                GroupedOrders = GetGroupedOrders()
            });
        }
    }
}
