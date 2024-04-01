using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SV20T1020091.BusinessLayers;
using SV20T1020091.DomainModels;
using SV20T1020091.Web.Models;
using SV20T1020091.Web;
using System;

namespace SV20T1020091.Web.Controllers
{
    [Authorize(Roles = $"{WebUserRoles.Employee}")]
    public class ProductController : Controller
    {
        private const int PAGE_SIZE = 20;
        private const string PRODUCT_SEARCH = "product_search";

        public IActionResult Index()
        {
            //Lấy đầu vào tìm kiếm hiện đang lưu trong session
            ProductSearchInput input = ApplicationContext.GetSessionData<ProductSearchInput>(PRODUCT_SEARCH);
            //Trường hợp trong session chưa có điều kiện thì tạo điều kiện mới 
            if (input == null)
            {
                input = new ProductSearchInput()
                {
                    Page = 1,
                    PageSize = PAGE_SIZE,
                    SearchValue = "",
                    CategoryID = 0,
                    SupplierID = 0,
                    maxPrice = 0,
                    minPrice = 0

                };
            }
            return View(input);
        }
        public IActionResult Search(ProductSearchInput input)
        {
            int rowCount = 0;
            var data = ProductDataService.ListProducts(out rowCount, input.Page, input.PageSize, input.SearchValue ?? "", input.CategoryID, input.SupplierID, input.minPrice, input.maxPrice);
            var model = new ProductSearchResult()
            {
                Page = input.Page,
                PageSize = input.PageSize,
                SearchValue = input.SearchValue ?? "",
                SupplierID = input.SupplierID,
                CategoryID = input.CategoryID,
                RowCount = rowCount,
                MinPrice = input.minPrice,
                MaxPrice = input.maxPrice,
                Data = data
            };
            //Lưu lại điều kiện tìm kiếm vào trong session
            ApplicationContext.SetSessionData(PRODUCT_SEARCH, input);
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Title = "Bổ sung mặt hàng";
            ViewBag.IsEdit = false;
            Product model = new Product()
            {
                ProductID = 0,
                Photo = "nophotoproduct.png"
            };

            return View("Edit", model);
        }
        public IActionResult Edit(int id = 0)
        {
            ViewBag.Title = "Cập nhật thông tin mặt hàng";
            ViewBag.IsEdit = true;
            Product? model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            if (string.IsNullOrEmpty(model.Photo))
            {
                model.Photo = "nophotoproduct.png";
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult Save(Product data, decimal priceInput, IFormFile? uploadPhoto)
        {
            //Xử lý giá
            decimal? price = priceInput;
            if (price.HasValue)
            {
                data.Price = price.Value;
            }
            //xử lý ảnh upload(nếu có ảnh upload thì lưu ảnh và gán tên file ảnh mới cho product)
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";//tên file để lưu 
                string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, "images\\products");//Đường dẫn đến thư mục lưu file
                string filePath = Path.Combine(folder, fileName);//đường dẫn đến file cần lưu
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            try
            {
                ViewBag.Title = data.ProductID == 0 ? "Bổ sung mặt hàng" : "Cập nhật thông tin mặt hàng";
                //Kiểm soát đầu vào và đưa các thông báo lỗi vào trong ModelState(nếu có)
                if (string.IsNullOrWhiteSpace(data.ProductName))
                {
                    ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng không được để trống");
                }
                if (string.IsNullOrWhiteSpace(data.ProductDescription))
                {
                    ModelState.AddModelError(nameof(data.ProductDescription), "Mô tả không được để trống");
                }
                if (data.CategoryID == 0 || string.IsNullOrEmpty(data.CategoryID.ToString()))
                {
                    ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng chọn loại hàng");
                    TempData["CategoryError"] = "Vui lòng chọn loại hàng";
                }
                if (data.SupplierID == 0 || string.IsNullOrEmpty(data.SupplierID.ToString()))
                {
                    ModelState.AddModelError(nameof(data.CategoryID), "Vui lòng chọn nhà cung cấp");
                    TempData["SupplierError"] = "Vui lòng chọn nhà cung cấp";
                }

                if (string.IsNullOrWhiteSpace(data.Unit))
                {
                    ModelState.AddModelError(nameof(data.Unit), "Đơn vị tính không được để trống");
                }
                if (data.Price == 0)
                {
                    ModelState.AddModelError(nameof(data.Price), "Giá không được để trống");
                }
                //Thông qua thuộc tính Isvalid của ModelState để kiểm tra xem có tồn tại lỗi hay không
                if (!ModelState.IsValid)
                {

                    return View("Edit", data);
                }
                if (data.ProductID == 0)
                {
                    int id = ProductDataService.AddProduct(data);
                    if (id <= 0)
                    {
                        ModelState.AddModelError(nameof(data.ProductName), "Tên mặt hàng bị trùng");
                        return View("Edit", data);
                    }

                }
                else
                {
                    bool result = ProductDataService.UpdateProduct(data);

                }
                return RedirectToAction("Index");
                //return Json(data);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public IActionResult Delete(int id)
        {
            if (Request.Method == "POST")
            {
                ProductDataService.DeleteProduct(id);
                return RedirectToAction("Index");
            }
            var model = ProductDataService.GetProduct(id);
            if (model == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.AllowDelete = !ProductDataService.IsUserProduct(id);

            return View(model);
        }

        public IActionResult Photo(int productId, string method, int photoId = 0, ProductPhoto model = null)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung ảnh";
                    model = new ProductPhoto()
                    {
                        PhotoID = 0,
                        ProductID = productId,
                        Photo = "nophotoproduct.png"

                    };

                    return View(model);
                case "edit":
                    ViewBag.Title = "Thay đổi ảnh";
                    model = ProductDataService.GetPhoto(photoId);
                    if (model == null)
                    {
                        return RedirectToAction("Edit", new { id = productId });
                    }
                    if (string.IsNullOrEmpty(model.Photo))
                    {
                        model.Photo = "nophotoproduct.png";
                    }
                    return View(model);
                case "delete":
                    // Xóa ảnh (xóa trực tiếp, không cần confirm)
                    ProductDataService.DeletePhoto(photoId);
                    return RedirectToAction("Edit", new { id = productId });
                default:
                    return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult SavePhoto(ProductPhoto data, IFormFile? uploadPhoto)
        {
            //xử lý ảnh upload(nếu có ảnh upload thì lưu ảnh và gán tên file ảnh mới cho product)
            if (uploadPhoto != null)
            {
                string fileName = $"{DateTime.Now.Ticks}_{uploadPhoto.FileName}";//tên file để lưu 
                string folder = Path.Combine(ApplicationContext.HostEnviroment.WebRootPath, "images\\products");//Đường dẫn đến thư mục lưu file
                string filePath = Path.Combine(folder, fileName);//đường dẫn đến file cần lưu
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    uploadPhoto.CopyTo(stream);
                }
                data.Photo = fileName;
            }

            try
            {
                if (data.PhotoID == 0)
                {
                    long id = ProductDataService.AddPhoto(data);

                }
                else
                {
                    bool result = ProductDataService.UpdatePhoto(data);

                }
                return RedirectToAction("Edit", new { id = data.ProductID });
                //return Json(data);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public IActionResult Attribute(int productId, string method, int attributeId = 0, ProductAttribute model = null)
        {
            switch (method)
            {
                case "add":
                    ViewBag.Title = "Bổ sung thuộc tính";
                    model = new ProductAttribute()
                    {
                        AttributeID = 0,
                        ProductID = productId

                    };

                    return View(model);
                case "edit":
                    ViewBag.Title = "Thay đổi thuộc tính";
                    model = ProductDataService.GetAttribute(attributeId);
                    if (model == null)
                    {
                        return RedirectToAction("Edit", new { id = productId });
                    }
                    return View(model);
                case "delete":
                    ProductDataService.DeleteAttribute(attributeId);
                    return RedirectToAction("Edit", new { id = productId });

                default:
                    return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult SaveAttribute(ProductAttribute data)
        {


            try
            {
                if (data.AttributeID == 0)
                {
                    long id = ProductDataService.AddAttribute(data);

                }
                else
                {
                    bool result = ProductDataService.UpdateAttribute(data);

                }
                return RedirectToAction("Edit", new { id = data.ProductID });
                //return Json(data);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
    }
}
