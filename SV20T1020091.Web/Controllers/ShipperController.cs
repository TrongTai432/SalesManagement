using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020091.BusinessLayers;
using SV20T1020091.DomainModels;
using SV20T1020091.Web.Models;
using System.Reflection;

namespace SV20T1020091.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Administrator}, {WebUserRoles.Employee}")]
    public class ShipperController : Controller
    {
        const int PAGE_SIZE = 20;
        const string SHIPPER_SEARCH = "shipper_search";
        public IActionResult Index(int page = 1, string searchvalue = "")
        {
            Models.PaginationSearchInput? input = ApplicationContext.GetSessionData<PaginationSearchInput>(SHIPPER_SEARCH);
            if (input == null)
            {
                input = new PaginationSearchInput
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                };

            }
            return View(input);
        }


        public IActionResult Search(PaginationSearchInput input)
        {
            int rowCount = 0;
            var data = CommonDataService.ListOfShippers(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "");
            var model = new ShipperSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                RowCount = rowCount,
                Data = data
            };

            //Lưu lại điều kiênn tim kiếm
            ApplicationContext.SetSessionData(SHIPPER_SEARCH, input);
            return View(model);
        }

        public IActionResult Create()
        {
            ViewBag.Tile = "Bổ Sung Người Giao Hàng";
            var model = new Shipper()
            {
                ShipperID = 0
            };
            return View("Edit", model);
        }

        public IActionResult Edit(int id = 0)
        {
            ViewBag.Tile = "Cập Nhật Thông Tin Người Giao Hàng";
            var model = CommonDataService.GetShipper(id);
            if (model == null)
                return RedirectToAction("Index");
            return View(model);

        }


        [HttpPost] // Attribut chỉ nhận dũ liệu gửi lên dưới dạng post
        public IActionResult Save(Shipper model)
        {
            //TODO: kiểm soát dữ liệu ở trong model có hợp lệ không
            //Yc: Ten KH,GD,EMAIL,TỈNH không được để trống
            if (string.IsNullOrWhiteSpace(model.ShipperName))
                ModelState.AddModelError(nameof(model.ShipperName), "Tên không được để trống");

            if (string.IsNullOrWhiteSpace(model.Phone))
                ModelState.AddModelError(nameof(model.Phone), "Vui lòng nhập số điện thoại");

            if (!ModelState.IsValid)
            {

                ViewBag.Tile = model.ShipperID == 0 ? "Bổ Sung Người Giao Hàng" : "Cập Nhật Thông Tin Người Giao Hàng";
                return View("Edit", model);
            }
            //AddError :lưu trữ thông báo lỗi
            //Nếu có dùng mode thì tên thông báo lỗi nên trùng vơi thuộc tính

            if (model.ShipperID == 0)
            {
                int id = CommonDataService.AddShipper(model);
                if (id <= 0)
                {
                    ModelState.AddModelError("Phone", "Số điện thoại bị trùng");
                    ViewBag.Tile = "Bổ Sung Người Giao Hàng";
                    return View("Edit", model);

                }
            }

            else
            {
                bool result = CommonDataService.UpdateShipper(model);

                if (!result)
                {
                    ModelState.AddModelError("Error", "Không cập nhật được. Có thể số điện thoại bị trùng");
                    ViewBag.Tile = "Cập Nhật Thông người Hàng";
                    return View("Edit", model);
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id = 0)
        {
            if (Request.Method == "POST")
            {
                bool result = CommonDataService.DeleteShipper(id);
                return RedirectToAction("Index");
            }
            var model = CommonDataService.GetShipper(id);
            if (model == null)
                return RedirectToAction("Index");

            return View(model);
        }
    }
}
