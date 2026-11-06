/**
 * Chatbot: mở panel và tải nội dung Razor qua iframe (GET /Chatbot/Frame) — không dùng fetch/JSON.
 */
(function () {
    var toggleBtn = document.getElementById('chatbot-toggle');
    var panel = document.getElementById('chatbot-panel');
    var closeBtn = document.getElementById('chatbot-close');
    var iframe = document.getElementById('chatbot-iframe');

    if (!toggleBtn || !panel || !iframe) {
        return;
    }

    var hasLoaded = false;
    var frameUrl = '/Chatbot/Frame?step=welcome';

    function openPanel() {
        panel.classList.remove('chatbot-panel-hidden');
        panel.setAttribute('aria-hidden', 'false');

        if (!hasLoaded) {
            hasLoaded = true;
            iframe.setAttribute('src', frameUrl);
        }
    }

    function closePanel() {
        panel.classList.add('chatbot-panel-hidden');
        panel.setAttribute('aria-hidden', 'true');
    }

    toggleBtn.addEventListener('click', openPanel);
    closeBtn.addEventListener('click', closePanel);
})();
