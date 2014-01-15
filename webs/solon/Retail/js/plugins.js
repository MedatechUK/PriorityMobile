// Avoid `console` errors in browsers that lack a console.
(function() {
    var method;
    var noop = function () {};
    var methods = [
        'assert', 'clear', 'count', 'debug', 'dir', 'dirxml', 'error',
        'exception', 'group', 'groupCollapsed', 'groupEnd', 'info', 'log',
        'markTimeline', 'profile', 'profileEnd', 'table', 'time', 'timeEnd',
        'timeStamp', 'trace', 'warn'
    ];
    var length = methods.length;
    var console = (window.console = window.console || {});

    while (length--) {
        method = methods[length];

        // Only stub undefined methods.
        if (!console[method]) {
            console[method] = noop;
        }
    }
}());

// Place any jQuery/helper plugins in here.

//jQuery(function($) {
    //console.log("jquery (plugins) ready");

   function startSlideshow(){
        var $slider = $("#slider p");
        //console.log("slider", $slider.length);
        if ($slider.find('img').length > 1) {
            $.each($slider.find('img'), function(i, v) {
                $(this).attr("data-thumb", $(this).attr("src"));
            });

            $slider
			.addClass("nivoSlider")
			.nivoSlider({
			    effect: 'fade', // Specify sets like: 'fold,fade,sliceDown'
			    slices: 15, // For slice animations
			    boxCols: 8, // For box animations
			    boxRows: 4, // For box animations
			    animSpeed: 1, // Slide transition speed
			    pauseTime: 3000, // How long each slide will show
			    startSlide: 0, // Set starting Slide (0 index)
			    directionNav: true, // Next & Prev navigation
			    controlNav: true, // 1,2,3... navigation
			    controlNavThumbs: true, // Use thumbnails for Control Nav
			    pauseOnHover: true, // Stop animation while hovering
			    manualAdvance: true, // Force manual transitions
			    prevText: 'Prev', // Prev directionNav text
			    nextText: 'Next', // Next directionNav text
			    randomStart: false, // Start on a random slide
			    beforeChange: function() { }, // Triggers before a slide transition
			    afterChange: function() { }, // Triggers after a slide transition
			    slideshowEnd: function() { }, // Triggers after all slides have been shown
			    lastSlide: function() { }, // Triggers when last slide is shown
			    afterLoad: function() { } // Triggers when slider has loaded
			});
            /*
            sliceDown
            sliceDownLeft
            sliceUp
            sliceUpLeft
            sliceUpDown
            sliceUpDownLeft
            fold
            fade
            random
            slideInRight
            slideInLeft
            boxRandom
            boxRain
            boxRainReverse
            boxRainGrow
            boxRainGrowReverse
            */
        }
    };

//});
