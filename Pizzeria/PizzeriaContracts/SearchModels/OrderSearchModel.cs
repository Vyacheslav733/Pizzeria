using PizzeriaDataModels.Enums;

namespace PizzeriaContracts.SearchModels
{
    public class OrderSearchModel
    {
        public int? Id { get; set; }
        public int? ClientId { get; set; }
        public OrderStatus? Status { get; set; }
        public int? ImplementerId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}