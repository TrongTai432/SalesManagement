using SV20T1020091.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020091.DataLayers
{
    public interface IOrderDAL

    {
        /// <summary>
        /// Lấy 1 chi tiết đơn hàng bằng mã đơn hàng và id mặt hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        OrderDetail? GetDetail(int orderID, int productID);
        /// <summary>
        /// Lấy 1 đơn hàng bằng ID
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        Order? Get(int orderID);

        ///	<summary>

        ///	Tìm kiếm và lấy danh sách đơn hàng dưới dạng phân trang

        ///	</summary>

        IList<Order> List(int page = 1, int pageSize = 0,

        int status = 0, DateTime? fromTime = null, DateTime? toTime = null, string searchValue = "");
        /// <summary>
        /// Lấy danh sách chi tiết đơn hàng bằng ID đơn hàng
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        IList<OrderDetail> ListDetails(int orderID);
        ///	<summary>

        ///	Đếm số lượng đơn hàng thỏa điều kiện tìm kiếm

        ///	</summary>


        int Count(int status = 0, DateTime? fromTime = null, DateTime? toTime = null, string searchValue = "");

        ///	<summary>

        ///	Lấy thông tin đơn hàng theo mã đơn hàng

        ///	</summary>

        ///	<summary>

        ///	Bổ sung đơn hàng mới

        ///	</summary>

        int Add(Order data);

        ///	<summary>

        ///	Cập nhật thông tin của đơn hàng

        ///	</summary>

        bool Update(Order data);

        ///	<summary>

        ///	Xóa đơn hàng và chi tiết của đơn hàng

        ///	</summary>

        bool Delete(int orderID);

        ///	<summary>

        ///	Lấy danh sách hàng được bán trong đơn hàng (chi tiết đơn hàng)

        ///	</summary>

        ///	<summary>

        ///	Lấy thông tin của 1 mặt hàng được bán trong đơn hàng (1 chi tiết trong đơn hàng)

        ///	</summary>

        ///	<summary>

        ///	Thêm mặt hàng được bán trong đơn hàng) theo nguyên tắc:

        ///	- Nếu mặt hàng chưa có trong chi tiết đơn hàng thì bổ sung

        ///	- Nếu mặt hàng đã có trong chi tiết đơn hàng thì cập nhật lại số lượng và giá bán

        ///	</summary>

        bool SaveDetail(int orderID, int productID, int quantity, decimal salePrice);

        ///	<summary>

        ///	Xóa 1 mặt hàng ra khỏi đơn hàng

        ///	</summary>

        bool DeleteDetail(int orderID, int productID);
        bool SaveAddressOrder(int orderID, string DeliveryAddress, string DeliveryProvince);

    }

}
