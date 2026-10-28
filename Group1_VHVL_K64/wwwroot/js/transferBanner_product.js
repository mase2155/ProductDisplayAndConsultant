$(document).ready(function () {
    const $slides = $('.banner-item');
    const $dots = $('.dot');
    let currentIndex = 0;

    function updateBanner(newIndex) {
        $slides.eq(currentIndex).removeClass('active').fadeOut(500);
        $slides.eq(newIndex).addClass('active').fadeIn(500);

        $dots.removeClass('active');
        $dots.eq(newIndex).addClass('active');

        currentIndex = newIndex;
    }

    $('#nextBtn').click(function () {
        let nextIndex = (currentIndex + 1) % $slides.length;
        updateBanner(nextIndex);
    });

    $('#prevBtn').click(function () {
        let prevIndex = (currentIndex - 1 + $slides.length) % $slides.length;
        updateBanner(prevIndex);
    });

    $dots.click(function () {
        let dotIndex = $(this).index();
        updateBanner(dotIndex);
    });
});