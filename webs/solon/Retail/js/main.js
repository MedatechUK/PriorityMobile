//MAIN JS FUNCTIONS.. plugins should be in plugins.js

/*HTML5 polyfills*/
Modernizr.load({
    test: Modernizr.meter,
    nope: ['js/meter-polyfill.js','css/meter-polyfill.css']
});


//does browser support microdata?
function supports_microdata_api() {
  return !!document.getItems;
}


(function($) {
    //$(".no-js").removeClass(".no-js").addClass("js"); // this will probably be done by moderizr.
    //Run any jquery interactivity here... menus/form validation/mouse events/slideshows etc.


    //HIGHLIGHT THE MENU ITEMS

    $('a[href="' + location.pathname.substr(1) + '"]').parent("li").addClass("active");
})(jQuery);

