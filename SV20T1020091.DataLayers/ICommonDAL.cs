using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Mô tả phép xử lí dữ liệu "chung chung"(generic)
/// </summary>
namespace SV20T1020091.DataLayers
{
    //ctrl + M + O để thu gọn tất cả lại (M+L để mở rộng ra)
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">trang cần hiển thị</typeparam>
    /// /// <typeparam name="T">số dòng trên mỗi trang (bằng 0 nếu ko phân trang)</typeparam>
    /// /// <typeparam name="T">giá trị tìm kiếm(chuỗi rỗng nếu lấy toàn  bộ dữ liệu</typeparam>
    public interface ICommonDAL<T> where T : class
    {
        IList<T> List(int page=1, int pageSize=0, string searchValue = "");
        /// <summary>
        /// đếm số lượng dòng dữ liệu tìm được
        /// </summary>
        /// <param name="searchValue">giá trị tìm kiếm(chuỗi rỗng nếu lấy toàn  bộ dữ liệu</param>
        /// <returns></returns>
        int Count(string searchValue = "");
        /// <summary>
        /// lấy một bản gh/dòng dữ liệu dựa trên mã
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? Get(int id);

        /// <summary>
        /// bổ sung  dữ liệu vào trong csdl. hàm trả về id của dữ liệu được bổ sung
        /// trả về giá trị nhỏ hơn hoặc bằng 0 nếu lỗi
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        int Add(T data);

        /// <summary>
        /// cập nhật dữ liệu 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool Update(T data);

        /// <summary>
        /// xóa một bản ghi dữ liệu dựa vào id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id);

        /// <summary>
        /// kiểm tra xem một bản ghi dữ liệu có mã id hiện đang được sử dụng bởi các bảng khác hay ko?
        /// có dữ liệu lquan hay ko?
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool IsUsed(int id);
    }
}
