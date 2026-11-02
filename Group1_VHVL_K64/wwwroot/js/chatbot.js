(function () {
    var toggleBtn = document.getElementById('chatbot-toggle');
    var panel = document.getElementById('chatbot-panel');
    var closeBtn = document.getElementById('chatbot-close');
    var messagesEl = document.getElementById('chatbot-messages');
    var optionsEl = document.getElementById('chatbot-options');

    if (!toggleBtn || !panel) {
        return;
    }

    var hasStarted = false;

    function openPanel() {
        panel.classList.remove('chatbot-panel-hidden');
        panel.setAttribute('aria-hidden', 'false');

        if (!hasStarted) {
            hasStarted = true;
            loadStep('welcome', 0);
        }
    }

    function closePanel() {
        panel.classList.add('chatbot-panel-hidden');
        panel.setAttribute('aria-hidden', 'true');
    }

    function appendUserBubble(text) {
        var div = document.createElement('div');
        div.className = 'chatbot-bubble chatbot-bubble-user';
        div.textContent = text;
        messagesEl.appendChild(div);
        scrollMessagesToBottom();
    }

    function appendBotBubble(htmlOrText, isHtml) {
        var div = document.createElement('div');
        div.className = 'chatbot-bubble chatbot-bubble-bot';
        if (isHtml) {
            div.innerHTML = htmlOrText;
        } else {
            div.textContent = htmlOrText;
        }
        messagesEl.appendChild(div);
        scrollMessagesToBottom();
    }

    function scrollMessagesToBottom() {
        messagesEl.scrollTop = messagesEl.scrollHeight;
    }

    function formatMoney(value) {
        var n = Number(value);
        if (Number.isNaN(n)) {
            return '0';
        }
        return n.toLocaleString('vi-VN');
    }

    function buildProductDetailHtml(detail) {
        var priceText = detail.formattedPrice;
        if (!priceText) {
            if (detail.newPrice != null && detail.newPrice > 0) {
                priceText = formatMoney(detail.newPrice) + ' đ';
            } else {
                priceText = formatMoney(detail.price) + ' đ';
            }
        }

        var stars = detail.rateByStars != null ? detail.rateByStars + ' / 5 sao' : 'Chưa có đánh giá';
        var description = detail.description || detail.details || 'Chưa có mô tả chi tiết.';

        var html = '<div class="chatbot-product-card">';

        if (detail.urlimage) {
            html += '<img src="' + escapeAttr(detail.urlimage) + '" alt="' + escapeAttr(detail.name) + '">';
        }

        html += '<h4>' + escapeHtml(detail.name) + '</h4>';
        html += '<div class="chatbot-meta"><strong>Danh mục:</strong> ' + escapeHtml(detail.categoryName || '') + '</div>';
        html += '<div class="chatbot-meta"><strong>Đánh giá:</strong> ' + escapeHtml(String(stars)) + '</div>';
        html += '<div class="chatbot-meta"><strong>Giá:</strong> <span class="chatbot-price-inline">' + escapeHtml(priceText) + '</span></div>';
        html += '<div class="chatbot-description-title">Mô tả sản phẩm</div>';
        html += '<div class="chatbot-description">' + escapeHtml(description) + '</div>';
        html += '<a class="chatbot-product-link" href="/Product/Index">Xem thêm tại trang Sản phẩm</a>';
        html += '</div>';

        return html;
    }

    function escapeHtml(text) {
        var div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }

    function escapeAttr(text) {
        return String(text).replace(/"/g, '&quot;');
    }

    function renderOptions(options) {
        optionsEl.innerHTML = '';

        if (!options || options.length === 0) {
            return;
        }

        for (var i = 0; i < options.length; i++) {
            var opt = options[i];
            var btn = document.createElement('button');
            btn.type = 'button';
            btn.className = 'chatbot-option-btn';
            btn.textContent = opt.label;

            if (opt.stepType === 'welcome' || (opt.label && opt.label.indexOf('←') === 0)) {
                btn.classList.add('chatbot-option-back');
            }

            btn.addEventListener('click', function (label, stepType, targetId) {
                return function () {
                    appendUserBubble(label);
                    loadStep(stepType, targetId);
                };
            }(opt.label, opt.stepType, opt.targetId));

            optionsEl.appendChild(btn);
        }
    }

    function getStepUrl(stepType, targetId) {
        if (stepType === 'welcome') {
            return '/Chatbot/Welcome';
        }
        if (stepType === 'category') {
            return '/Chatbot/Category?id=' + encodeURIComponent(targetId);
        }
        if (stepType === 'product') {
            return '/Chatbot/Product?id=' + encodeURIComponent(targetId);
        }
        return '/Chatbot/Welcome';
    }

    function normalizeKeys(data) {
        if (data.productDetail) {
            return {
                botMessage: data.botMessage,
                options: data.options || [],
                productDetail: {
                    productId: data.productDetail.productId,
                    name: data.productDetail.name,
                    details: data.productDetail.details,
                    description: data.productDetail.description,
                    price: data.productDetail.price,
                    newPrice: data.productDetail.newPrice,
                    rateByStars: data.productDetail.rateByStars,
                    urlimage: data.productDetail.urlimage,
                    categoryName: data.productDetail.categoryName,
                    formattedPrice: data.productDetail.formattedPrice
                }
            };
        }
        return {
            botMessage: data.botMessage,
            options: data.options || [],
            productDetail: null
        };
    }

    function loadStep(stepType, targetId) {
        optionsEl.innerHTML = '';

        var url = getStepUrl(stepType, targetId);

        fetch(url, {
            headers: { 'X-Requested-With': 'XMLHttpRequest' }
        })
            .then(function (response) {
                return response.json();
            })
            .then(function (raw) {
                var data = normalizeKeys(raw);

                appendBotBubble(data.botMessage.replace(/\*\*/g, ''), false);

                if (data.productDetail) {
                    appendBotBubble(buildProductDetailHtml(data.productDetail), true);
                }

                var opts = (data.options || []).map(function (o) {
                    return {
                        label: o.label,
                        stepType: o.stepType,
                        targetId: o.targetId
                    };
                });

                renderOptions(opts);
            })
            .catch(function () {
                appendBotBubble('Đã có lỗi khi tải dữ liệu. Anh/chị thử đóng mở lại hộp chat.', false);
            });
    }

    toggleBtn.addEventListener('click', openPanel);
    closeBtn.addEventListener('click', closePanel);
})();
