using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020091.BusinessLayers
{
    /// <summary>
    /// khởi tạo và lưu trữ các thông tin cấu hình của businesslayer
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// chuỗi thông số kết nối với csdl
        /// </summary>
        public static string ConnectionString { get; private set; } = "";

        /// <summary>
        /// hàm khởi tạo cấu hình cho businesslayer
        /// hàm này phải được gọi trc khi chạy ứng dụng
        /// </summary>
        /// <param name="connectionString"></param>
        public static void Initialize(string connectionString)
        {
            Configuration.ConnectionString = connectionString;
        }
    }
}
