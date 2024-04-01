namespace SV20T1020091.Web.Models
{
    /// <summary>
    /// đầu vào tìm kiếm dữ liệu để nhận dữ liệu dưới dạng phân trang
    /// </summary>
    public class PaginationSearchInput
    {
        /// <summary>
        /// Đầu vào tìm kiếm dữ liệu để nhận dữ liệu dưới dạng phân trang
        /// </summary>
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string SearchValue { get; set; } = "";
    }
    public class ProductSearchInput : PaginationSearchInput
    {
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public decimal minPrice { get; set; }
        public decimal maxPrice { get; set; }
    }

}
