using System.Globalization;

namespace SV20T1020091.Web
{
    public static class Converter
    {
        /// <summary>
        /// chuyển chuỗi s sang kiểu datetime theo các format được quy định
        /// hàm trả về null nếu ko thành công
        /// </summary>
        /// <param name="s"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string s, string formats = "d/M/yyyy;d-M-yyyy;d.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, formats.Split(';'), CultureInfo.InvariantCulture);

            }
            catch
            {
                return null;
            }
        }
    }
}
