document.addEventListener('DOMContentLoaded', function () {
    const cartPage = document.getElementById('cart-page');
    if (!cartPage) {
        return;
    }

    function formatMoney(value) {
        const numberValue = Number(value);
        if (Number.isNaN(numberValue)) {
            return '0';
        }
        return numberValue.toLocaleString('vi-VN');
    }

    function updateGrandTotalOnPage(grandTotal) {
        const topTotal = document.getElementById('cart-grand-total');
        const footerTotal = document.getElementById('cart-grand-total-footer');

        if (topTotal) {
            topTotal.textContent = formatMoney(grandTotal);
        }
        if (footerTotal) {
            footerTotal.textContent = formatMoney(grandTotal);
        }
    }

    function updateCartBadge(count) {
        const badge = document.querySelector('.cart-count-badge');
        if (!badge) {
            return;
        }
        badge.textContent = count;
        if (count <= 0) {
            badge.style.display = 'none';
        } else {
            badge.style.display = 'inline-flex';
        }
    }

    function postForm(url, bodyObject) {
        const body = new URLSearchParams(bodyObject).toString();

        return fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'X-Requested-With': 'XMLHttpRequest'
            },
            body: body
        }).then(function (response) {
            return response.json();
        });
    }

    function sendQuantityUpdate(productId, quantity) {
        postForm('/Cart/UpdateQuantity', {
            productId: productId,
            quantity: quantity
        })
            .then(function (data) {
                if (!data.success) {
                    alert(data.message || 'Không cập nhật được số lượng.');
                    return;
                }

                const row = document.querySelector('.cart-row[data-product-id="' + productId + '"]');
                if (row) {
                    const lineTotalEl = row.querySelector('.line-total-value');
                    if (lineTotalEl) {
                        lineTotalEl.textContent = formatMoney(data.lineTotal);
                    }
                }

                updateGrandTotalOnPage(data.grandTotal);
                updateCartBadge(data.totalCartItems);
            })
            .catch(function () {
                alert('Đã có lỗi xảy ra, vui lòng thử lại!');
            });
    }

    cartPage.addEventListener('click', function (event) {
        const clearAllBtn = event.target.closest('.btn-clear-all-cart');
        const minusBtn = event.target.closest('.btn-qty-minus');
        const plusBtn = event.target.closest('.btn-qty-plus');
        const removeBtn = event.target.closest('.btn-remove-from-cart');

        if (clearAllBtn) {
            if (!confirm('Bạn có chắc muốn xóa toàn bộ sản phẩm trong giỏ hàng?')) {
                return;
            }

            postForm('/Cart/ClearCart', {})
                .then(function (data) {
                    if (!data.success) {
                        alert(data.message || 'Không xóa được giỏ hàng.');
                        return;
                    }

                    updateCartBadge(0);
                    window.location.reload();
                })
                .catch(function () {
                    alert('Đã có lỗi xảy ra, vui lòng thử lại!');
                });
            return;
        }

        if (minusBtn) {
            const productId = minusBtn.getAttribute('data-id');
            const input = cartPage.querySelector('.qty-input[data-id="' + productId + '"]');
            if (!input) {
                return;
            }
            let quantity = parseInt(input.value, 10);
            if (quantity <= 1) {
                return;
            }
            quantity -= 1;
            input.value = quantity;
            sendQuantityUpdate(productId, quantity);
            return;
        }

        if (plusBtn) {
            const productId = plusBtn.getAttribute('data-id');
            const input = cartPage.querySelector('.qty-input[data-id="' + productId + '"]');
            if (!input) {
                return;
            }
            let quantity = parseInt(input.value, 10);
            if (quantity >= 99) {
                return;
            }
            quantity += 1;
            input.value = quantity;
            sendQuantityUpdate(productId, quantity);
            return;
        }

        if (removeBtn) {
            const productId = removeBtn.getAttribute('data-id');
            if (!confirm('Bạn có chắc muốn xóa sản phẩm này khỏi giỏ hàng?')) {
                return;
            }

            postForm('/Cart/RemoveFromCart', { productId: productId })
                .then(function (data) {
                    if (!data.success) {
                        alert(data.message || 'Không xóa được sản phẩm.');
                        return;
                    }

                    if (data.isEmpty) {
                        window.location.reload();
                        return;
                    }

                    const row = document.querySelector('.cart-row[data-product-id="' + productId + '"]');
                    if (row) {
                        row.remove();
                    }

                    updateGrandTotalOnPage(data.grandTotal);
                    updateCartBadge(data.totalCartItems);
                })
                .catch(function () {
                    alert('Đã có lỗi xảy ra, vui lòng thử lại!');
                });
        }
    });

    cartPage.addEventListener('change', function (event) {
        const input = event.target.closest('.qty-input');
        if (!input) {
            return;
        }

        const productId = input.getAttribute('data-id');
        let quantity = parseInt(input.value, 10);

        if (Number.isNaN(quantity) || quantity < 1) {
            quantity = 1;
            input.value = '1';
        }
        if (quantity > 99) {
            quantity = 99;
            input.value = '99';
        }

        sendQuantityUpdate(productId, quantity);
    });
});
