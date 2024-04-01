using SV20T1020091.DomainModels;

namespace SV20T1020091.Web.Models
{
    public abstract class BasePaginationResult
    {
        public int Page { get; set; }
        public int PageSize { get; set; }// 20
        public string SearchValue { get; set; } = "";
        public int RowCount { get; set; }
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                {
                    return 1;
                }
                int c = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                {
                    c += 1;
                }
                return c;
            }
        }
    }
    public abstract class BasePaginationProductResult
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string SearchValue { get; set; } = "";
        public int CategoryID { get; set; }
        public int SupplierID { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public int RowCount { get; set; }
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                {
                    return 1;
                }
                int c = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                {
                    c += 1;
                }
                return c;
            }
        }
    }
    public class ProductSearchResult : BasePaginationProductResult
    {
        public List<Product> Data { get; set; }
    }
    /// <summary>
    /// kết quả tìm kiếm và lấy danh sách khách hàng
    /// </summary>
    public class CustomerSearchResult : BasePaginationResult
    {
        public List<Customer> Data { get; set; }
    }
    /// <summary>
    /// kết quả tìm kiếm và lấy danh sách loại hàng
    /// </summary>
    public class CategorySearchResult : BasePaginationResult
    {
        public List<Category> Data { get; set; }
    }
    /// <summary>
    /// kết quả tìm kiếm và lấy danh sách nhân viên
    /// </summary>
    public class EmployeeSearchResult : BasePaginationResult
    {
        public List<Employee> Data { get; set; }
    }
    /// <summary>
    /// kết quả tìm kiếm và lấy danh sách người giao hàng
    /// </summary>
    public class ShipperSearchResult : BasePaginationResult
    {
        public List<Shipper> Data { get; set; }
    }
    /// <summary>
    /// kết quả tìm kiếm và lấy danh sách nhà cung cấp
    /// </summary>
    public class SupplierSearchResult : BasePaginationResult
    {
        public List<Supplier> Data { get; set; }
    }
}
