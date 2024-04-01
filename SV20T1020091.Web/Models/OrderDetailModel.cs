using SV20T1020091.DomainModels;

namespace SV20T1020091.Web.Models
{
    public class OrderDetailModel :Order
    {
        public Order Order { get; set; }

        public List<OrderDetail> Details { get; set; }

    }
}
