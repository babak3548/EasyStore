//<reference path="jquery-1.6.2-vsdoc.js" />

$(document).ready(function () {
    $('#nav > li > a').mouseover(function () {
        if ($(this).attr('class') != 'active') {
            $('#nav li ul').slideUp(100);
            $(this).next().slideToggle(400);

        }
    });
});
 
$(document).ready(function () {
    $('#nav').mouseleave(function () {
        if ($(this).attr('class') != 'active') {
            $('#nav li ul').slideUp(100);
        }
    });
}); 
/////////////////////////////////////////////
function hoverCate1(thisEle) {
    $('#cate li ul').attr("class", "")
    $(thisEle).next().attr("class", "active");
   
}

function hoverCate(thisEle) {
 
    $('#cate li ul').attr("class", "")
    $('ul:first', thisEle).attr("class", "active");
}

$(document).ready(function () {
    $('#cate').mouseleave(function () {
        $('#cate li ul').attr("class","")
    });
});
///////////////////////////////////////////////////////////////
function hoverTM(thisEle) {
    //document.getElementById()
    var parentEle = thisEle.parentNode;
    $('#'+parentEle.id+' li ul').attr("class", "")
    $('ul:first', thisEle).attr("class", "active");
}

$(document).ready(function () {
    $('#TM, #TMcart').mouseleave(function () {
        $('#TM li ul , #TMcart li ul').attr("class", "")
    });
});
////////////////////////tabs script//////////////
function actItem(li, itm) {
    $('.tabs ul li').each(function () {
        $(this).removeClass("activeTb");
    });
    $('.tabs ul #' + li).addClass("activeTb");
    $('.tabInner .cssItem').each(function () {
        $(this).css("display", "none");
    });
    $('.tabInner #' + itm).css("display", "block");
}