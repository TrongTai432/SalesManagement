using Microsoft.AspNetCore.Mvc.Rendering;
using SV20T1020091.BusinessLayers;

namespace SV20T1020091.Web
{
    public static class SelectListHelper
    {
        /// <summary>
        /// danh sách tỉnh thành
        /// </summary>
        /// <returns></returns>
        public static List<SelectListItem> Provinces()
        {
            List<SelectListItem > list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "Chọn tỉnh thành",
            });  
            foreach (var item in CommonDataService.ListOfProvinces())
            {
                list.Add(new SelectListItem()
                {
                    Value = item.ProvinceName, Text = item.ProvinceName,
                });
            }
            return list;
        }

        public static List<SelectListItem> Category()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "-- Tất cả loại hàng --",

            });
            foreach (var item in CommonDataService.ListOfCategoryName())
            {

                list.Add(new SelectListItem()
                {
                    Value = item.CategoryID.ToString(),
                    Text = item.CategoryName,

                });
            }
            return list;
        }

        public static List<SelectListItem> Supplier()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem()
            {
                Value = "",
                Text = "-- Nhà cung cấp --",

            });
            foreach (var item in CommonDataService.ListOfSupplierName())
            {

                list.Add(new SelectListItem()
                {
                    Value = item.SupplierID.ToString(),
                    Text = item.SupplierName,

                });
            }
            return list;
        }
    }
}
