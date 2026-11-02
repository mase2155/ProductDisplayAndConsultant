document.addEventListener('click', function (event) {
    // Kiểm tra xem phần tử được click có phải là nút thêm vào giỏ hàng không
    const btn = event.target.closest('.btn-add-to-cart');
    if (!btn) return;

    event.preventDefault(); // Ngăn chặn hành vi mặc định nếu có

    // Lấy ID sản phẩm từ thuộc tính data-id
    const productId = btn.getAttribute('data-id');

    // Dùng Fetch API (chuẩn JS thuần hiện đại) để gửi request lên Controller
    fetch('/Cart/AddToCart', {  // <-- Chú ý đoạn này phải là /Cart/AddToCart
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: `productId=${encodeURIComponent(productId)}`
    })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Thông báo thành công (có thể thay bằng toast hoặc thông báo đẹp hơn)
                alert(data.message);

                // (Tùy chọn) Cập nhật số lượng giỏ hàng trên Header nếu có element đó
                const badge = document.querySelector('.cart-count-badge');
                if (badge) {
                    badge.textContent = data.totalCartItems;
                }
            } else {
                alert(data.message || "Có lỗi xảy ra!");
            }
        })
        .catch(error => {
            console.error('Lỗi:', error);
            alert("Đã có lỗi xảy ra, vui lòng thử lại!");
        });
});