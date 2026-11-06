using Group1_VHVL_K64.Areas.Admins.Models;
using Group1_VHVL_K64.Models.DBModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Group1_VHVL_K64.Areas.Admins.Controllers
{
    public class OrdersController : AdminBaseController
    {
        private readonly HoangHoanFurnitureContext _context;

        private static readonly string[] OrderStatusList =
        {
            "Pending",
            "Processing",
            "Shipped",
            "Completed",
            "Cancelled"
        };

        public OrdersController(HoangHoanFurnitureContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Order> orders = await _context.Orders
                .Include(o => o.Customer)
                .OrderBy(o => o.Id)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order? order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public async Task<IActionResult> Create()
        {
            OrderFormModel formModel = CreateEmptyFormModel();
            await FillDropdownsAsync(formModel);
            return View(formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderFormModel formModel)
        {
            await FillDropdownsAsync(formModel);

            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            List<OrderDetail> validDetails = await BuildOrderDetailsFromFormAsync(formModel);
            if (validDetails.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng thêm ít nhất một sản phẩm hợp lệ.");
                return View(formModel);
            }

            decimal totalAmount = 0;
            foreach (OrderDetail detail in validDetails)
            {
                totalAmount += detail.LineTotal;
            }

            Order newOrder = new Order
            {
                CustomerId = formModel.CustomerId,
                OrderDate = formModel.OrderDate,
                Status = formModel.Status,
                ShippingAddress = formModel.ShippingAddress,
                Phone = formModel.Phone,
                Note = formModel.Note,
                TotalAmount = totalAmount
            };

            foreach (OrderDetail detail in validDetails)
            {
                newOrder.OrderDetails.Add(detail);
            }

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Tạo đơn hàng thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order? order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            OrderFormModel formModel = MapOrderToFormModel(order);
            await FillDropdownsAsync(formModel);
            return View(formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderFormModel formModel)
        {
            if (id != formModel.Id)
            {
                return NotFound();
            }

            await FillDropdownsAsync(formModel);

            if (!ModelState.IsValid)
            {
                return View(formModel);
            }

            Order? order = await _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            List<OrderDetail> validDetails = await BuildOrderDetailsFromFormAsync(formModel);
            if (validDetails.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Vui lòng thêm ít nhất một sản phẩm hợp lệ.");
                return View(formModel);
            }

            decimal totalAmount = 0;
            foreach (OrderDetail detail in validDetails)
            {
                totalAmount += detail.LineTotal;
            }

            order.CustomerId = formModel.CustomerId;
            order.OrderDate = formModel.OrderDate;
            order.Status = formModel.Status;
            order.ShippingAddress = formModel.ShippingAddress;
            order.Phone = formModel.Phone;
            order.Note = formModel.Note;
            order.TotalAmount = totalAmount;

            _context.OrderDetails.RemoveRange(order.OrderDetails);
            foreach (OrderDetail detail in validDetails)
            {
                order.OrderDetails.Add(detail);
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật đơn hàng thành công.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order? order = await _context.Orders
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Order? order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa đơn hàng.";
            }

            return RedirectToAction(nameof(Index));
        }

        private OrderFormModel CreateEmptyFormModel()
        {
            OrderFormModel formModel = new OrderFormModel
            {
                OrderDate = DateTime.Now,
                Status = "Pending"
            };

            for (int i = 0; i < 3; i++)
            {
                formModel.Details.Add(new OrderDetailFormLine { Quantity = 1 });
            }

            return formModel;
        }

        private static OrderFormModel MapOrderToFormModel(Order order)
        {
            OrderFormModel formModel = new OrderFormModel
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                Phone = order.Phone,
                Note = order.Note
            };

            foreach (OrderDetail detail in order.OrderDetails)
            {
                formModel.Details.Add(new OrderDetailFormLine
                {
                    ProductId = detail.ProductId,
                    Quantity = detail.Quantity
                });
            }

            while (formModel.Details.Count < 3)
            {
                formModel.Details.Add(new OrderDetailFormLine { Quantity = 1 });
            }

            return formModel;
        }

        private async Task FillDropdownsAsync(OrderFormModel formModel)
        {
            List<Customer> customers = await _context.Customers
                .OrderBy(c => c.Fullname)
                .ToListAsync();

            ViewBag.CustomerId = new SelectList(customers, "Id", "Fullname", formModel.CustomerId);

            List<Product> products = await _context.Products
                .OrderBy(p => p.Name)
                .ToListAsync();

            ViewBag.ProductList = products;
            ViewBag.StatusList = new SelectList(OrderStatusList, formModel.Status);
        }

        private async Task<List<OrderDetail>> BuildOrderDetailsFromFormAsync(OrderFormModel formModel)
        {
            List<OrderDetail> result = new List<OrderDetail>();

            if (formModel.Details == null)
            {
                return result;
            }

            foreach (OrderDetailFormLine line in formModel.Details)
            {
                if (line.ProductId <= 0 || line.Quantity <= 0)
                {
                    continue;
                }

                Product? product = await _context.Products.FindAsync(line.ProductId);
                if (product == null)
                {
                    continue;
                }

                decimal unitPrice = product.NewPrice ?? product.Price;
                decimal lineTotal = unitPrice * line.Quantity;

                result.Add(new OrderDetail
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = line.Quantity,
                    UnitPrice = unitPrice,
                    LineTotal = lineTotal
                });
            }

            return result;
        }
    }
}
