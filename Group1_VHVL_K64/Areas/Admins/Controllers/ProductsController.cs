using Group1_VHVL_K64.Areas.Admins.Models;
using Group1_VHVL_K64.Models.DBModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Group1_VHVL_K64.Areas.Admins.Controllers
{
    public class ProductsController : AdminBaseController
    {
        private readonly HoangHoanFurnitureContext _context;

        public ProductsController(HoangHoanFurnitureContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            await FillCategoryDropdownAsync(null);

            AdminProductFormViewModel model = new AdminProductFormViewModel
            {
                RateByStars = 0
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminProductFormViewModel model)
        {
            await FillCategoryDropdownAsync(model.CategoryId);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Product newProduct = new Product
            {
                Name = model.Name.Trim(),
                Details = string.IsNullOrWhiteSpace(model.Details) ? null : model.Details.Trim(),
                Price = model.Price,
                NewPrice = model.NewPrice,
                RateByStars = model.RateByStars,
                Urlimage = string.IsNullOrWhiteSpace(model.Urlimage) ? null : model.Urlimage.Trim(),
                CategoryId = model.CategoryId
            };

            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã thêm sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await FillCategoryDropdownAsync(product.CategoryId);

            AdminProductFormViewModel model = MapEntityToForm(product);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminProductFormViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            await FillCategoryDropdownAsync(model.CategoryId);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Product? product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = model.Name.Trim();
            product.Details = string.IsNullOrWhiteSpace(model.Details) ? null : model.Details.Trim();
            product.Price = model.Price;
            product.NewPrice = model.NewPrice;
            product.RateByStars = model.RateByStars;
            product.Urlimage = string.IsNullOrWhiteSpace(model.Urlimage) ? null : model.Urlimage.Trim();
            product.CategoryId = model.CategoryId;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã cập nhật sản phẩm thành công.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product? product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            int orderLineCount = await _context.OrderDetails.CountAsync(od => od.ProductId == product.Id);
            ViewBag.OrderLineCount = orderLineCount;

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product? product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            bool hasOrderLines = await _context.OrderDetails.AnyAsync(od => od.ProductId == id);
            if (hasOrderLines)
            {
                TempData["ErrorMessage"] = "Không xóa được: sản phẩm đã nằm trong ít nhất một đơn hàng.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đã xóa sản phẩm.";
            return RedirectToAction(nameof(Index));
        }

        private async Task FillCategoryDropdownAsync(int? selectedCategoryId)
        {
            List<Category> categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            List<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem { Value = "", Text = "-- Chọn danh mục --", Selected = !selectedCategoryId.HasValue }
            };

            foreach (Category c in categories)
            {
                bool selected = selectedCategoryId.HasValue && c.Id == selectedCategoryId.Value;
                items.Add(new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name,
                    Selected = selected
                });
            }

            ViewBag.CategoryOptions = items;
        }

        private static AdminProductFormViewModel MapEntityToForm(Product product)
        {
            return new AdminProductFormViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Details = product.Details,
                Price = product.Price,
                NewPrice = product.NewPrice,
                RateByStars = product.RateByStars,
                Urlimage = product.Urlimage,
                CategoryId = product.CategoryId
            };
        }
    }
}
