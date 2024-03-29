function GetWithoutSpecialChar(str) { return str.replace(/[^0-9a-zA-Z]+/g, '-'); }

if (!String.prototype.trim) { String.prototype.trim = function () { return this.replace(/^\s+|\s+$/g, ''); }; }
if (!String.prototype.Contains) { String.prototype.Contains = function (str) { return this.indexOf(str) !== -1; }; }
if (!String.prototype.startsWith) { String.prototype.startsWith = function (str) { return !this.indexOf(str); }; }
if (!String.prototype.endsWith) { String.prototype.endsWith = function (suffix) { return this.indexOf(suffix, this.length - suffix.length) !== -1; };}

function isNumber(evt) {
    evt = evt ? evt : window.event;
    var charCode = evt.which ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function isTextOnly(evt) {
    evt = evt ? evt : window.event;
    var charCode = evt.which ? evt.which : evt.keyCode;
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode === 9) || charCode === 8) {
        return true;
    }
    return false;
}

function isAlphaNumeric(evt) {
    evt = evt ? evt : window.event;
    var charCode = evt.which ? evt.which : evt.keyCode;
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode === 9) || (charCode > 47 && charCode < 58) || charCode === 32 || charCode === 8 || charCode === 13) {
        return true;
    }
    return false;
}

function isAlphaNumericAndSpecialChar(evt) {
    evt = evt ? evt : window.event;
    var charCode = evt.which ? evt.which : evt.keyCode;
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode === 9) || (charCode > 47 && charCode < 58) || charCode === 32 || charCode === 45 || charCode === 95 || charCode === 8 || charCode === 46 || charCode === 44 || charCode === 47 || charCode === 92 || charCode === 58 || charCode === 38 || charCode === 37 || charCode === 39) {
        return true;
    }
    return false;
}

function isEmailChar(evt) {
    evt = evt ? evt : window.event;
    console.log(evt);
    var charCode = evt.which ? evt.which : evt.keyCode;
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || (charCode > 47 && charCode < 58) || charCode === 45 || charCode === 95 || charCode === 8 || charCode === 46) {
        return true;
    }
    return false;
}

function isDecimal(evt) {
    evt = evt ? evt : window.event;
    var charCode = evt.which ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode !== 46 && (charCode < 48 || charCode > 57 || charCode === 9))) {
        return false;
    }
    return true;
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function validateEmail(email) {
    var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return re.test(email);
}

function validateUrl(value) {
    return /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(value);
}

var month = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];


function GetMonthAndYear() {
    var d = new Date();
    var yr = d.getFullYear();
    var n = month[d.getMonth()];
    return n + ' ' + yr;
}





var isMobile = {
    Android: function () { return navigator.userAgent.match(/Android/i); },
    BlackBerry: function () { return navigator.userAgent.match(/BlackBerry/i); },
    iOS: function () { return navigator.userAgent.match(/iPhone|iPad|iPod/i); },
    Opera: function () { return navigator.userAgent.match(/Opera Mini/i); },
    Windows: function () { return navigator.userAgent.match(/IEMobile/i); },
    any: function () { return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows()); }
};



