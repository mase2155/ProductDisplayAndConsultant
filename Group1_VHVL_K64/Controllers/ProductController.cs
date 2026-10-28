using Group1_VHVL_K64.Models.DBModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group1_VHVL_K64.Controllers
{
    public class ProductController : Controller
    {
        private readonly HoangHoanFurnitureContext _context;

        public ProductController(HoangHoanFurnitureContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productWithCategory = await _context.Categories.Include(p => p.Products).ToListAsync();
            return View(productWithCategory);
        }
    }
}
