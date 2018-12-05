$(document).ready(function () {
    calculateHeight();

    $('#main-menu-toggle').click(function () {
        var hidden = $('#main-menu');
        if (hidden.hasClass('open')) {
            $('#main-menu').removeClass('open');
            $('#menubackdrop').remove();
        } else {
            $('#main-menu').addClass('open');
            $('body').append('<div id="menubackdrop"></div>');
        }
    });

    $(document).on('click', '#menubackdrop', function () {
        $('#main-menu').removeClass('open');
        $('#menubackdrop').remove();
    });

    //**********************************BEGIN MAIN MENU********************************
    jQuery('.page-sidebar li > a').on('click', function (e) {
        if ($(this).next().hasClass('sub-menu') === false) {
            return;
        }
        var parent = $(this).parent().parent();


        parent.children('li.open').children('a').children('.arrow').removeClass('open');
        parent.children('li.open').children('a').children('.arrow').removeClass('active');
        parent.children('li.open').children('.sub-menu').slideUp(200);
        parent.children('li').removeClass('open');
        //  parent.children('li').removeClass('active');

        var sub = jQuery(this).next();
        if (sub.is(":visible")) {
            jQuery('.arrow', jQuery(this)).removeClass("open");
            jQuery(this).parent().removeClass("active");
            sub.slideUp(200, function () {
                // handleSidenarAndContentHeight();
            });
        } else {
            jQuery('.arrow', jQuery(this)).addClass("open");
            jQuery(this).parent().addClass("open");
            sub.slideDown(200, function () {
                //handleSidenarAndContentHeight();
            });
        }

        e.preventDefault();
    });
    //Auto close open menus in Condensed menu
    if ($('.page-sidebar').hasClass('mini')) {
        var elem = jQuery('.page-sidebar ul');
        elem.children('li.open').children('a').children('.arrow').removeClass('open');
        elem.children('li.open').children('a').children('.arrow').removeClass('active');
        elem.children('li.open').children('.sub-menu').slideUp(200);
        elem.children('li').removeClass('open');
    }
    //**********************************END MAIN MENU********************************

    $('.scrollbar-inner').scrollbar();



    if ($.cookie('setcookieforpage') == "yes") {
        $('#main-menu').addClass('mini');
        $('.page-content').addClass('condensed');
    }


    $('#layout-condensed-toggle').click(function () {
        var winwidth = $(window).width();
        if ($('#main-menu').attr('data-inner-menu') == '1') {
            //console.log("Menu is already condensed");
        } else {
            if ($('#main-menu').hasClass('mini')) {
                $.cookie('setcookieforpage', 'no', { expires: 7, path: '/' });

                $('body').removeClass('condense-menu');
                $('#main-menu').removeClass('mini');
                $('.page-content').removeClass('condensed');
                $('.header-seperation').show();
                $(".menuselected").parent().parent().css("display", "block");
                $(".menuselected").parent().parent().parent().addClass("open");
                $(".menuselected").parent().parent().parent().parent().css("display", "block");
                $(".menuselected").parent().parent().parent().parent().parent().addClass("open");
            } else {
                if (winwidth > 768) {
                    $.cookie('setcookieforpage', 'yes', { expires: 7, path: '/' });
                }
                $('#main-menu').addClass('mini');
                $('.page-content').addClass('condensed');

                $('.page-sidebar li.open > a').trigger('click');
            }
        }
    });

    $(window).resize(function () {
        calculateHeight();
        var winwidth = $(window).width();
        if (winwidth <= 768) {
            if ($("#main-menu").hasClass("mini")) {
                $("#main-menu").removeClass("mini");
                $.cookie('setcookieforpage', 'no', { expires: 7, path: '/' });
                $('.page-content').removeClass('condensed');
            }
        }
    });

    var pgurl = window.location.href.substr(window.location.href.lastIndexOf("/") + 1);

  
    //$('[data-toggle="tooltip"]').tooltip();
  

    $("#menu li a").each(function () {

        if ($(this).attr("href") == pgurl || $(this).attr("href") == '') {
            $(this).addClass("menuselected");
        }
        if ($(".menuselected").parent().parent().hasClass("sub-menu")) {
            if ($("#main-menu").hasClass("mini")) {
                $(".menuselected").parent().parent().parent().children().first().find('i').addClass('hghtyellow');
            } else {
                $(".menuselected").parent().parent().css("display", "block");
                $(".menuselected").parent().parent().parent().addClass("open");
                $(".menuselected").parent().parent().parent().children().first().find('i').addClass('hghtyellow');
            }
        }
        if ($(".menuselected").parent().parent().parent().parent().hasClass("sub-menu")) {
            if ($("#main-menu").hasClass("mini")) {
                $(".menuselected").parent().parent().css("display", "block");
                $(".menuselected").parent().parent().parent().addClass("open");
                $(".menuselected").parent().parent().parent().parent().parent().children().first().find('i').addClass('hghtyellow');
            } else {
                $(".menuselected").parent().parent().parent().parent().css("display", "block");
                $(".menuselected").parent().parent().parent().parent().parent().addClass("open");
                $(".menuselected").parent().parent().parent().parent().parent().children().first().find('i').addClass('hghtyellow');
            }
        }
    });

    if ($("#menu li a").hasClass("menuselected")) {
        var targetOffset = $(".menuselected").parent().offset().top - 61;
        $("#main-menu-wrapper").animate({ scrollTop: targetOffset }, 0);
    }


});


function calculateHeight() {
    var winwidtha = $(window).width();
    if (winwidtha >= 768) {
        $('.page-content').css({ 'min-height': ($(window).height()) + 'px' });
    } else {
        $('.page-content').css({ 'min-height': '100%' });
    }
}

