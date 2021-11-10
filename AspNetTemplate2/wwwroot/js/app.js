
// Document Ready ...

$(document).ready(function() {
    // ...
});

jQuery(function() {
    // ...
});

$(function() {
    // ...
});

// .. Document Ready


// Application Theme:

$(function ()
{
    var theme = (localStorage.getItem('theme') == null) ? ("default") : (localStorage.getItem('theme'));
    $("link#bootstrap").attr("href", `../css/themes/${theme}/bootstrap.min.css`);
});

function ChangeTheme(themeName) {    
    localStorage.setItem('theme', themeName);
}

// Application Theme:


(function ($) {
	"use strict";

	// extend jquery here:

})(jQuery);