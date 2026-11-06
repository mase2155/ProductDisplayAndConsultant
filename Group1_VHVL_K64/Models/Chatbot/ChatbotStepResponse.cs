namespace Group1_VHVL_K64.Models.Chatbot
{
    /// <summary>
    /// Dữ liệu một bước hội thoại bot (hiển thị Razor /Chatbot/Frame, chọn bằng nút).
    /// </summary>
    public class ChatbotStepResponse
    {
        public string BotMessage { get; set; } = string.Empty;

        public List<ChatbotOptionButton> Options { get; set; } = new List<ChatbotOptionButton>();

        public ChatbotProductDetailBlock? ProductDetail { get; set; }
    }

    public class ChatbotOptionButton
    {
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// welcome | category | product
        /// </summary>
        public string StepType { get; set; } = string.Empty;

        public int TargetId { get; set; }
    }

    public class ChatbotProductDetailBlock
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Details { get; set; }

        public decimal Price { get; set; }

        public decimal? NewPrice { get; set; }

        public decimal? RateByStars { get; set; }

        public string? Urlimage { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Mô tả chi tiết (cột Details trong Products).
        /// </summary>
        public string Description { get; set; } = string.Empty;

        public string FormattedPrice { get; set; } = string.Empty;
    }
}
