// the functions in this file require the supplementary library lib.js

// These defaults should be changed the way it best fits your site
var _POPUP_FEATURES = 'location=1,statusbar=1,scrollbars=1,menubar=1,width=600,height=400';

function raw_popup(url, target, features) {
    // pops up a window containing url optionally named target, optionally having features
    if (isUndefined(features)) features = _POPUP_FEATURES;
    if (isUndefined(target  )) target   = '_blank';
    var theWindow = window.open(url, target, features);
    theWindow.focus();
    return theWindow;
}