$(document).ready(function () {
    
    // Toggle Search
    $('.show-search').click(function(){
        $('.search-form').css('margin-top', '0');
        $('.search-input').focus();
    });
    
    $('.close-search').click(function(){
        $('.search-form').css('margin-top', '-70px');
    });
    
    // Fullscreen
    function toggleFullScreen() {
        if ((document.fullScreenElement && document.fullScreenElement !== null) ||  
            (!document.mozFullScreen && !document.webkitIsFullScreen)) {
            if (document.documentElement.requestFullScreen) {  
                document.documentElement.requestFullScreen();  
            } else if (document.documentElement.mozRequestFullScreen) {  
                document.documentElement.mozRequestFullScreen(); 
            } else if (document.documentElement.webkitRequestFullScreen) {
                document.documentElement.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);  
            }  
        } else {  
            if (document.cancelFullScreen) {  
                document.cancelFullScreen();  
            } else if (document.mozCancelFullScreen) {  
                document.mozCancelFullScreen();  
            } else if (document.webkitCancelFullScreen) {  
                document.webkitCancelFullScreen();  
            }  
        }  
    }
    
    // Waves
    Waves.displayEffect();
    
    // tooltips
    $( '[data-toggle~="tooltip"]' ).tooltip({
        container: 'body'
    });
    
    // Switchery
    var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
    
    elems.forEach(function(html) {
        var switchery = new Switchery(html, { color: '#23B7E5' });
    });
    
    // Element Blocking
    function blockUI(item) {    
        $(item).block({
            message: '<img src="/assets/images/reload.gif" width="20px" alt="">',
            css: {
                border: 'none',
                padding: '0px',
                width: '20px',
                height: '20px',
                backgroundColor: 'transparent'
            },
            overlayCSS: {
                backgroundColor: '#fff',
                opacity: 0.9,
                cursor: 'wait'
            }
        });
    }
    
    function unblockUI(item) {
        $(item).unblock();
    }  
    
    // Panel Control
    $('.panel-collapse').click(function(){
        $(this).closest(".panel").children('.panel-body').slideToggle('fast');
    });
    
    $('.panel-reload').click(function() { 
        var el = $(this).closest(".panel").children('.panel-body');
        blockUI(el);
        window.setTimeout(function () {
            unblockUI(el);
        }, 1000);
    
    }); 
    
    $('.panel-remove').click(function(){
        $(this).closest(".panel").hide();
    });
    
    // Push Menu
    $('.push-sidebar').click(function(){
        var hidden = $('.sidebar');
        
        if (hidden.hasClass('visible')){
            hidden.removeClass('visible');
            $('.page-inner').removeClass('sidebar-visible');
        } else {
            hidden.addClass('visible');
            $('.page-inner').addClass('sidebar-visible');
        }
        
            $('.push-sidebar i').toggleClass("icon-arrow-left");
            $('.push-sidebar i').toggleClass("icon-arrow-right");
    });
    
    // sortable
    $(".sortable").sortable({
        connectWith: '.sortable',
        items: '.panel',
        helper: 'original',
        revert: true,
        placeholder: 'panel-placeholder',
        forcePlaceholderSize: true,
        opacity: 0.95,
        cursor: 'move'
    });
    
    // Uniform
    //var checkBox = $("input[type=checkbox]:not(.switchery), input[type=radio]:not(.no-uniform)");
    //if (checkBox.length > 0) {
    //    checkBox.each(function() {
    //        $(this).uniform();
    //    });
    //};
    
    // .toggleAttr() Function
    $.fn.toggleAttr = function(a, b) {
        var c = (b === undefined);
        return this.each(function() {
            if((c && !$(this).is("["+a+"]")) || (!c && b)) $(this).attr(a,a);
            else $(this).removeAttr(a);
        });
    };
    
    // Sidebar Menu
    var parent, ink, d, x, y;
    $('.sidebar .accordion-menu li .sub-menu').slideUp(0);
    $('.sidebar .accordion-menu li.open .sub-menu').slideDown(0);
    $('.small-sidebar .sidebar .accordion-menu li.open .sub-menu').hide(0);
    $('.sidebar .accordion-menu > li.droplink > a').click(function(){
        
        if(!($('body').hasClass('small-sidebar'))&&(!$('body').hasClass('page-horizontal-bar'))&&(!$('body').hasClass('hover-menu'))) {
        
        var menu = $('.sidebar .menu'),
            sidebar = $('.page-sidebar-inner'),
            page = $('.page-content'),
            sub = $(this).next(),
            el = $(this);
        
        menu.find('li').removeClass('open');
        $('.sub-menu').slideUp(200, function() {
            sidebarAndContentHeight();
        });
        sidebarAndContentHeight();
        
        if (!sub.is(':visible')) {
            $(this).parent('li').addClass('open');
            $(this).next('.sub-menu').slideDown(200, function() {
                sidebarAndContentHeight();
            });
        } else {
            sub.slideUp(200, function() {
                sidebarAndContentHeight();
            });
        }
        return false;
        };
        
        if(($('body').hasClass('small-sidebar'))&&($('body').hasClass('page-sidebar-fixed'))) {
            
        var menu = $('.sidebar .menu'),
            sidebar = $('.page-sidebar-inner'),
            page = $('.page-content'),
            sub = $(this).next(),
            el = $(this);
        
        menu.find('li').removeClass('open');
        $('.sub-menu').slideUp(200, function() {
            sidebarAndContentHeight();
        });
        sidebarAndContentHeight();
        
        if (!sub.is(':visible')) {
            $(this).parent('li').addClass('open');
            $(this).next('.sub-menu').slideDown(200, function() {
                sidebarAndContentHeight();
            });
        } else {
            sub.slideUp(200, function() {
                sidebarAndContentHeight();
            });
        }
        return false;
        };
    });
    
    $('.sidebar .accordion-menu .sub-menu li.droplink > a').click(function(){
        
        var menu = $(this).parent().parent(),
            sidebar = $('.page-sidebar-inner'),
            page = $('.page-content'),
            sub = $(this).next(),
            el = $(this);
        
        menu.find('li').removeClass('open');
        sidebarAndContentHeight();
        
        if (!sub.is(':visible')) {
            $(this).parent('li').addClass('open');
            $(this).next('.sub-menu').slideDown(200, function() {
                sidebarAndContentHeight();
            });
        } else {
            sub.slideUp(200, function() {
                sidebarAndContentHeight();
            });
        }
        return false;
    });
    
    // Small Fixed Sidebar
    var onSidebarHover = function() {
        $('.small-sidebar.page-sidebar-fixed:not(.page-horizontal-bar) .page-sidebar').mouseenter(function(){
            $('body:not(.page-horizontal-bar) .navbar').addClass('hovered-sidebar');
            $('.small-sidebar.page-sidebar-fixed:not(.page-horizontal-bar) .navbar .logo-box a span').html($('.navbar .logo-box a span').text() == smTxt ? str : smTxt);
        });
        $('.small-sidebar.page-sidebar-fixed:not(.page-horizontal-bar) .page-sidebar').mouseleave(function(){
            $('body:not(.page-horizontal-bar) .navbar').removeClass('hovered-sidebar');
    $('.small-sidebar.page-sidebar-fixed:not(.page-horizontal-bar) .navbar .logo-box a span').html($('.navbar .logo-box a span').text() == smTxt ? str : smTxt);
        });
    };
    
    onSidebarHover();
    
    // Makes .page-inner height same as .page-sidebar height
    var sidebarAndContentHeight = function () {
        var content = $('.page-inner'),
            sidebar = $('.page-sidebar'),
            body = $('body'),
            height,
            footerHeight = $('.page-footer').outerHeight(),
            pageContentHeight = $('.page-content').height();
        
            content.attr('style', 'min-height:' + sidebar.height() + 'px !important');
        if (body.hasClass('page-sidebar-fixed')) {
            height = sidebar.height() + footerHeight;
        } else {
            height = sidebar.height() + footerHeight;
            if (height  < $(window).height()) {
                height = $(window).height();
            }
        };
        
        if (height >= content.height()) {
            content.attr('style', 'min-height:' + height + 'px !important');
        };
        if(body.hasClass('page-horizontal-bar')){
            content.attr('style', 'min-height:' + $(window).height() + 'px !important' );
        };
        if((!sidebar.length)&&(!$('.navbar').length)) {
            content.attr('style', 'min-height:' + pageContentHeight + 'px !important' );
        }
    };
    
    sidebarAndContentHeight();
    
    window.onresize = sidebarAndContentHeight;
    
    // Slimscroll
    $('.slimscroll').slimscroll({
        allowPageScroll: true
    });
    
    // Layout Settings
    var fixedHeaderCheck = document.querySelector('.fixed-header-check'),
        fixedSidebarCheck = document.querySelector('.fixed-sidebar-check'),
        horizontalBarCheck = document.querySelector('.horizontal-bar-check'),
        toggleSidebarCheck = document.querySelector('.toggle-sidebar-check'),
        boxedLayoutCheck = document.querySelector('.boxed-layout-check'),
        compactMenuCheck = document.querySelector('.compact-menu-check'),
        hoverMenuCheck = document.querySelector('.hover-menu-check'),
        defaultOptions = function() {
            
            if(($('body').hasClass('small-sidebar'))&&(toggleSidebarCheck.checked == 1)){
                toggleSidebarCheck.click();
            }
        
            if(($('body').hasClass('page-header-fixed'))&&(fixedHeaderCheck.checked == 1)){
                fixedHeaderCheck.click();
            }
        
            if(($('body').hasClass('page-sidebar-fixed'))&&(fixedSidebarCheck.checked == 1)){
                fixedSidebarCheck.click();
            }
        
            if(($('body').hasClass('page-horizontal-bar'))&&(horizontalBarCheck.checked == 1)){
                horizontalBarCheck.click();
            }
        
            if(!($('body').hasClass('compact-menu'))&&(compactMenuCheck.checked == 0)){
                compactMenuCheck.click();
            }
        
            if(($('body').hasClass('hover-menu'))&&(hoverMenuCheck.checked == 1)){
                hoverMenuCheck.click();
            }
        
            if(($('.page-content').hasClass('container'))&&(boxedLayoutCheck.checked == 1)){
                boxedLayoutCheck.click();
            }
           
            sidebarAndContentHeight();
        },
        str = $('.navbar .logo-box a span').text(),
        smTxt = (str.slice(0,1)),
        collapseSidebar = function() {
            $('body').toggleClass("small-sidebar");
            $('.navbar .logo-box a span').html($('.navbar .logo-box a span').text() == smTxt ? str : smTxt);
            sidebarAndContentHeight();
            $('.sidebar-toggle i').toggleClass("icon-arrow-left");
            $('.sidebar-toggle i').toggleClass("icon-arrow-right");
            onSidebarHover();
        },
        fixedHeader = function() {
            if(($('body').hasClass('page-horizontal-bar'))&&($('body').hasClass('page-sidebar-fixed'))&&($('body').hasClass('page-header-fixed'))){
                fixedSidebarCheck.click();
                alert("Static header isn't compatible with a fixed horizontal nav mode. I will set a static mode on the horizontal nav.");
            onSidebarHover();
            };
            $('body').toggleClass('page-header-fixed');
            sidebarAndContentHeight();
            onSidebarHover();
        },
        fixedSidebar = function() {
            if(($('body').hasClass('page-horizontal-bar'))&&(!$('body').hasClass('page-sidebar-fixed'))&&(!$('body').hasClass('page-header-fixed'))){
                fixedHeaderCheck.click();
                alert("Fixed horizontal nav isn't compatible with a static header mode. I will set a fixed mode on the header.");
            };
            if(($('body').hasClass('hover-menu'))&&(!$('body').hasClass('page-sidebar-fixed'))){
                hoverMenuCheck.click();
                alert("Fixed sidebar isn't compatible with a hover menu mode. I will set an accordion mode on the menu.");
            };
            $('body').toggleClass('page-sidebar-fixed');
            if ($('body').hasClass('.page-sidebar-fixed')) {
                $('.page-sidebar-inner').slimScroll({
                    destroy:true
                });
            };
            $('.page-sidebar-inner').slimScroll();
            sidebarAndContentHeight();
            onSidebarHover();
        },
        horizontalBar = function() {
            $('.sidebar').toggleClass('horizontal-bar');
            $('.sidebar').toggleClass('page-sidebar');
            $('body').toggleClass('page-horizontal-bar');
            if(($('body').hasClass('page-sidebar-fixed'))&&(!$('body').hasClass('page-header-fixed'))){
                fixedHeaderCheck.click();
                alert("Static header isn't compatible with a fixed horizontal nav mode. I will set a static mode on the horizontal nav.");
            };
            sidebarAndContentHeight();
            onSidebarHover();
        },
        boxedLayout = function() {
            $('.page-content').toggleClass('container');
            $('.search-form').toggleClass('boxed-layout-search');
            $('.search-form .input-group').toggleClass('container');
            sidebarAndContentHeight();
            onSidebarHover();
        },
        compactMenu = function() {
            $('body').toggleClass('compact-menu');
            sidebarAndContentHeight();
            onSidebarHover();
        },
        hoverMenu = function() {
            if((!$('body').hasClass('hover-menu'))&&($('body').hasClass('page-sidebar-fixed'))){
                fixedSidebarCheck.click();
                alert("Fixed sidebar isn't compatible with a hover menu mode. I will set a static mode on the sidebar.");
            onSidebarHover();
            };
            $('body').toggleClass('hover-menu');
            sidebarAndContentHeight();
            onSidebarHover();
        };
    
    // Logo text on Collapsed Sidebar
    $('.small-sidebar .navbar .logo-box a span').html($('.navbar .logo-box a span').text() == smTxt ? str : smTxt);
    
    
    if( !$('.theme-settings').length ) {
        $('.sidebar-toggle').click(function() {
            collapseSidebar();
        });
    };
    
    if( $('.theme-settings').length ) {
    fixedHeaderCheck.onchange = function() {
        fixedHeader();
    };
    
    fixedSidebarCheck.onchange = function() {
        fixedSidebar();
    };
    
    horizontalBarCheck.onchange = function() {
        horizontalBar();
    };
    
    toggleSidebarCheck.onchange = function() {
        collapseSidebar();
    };
        
    compactMenuCheck.onchange = function() {
        compactMenu();
    };
        
    hoverMenuCheck.onchange = function() {
        hoverMenu();
    };
        
    boxedLayoutCheck.onchange = function() {
        boxedLayout();
    };
    
    
    // Sidebar Toggle
    $('.sidebar-toggle').click(function() {
        toggleSidebarCheck.click();
    });
    
    // Reset options
    $('.reset-options').click(function() {
        defaultOptions();
    });
    
    // Fixed Sidebar Bug
    if(!($('body').hasClass('page-sidebar-fixed'))&&(fixedSidebarCheck.checked == 1)){
        $('body').addClass('page-sidebar-fixed');
    }
    
    if(($('body').hasClass('page-sidebar-fixed'))&&(fixedSidebarCheck.checked == 0)){
        $('.fixed-sidebar-check').prop('checked', true);
    }
    
    // Fixed Header Bug
    if(!($('body').hasClass('page-header-fixed'))&&(fixedHeaderCheck.checked == 1)){
        $('body').addClass('page-header-fixed');
    }
    
    if(($('body').hasClass('page-header-fixed'))&&(fixedHeaderCheck.checked == 0)){
        $('.fixed-header-check').prop('checked', true);
    }
    
    // horizontal bar Bug
    if(!($('body').hasClass('page-horizontal-bar'))&&(horizontalBarCheck.checked == 1)){
        $('body').addClass('page-horizontal-bar');
        $('.sidebar').addClass('horizontal-bar');
        $('.sidebar').removeClass('page-sidebar');
    }
    
    if(($('body').hasClass('page-horizontal-bar'))&&(horizontalBarCheck.checked == 0)){
        $('.horizontal-bar-check').prop('checked', true);
    }
    
    // Toggle Sidebar Bug
    if(!($('body').hasClass('small-sidebar'))&&(toggleSidebarCheck.checked == 1)){
        $('body').addClass('small-sidebar');
    }
    
    if(($('body').hasClass('small-sidebar'))&&(toggleSidebarCheck.checked == 0)){
        $('.horizontal-bar-check').prop('checked', true);
    }
    
    // Boxed Layout Bug
    if(!($('.page-content').hasClass('container'))&&(boxedLayoutCheck.checked == 1)){
        $('.toggle-sidebar-check').addClass('container');
    }
    
    if(($('.page-content').hasClass('container'))&&(boxedLayoutCheck.checked == 0)){
        $('.boxed-layout-check').prop('checked', true);
    }
        
    // Boxed Layout Bug
    if(!($('.page-content').hasClass('container'))&&(boxedLayoutCheck.checked == 1)){
        $('.toggle-sidebar-check').addClass('container');
    }
    
    if(($('.page-content').hasClass('container'))&&(boxedLayoutCheck.checked == 0)){
        $('.boxed-layout-check').prop('checked', true);
    }
        
    // Boxed Layout Bug
    if(!($('.page-content').hasClass('container'))&&(boxedLayoutCheck.checked == 1)){
        $('.toggle-sidebar-check').addClass('container');
    }
    
    if(($('.page-content').hasClass('container'))&&(boxedLayoutCheck.checked == 0)){
        $('.boxed-layout-check').prop('checked', true);
    }
    }
    
    // Chat Sidebar
    if($('.chat').length) {
        var menuRight = document.getElementById( 'cbp-spmenu-s1' ),
        showRight = document.getElementById( 'showRight' ),
        closeRight = document.getElementById( 'closeRight' ),
        menuRight2 = document.getElementById( 'cbp-spmenu-s2' ),
        closeRight2 = document.getElementById( 'closeRight2' ),
        body = document.body;
    
    showRight.onclick = function() {
        classie.toggle( menuRight, 'cbp-spmenu-open' );
    };
    
    closeRight.onclick = function() {
        classie.toggle( menuRight, 'cbp-spmenu-open' );
    };
    
    closeRight2.onclick = function() {
        classie.toggle( menuRight2, 'cbp-spmenu-open' );
    };
    
    $('.showRight2').click(function() {
        classie.toggle( menuRight2, 'cbp-spmenu-open' );
    });
    
    $(".chat-write form input").keypress(function (e) {
        if ((e.which == 13)&&(!$(this).val().length == 0)) {
            $('<div class="chat-item chat-item-right"><div class="chat-message">' + $(this).val() + '</div></div>').insertAfter(".chat .chat-item:last-child");
            $(this).val('');
        } else if(e.which == 13) {
            return;
        }
        $('.chat').slimscroll({
            allowPageScroll: true
        });
    });
        }
});