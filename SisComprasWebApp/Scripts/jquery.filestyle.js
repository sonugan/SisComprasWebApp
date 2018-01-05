/*
 * Style File - jQuery plugin for styling file input elements
 * Copyright (c) 2007-2009 Mika Tuupola
 * Licensed under the MIT license:
 *   http://www.opensource.org/licenses/mit-license.php
 * Based on work by Shaun Inman
 *   http://www.shauninman.com/archive/2007/09/10/styling_file_inputs_with_css_and_the_dom
 */
/* 
	REFORMADO POR SOFRECOM
*/
(function($) {
    $.fn.filestyle = function(options) {
        var settings = {
            width : 250
        };
                
        if(options) {
            $.extend(settings, options);
        };
                        
        return this.each(function() {
            
            var self = this;
                            
            var filename = $('<input type="text">')
                             .css({
                                 "display": "inline",
								 "width": settings.width + "px"
                             });
							 
			var filename2 = $('<div class="boton-upload">Examinar</div>');

			$(this).before(filename);
			$(this).before(filename2);

            $(this).css({
                        "position": "absolute",
                        "height": settings.imageheight + "px",
                        "width": settings.width + 80 + 15 + "px",
                        "display": "inline",
                        "cursor": "pointer",
                        "opacity": "0.0",
						"margin-left": 	- settings.width - 11 + "px"
                    });

            $(this).bind("change", function() {
                filename.val($(self).val());
            });
      
        });
       
    };
    
})(jQuery);