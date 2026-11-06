using Group1_VHVL_K64.Models.Chatbot;
using Group1_VHVL_K64.Models.DBModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group1_VHVL_K64.Controllers
{
    /// <summary>
    /// Trợ lý tư vấn: chỉ trả Razor View (không trả Json), tải trong iframe widget.
    /// </summary>
    public class ChatbotController : Controller
    {
        private readonly HoangHoanFurnitureContext _context;

        public ChatbotController(HoangHoanFurnitureContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Khung nội dung chatbot (GET — luồng welcome / category / product).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Frame(string step = "welcome", int id = 0)
        {
            ChatbotStepResponse model;

            if (step == "category")
            {
                model = await BuildCategoryStepAsync(id);
            }
            else if (step == "product")
            {
                model = await BuildProductStepAsync(id);
            }
            else
            {
                model = await BuildWelcomeStepAsync();
            }

            return View("Frame", model);
        }

        private async Task<ChatbotStepResponse> BuildWelcomeStepAsync()
        {
            List<Category> categories = await _context.Categories
                .OrderBy(c => c.Name)
                .ToListAsync();

            ChatbotStepResponse response = new ChatbotStepResponse
            {
                BotMessage = "Xin chào! Em là trợ lý Hoàng Hoan. Anh/chị chọn loại mặt hàng bên dưới nhé."
            };

            foreach (Category category in categories)
            {
                response.Options.Add(new ChatbotOptionButton
                {
                    Label = category.Name,
                    StepType = "category",
                    TargetId = category.Id
                });
            }

            if (response.Options.Count == 0)
            {
                response.BotMessage = "Hiện chưa có danh mục sản phẩm. Anh/chị vui lòng xem trang Sản phẩm hoặc liên hệ shop.";
            }

            return response;
        }

        private async Task<ChatbotStepResponse> BuildCategoryStepAsync(int categoryId)
        {
            Category? category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                return CreateErrorStep("Không tìm thấy danh mục này.");
            }

            ChatbotStepResponse response = new ChatbotStepResponse
            {
                BotMessage = "Danh mục **" + category.Name + "**. Anh/chị chọn sản phẩm:"
            };

            foreach (Product product in category.Products.OrderBy(p => p.Name))
            {
                response.Options.Add(new ChatbotOptionButton
                {
                    Label = product.Name,
                    StepType = "product",
                    TargetId = product.Id
                });
            }

            if (response.Options.Count == 0)
            {
                response.BotMessage = "Danh mục **" + category.Name + "** chưa có sản phẩm.";
            }

            response.Options.Add(new ChatbotOptionButton
            {
                Label = "← Quay lại danh mục",
                StepType = "welcome",
                TargetId = 0
            });

            return response;
        }

        private async Task<ChatbotStepResponse> BuildProductStepAsync(int productId)
        {
            Product? product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return CreateErrorStep("Không tìm thấy sản phẩm này.");
            }

            decimal displayPrice = product.NewPrice ?? product.Price;
            string descriptionText = product.Details ?? "Sản phẩm đang cập nhật mô tả chi tiết.";

            ChatbotStepResponse response = new ChatbotStepResponse
            {
                BotMessage = "Sản phẩm **" + product.Name + "** thuộc danh mục "
                    + (product.Category?.Name ?? "")
                    + ". Em gửi anh/chị thông số và mô tả bên dưới:",
                ProductDetail = new ChatbotProductDetailBlock
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Details = product.Details,
                    Description = descriptionText,
                    Price = product.Price,
                    NewPrice = product.NewPrice,
                    RateByStars = product.RateByStars,
                    Urlimage = product.Urlimage,
                    CategoryName = product.Category?.Name ?? "",
                    FormattedPrice = displayPrice.ToString("N0") + " đ"
                }
            };

            if (product.CategoryId.HasValue)
            {
                response.Options.Add(new ChatbotOptionButton
                {
                    Label = "← Chọn sản phẩm khác",
                    StepType = "category",
                    TargetId = product.CategoryId.Value
                });
            }

            response.Options.Add(new ChatbotOptionButton
            {
                Label = "← Quay lại danh mục",
                StepType = "welcome",
                TargetId = 0
            });

            return response;
        }

        private static ChatbotStepResponse CreateErrorStep(string message)
        {
            ChatbotStepResponse response = new ChatbotStepResponse
            {
                BotMessage = message
            };

            response.Options.Add(new ChatbotOptionButton
            {
                Label = "← Quay lại danh mục",
                StepType = "welcome",
                TargetId = 0
            });

            return response;
        }
    }
}
