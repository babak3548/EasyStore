//<reference path="jquery-1.6.2-vsdoc.js" />
var validateForm = true;
var AjaxRequestError = true;
var AjaxRequestErrorBuffer = Array();

function ValidateForm(Please_fill_correctly_above_fields,e) {

    validateForm = true;
    AjaxRequestError = true;
    $('.dig').each(function () {
        var valDig = $(this).val();
        valDig= valDig.replace(/\,/g, '');
        $(this).val(valDig);
    });

    ValiFrmAjaxReqFie();

    //   debugger;
    $('input').trigger('blur');
    $('select').trigger('blur');
    $('textarea').trigger('blur');
    if (validateForm && AjaxRequestError) { $('#lblErrorSubmitForm').remove(); $('#EditorRowForm').submit(); }
    else { $('#lblErrorSubmitForm').remove(); e.preventDefault(); $('#btnFormSubmit').after('<label id="lblErrorSubmitForm" style="color:Red;width:auto;"> ' + Please_fill_correctly_above_fields + ' </label>'); }
}

function ValFrmWithOutSele(Please_fill_correctly_above_fields) {

    validateForm = true;
    var AjaxRequestError = true;
    ValiFrmAjaxReqFie();
    $('input').trigger('blur');
    $('textarea').trigger('blur');
    if (validateForm && AjaxRequestError) { $('#lblErrorSubmitForm').remove(); $('#EditorRowForm').submit(); }
    else { $('#lblErrorSubmitForm').remove(); $('#btnFormSubmit').after('<label id="lblErrorSubmitForm" style="color:Red;width:auto;"> ' + Please_fill_correctly_above_fields + ' </label>'); }
}
function ValiFrmAjaxReqFie() {
    for (var field in AjaxRequestErrorBuffer) {
        if (AjaxRequestErrorBuffer[field] != "") {
            AjaxRequestError = false;
        }
    }
}

function AddToAjaxReqErr(ElementId) {
    for (var field in AjaxRequestErrorBuffer) {
        if (AjaxRequestErrorBuffer[field] == ElementId) {
            return;
        }
    }
    AjaxRequestErrorBuffer.push(ElementId);
}

function RemoveFromAjaxReqErr(ElementId) {
    for (var field in AjaxRequestErrorBuffer) {
        if (AjaxRequestErrorBuffer[field] == ElementId) {
            AjaxRequestErrorBuffer[field] = "";

        }
    }

}
function StringValidation(ElementId, MinLength, MaxLength, this_fieled_cannot_null, min_value_this_field, max_value_this_field) {

    var value = $('#EditorRowForm #' + ElementId).val();
    if (value.length == 0 && MinLength != 0) { errorMessage(ElementId, this_fieled_cannot_null); }
    else if (value.length < MinLength) { errorMessage(ElementId, min_value_this_field + MinLength); }
    else if (value.length > MaxLength) { errorMessage(ElementId, max_value_this_field + MaxLength); }
    else { $('#EditorRowForm #lblER' + ElementId).remove(); }

}
function ValidationCheckBox(ElementId, this_fieled_cannot_null) {
    var value = $('#EditorRowForm #' + ElementId).get(0).checked;
    if (!value) { errorMessage(ElementId, this_fieled_cannot_null); }
    else { $('#EditorRowForm #lblER' + ElementId).remove(); }
}
function ValidationCheckBoxs(ElementId, ElementId1, select_one_state) {
    var value = $('#EditorRowForm #' + ElementId).get(0).checked;
    var value1 = $('#EditorRowForm #' + ElementId1).get(0).checked;
    if ((value && value1) || (!value && !value1)) { errorMessage(ElementId1, select_one_state); }
    else { $('#EditorRowForm #lblER' + ElementId1).remove(); }
}
function drdValidation(ElementId, this_fieled_cannot_null) {
    var value = $('#EditorRowForm #' + ElementId).find('option:selected').val();

    if (value == "") { errorMessage(ElementId, this_fieled_cannot_null); }
    else { $('#EditorRowForm #lblER' + ElementId).remove(); }
}
function errorMessage(ElementId, message) {
    $('#EditorRowForm #lblER' + ElementId).remove();
    $('#EditorRowForm #' + ElementId).after('<label id=lblER' + ElementId + ' style="color:Red;width:auto;">' + message + '</label>'); validateForm = false;
}


function Message(ElementId, message) {
    $('#EditorRowForm #lblMe' + ElementId).remove();
    $('#EditorRowForm #' + ElementId).after('<label id=lblMe' + ElementId + ' style="color:Green;width:auto;">' + message + '</label>');
}

function ValidationNumber(ElementId, this_field_must_be_number) {
    var value = $('#EditorRowForm #' + ElementId).val();
    value = value.replace(/\,/g, '');
    if (isNaN(value)) { errorMessage(ElementId, this_field_must_be_number); }
    else { $('#EditorRowForm #lblER' + ElementId).remove(); }
}

function ValidationNumberWithMinMax(ElementId, MinValue, MaxValue, this_field_must_be_number, min_value_this_field, max_value_this_field) {
    var value = $('#EditorRowForm #' + ElementId).val();
    value = value.replace(/\,/g, '');
    if (isNaN(value)) { errorMessage(ElementId, this_field_must_be_number); }
    else if (value < MinValue) { errorMessage(ElementId, min_value_this_field + MinValue); }
    else if (value > MaxValue) { errorMessage(ElementId, max_value_this_field + MaxValue); }
    else { $('#EditorRowForm #lblER' + ElementId).remove(); }
}

function ValidationNumberLength(ElementId, MinLength, MaxLength, this_fieled_cannot_null, min_value_this_field, max_value_this_field, this_field_must_be_number) {
    var value = $('#EditorRowForm #' + ElementId).val();
    value = value.replace(/\,/g, '');
    var ErrState = false;
    var ErrMess = "";
    if (value.length == 0 && MinLength != 0) { ErrState = true; ErrMess = ErrMess + this_fieled_cannot_null }
    else if (value.length < MinLength) { ErrState = true; ErrMess = ErrMess + min_value_this_field + MinLength; }
    else if (value.length > MaxLength) { ErrState = true; ErrMess = ErrMess + max_value_this_field + MaxLength; }

    if (isNaN(value)) { ErrState = true; ErrMess = ErrMess + this_field_must_be_number; }

    if (ErrState) { errorMessage(ElementId, ErrMess); }
    else { $('#EditorRowForm #lblER' + ElementId).remove(); }
}

function ValidationRepeatField(ElementId, reElementId, Repeat_field_not_correct) {
    var value = $('#EditorRowForm #' + ElementId).val();
    var revalue = $('#EditorRowForm #' + reElementId).val();

    if (value != revalue) { errorMessage(reElementId, Repeat_field_not_correct); }
    else { $('#EditorRowForm #lblER' + reElementId).remove(); }
}

function GenDig(ElementId) {
    var value = $('#'+ElementId.id).val();
    var resVal = GenDigVal(value);
   
    $('#' + ElementId.id).val(resVal);

}
function GenDigVal(value) {
    value = value.replace(/\,/g, '');
    var i = value.length;
    var resVal = "";
    while (i > 3) {
        resVal = "," + value.substring(i - 3, i) + resVal;
        i -= 3;
    }
    resVal = value.substring(0, i) + resVal
    return resVal;
}
function emailValidate(ElementId, this_fieled_cannot_null, Enter_a_valid_email_address) {
    //   debugger;
    var emailReg = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;    ///^([\w-\.]+@@([\w-]+\.)+[\w-]{2,4})?$/;
    //   /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/; 
    var value = $('#EditorRowForm #' + ElementId).val();
    if (value.length == 0) { errorMessage(ElementId, this_fieled_cannot_null); }
    else if (!emailReg.test(value)) { errorMessage(ElementId, Enter_a_valid_email_address); }
    else { $('#EditorRowForm #lblER' + ElementId).remove(); }
}

function checkMelliCode(meli_code) {
    if (meli_code.length == 10) {
        if (meli_code == '1111111111' ||
        meli_code == '0000000000' ||
        meli_code == '2222222222' ||
        meli_code == '3333333333' ||
        meli_code == '4444444444' ||
        meli_code == '5555555555' ||
        meli_code == '6666666666' ||
        meli_code == '7777777777' ||
        meli_code == '8888888888' ||
        meli_code == '9999999999') {
            return false;
        }
        c = parseInt(meli_code.charAt(9));
        n = parseInt(meli_code.charAt(0)) * 10 +
        parseInt(meli_code.charAt(1)) * 9 +
        parseInt(meli_code.charAt(2)) * 8 +
        parseInt(meli_code.charAt(3)) * 7 +
        parseInt(meli_code.charAt(4)) * 6 +
        parseInt(meli_code.charAt(5)) * 5 +
        parseInt(meli_code.charAt(6)) * 4 +
        parseInt(meli_code.charAt(7)) * 3 +
        parseInt(meli_code.charAt(8)) * 2;
        r = n - parseInt(n / 11) * 11;
        if ((r == 0 && r == c) || (r == 1 && c == 1) || (r > 1 && c == 11 - r)) {
            return true;
        } else {
            return false;
        }
    } else {
        return false;
    }
}

/////////////////////////////////////////////////Upload /////////////
function fileUpload(form, action_url, div_id, InputPropName) {
    debugger;
    // Create the iframe...
    var iframe = document.createElement("iframe");
    iframe.setAttribute("id", "upload_iframe");
    iframe.setAttribute("name", "upload_iframe");
    iframe.setAttribute("width", "0");
    iframe.setAttribute("height", "0");
    iframe.setAttribute("border", "0");
    iframe.setAttribute("style", "width: 0; height: 0; border: none;");

    // Add to document...
    form.parentNode.appendChild(iframe);
    window.frames['upload_iframe'].name = "upload_iframe";

    iframeId = document.getElementById("upload_iframe");

    // Add event...
    var eventHandler = function () {
        debugger;
        if (iframeId.detachEvent) iframeId.detachEvent("onload", eventHandler);
        else iframeId.removeEventListener("load", eventHandler, false);

        // Message from server...
        if (iframeId.contentDocument) {
            content = iframeId.contentDocument.body.textContent;
        } else if (iframeId.contentWindow) {
            content = iframeId.contentWindow.document.body.innerHTML;
        } else if (iframeId.document) {
            content = iframeId.document.body.innerHTML;
        }

        $('#EditorRowForm #imgloadwait').remove();
        //document.getElementById(div_id).value = content;
        $('#EditorRowForm #' + div_id).val(content);
        $('#EditorRowForm #' + InputPropName).val("");

        // Del the iframe...
        setTimeout('iframeId.parentNode.removeChild(iframeId)', 250);
    }

    if (iframeId.addEventListener) iframeId.addEventListener("load", eventHandler, true);
    if (iframeId.attachEvent) iframeId.attachEvent("onload", eventHandler);
    var oldAction = $(form).attr("action");
    //  $('#your_form').attr('action'); گرفتن مقدار یک صفت
    // Set properties of form...
    form.setAttribute("target", "upload_iframe");
    form.setAttribute("action", action_url);
    form.setAttribute("method", "post");
    form.setAttribute("enctype", "multipart/form-data");
    form.setAttribute("encoding", "multipart/form-data");

    // Submit the form...
    form.submit();
    //document.getElementById(div_id).innerHTML = '<img width="66" height="66" src="/Images/ajaxLoader.gif" />';
    $('#EditorRowForm #' + div_id).val("لطفامنتظر بمانید درحال بارگذاری فایل...");
    $('#EditorRowForm #' + div_id).after('<img id="imgloadwait" width="40" height="40" src="/Images/ajaxLoader.gif" />');
    $(form).attr("action", oldAction);
    $(form).attr("target", null);

}
////////////////////////////////////////////////////////////////////


function ValidationNationalCode(ElementId, NationalCode_Is_Wrong) {
    var value = $('#EditorRowForm #' + ElementId).val();

    if (!checkMelliCode(value)) { errorMessage(ElementId, NationalCode_Is_Wrong); }
    else { $('#EditorRowForm #lblER' + ElementId).remove(); }
}

/////////////////////////////////////////// ajax validation
function AjaxReqestvalidate(ElementId, urlValue, NoFindACase) {
    //validateAjaxRequest = true;
    var value = $('#EditorRowForm #' + ElementId).val();
    if (value != "") {

        if (!isNaN(value)) {
            $.ajax({
                url: urlValue,
                data: { 'id': value },
                type: "post",
                cache: false,
                success: function (result) {
                    $('#EditorRowForm #lblER' + ElementId).remove();
                    if (result == "NoFindACase" || result == "Error") {
                        $('#EditorRowForm #' + ElementId).val('not Found Or Error');
                        $('#EditorRowForm #' + ElementId).after('<label id=lblER' + ElementId + ' style="color:Red;width:auto;">' + NoFindACase + '</label>');
                        //debugger;
                        AddToAjaxReqErr(ElementId);

                        $('#EditorRowForm #' + ElementId).attr("style", "color:Red");
                    }
                    else {
                        $('#EditorRowForm #' + ElementId).val(value);

                        $('#EditorRowForm #' + ElementId).after('<label id=lblER' + ElementId + ' style="color:Green;width:auto;">' + result + '</label>');
                        $('#EditorRowForm #' + ElementId).attr("style", "color:Green");
                        RemoveFromAjaxReqErr(ElementId);


                    }

                },
                error: function (xhr, ajaxOptions, thrownError) {
                    errorMessage(ElementId, faild_Inquiry);

                }
            });
        }
    }
    else {

        $('#EditorRowForm #lblER' + ElementId).remove();
        RemoveFromAjaxReqErr(ElementId);
    }

}

function AjaxValidateStr(ElementId, urlValue, ErrMsg) {

    var value = $('#EditorRowForm #' + ElementId).val();
    if (value != "") {
        $.ajax({
            url: urlValue,
            data: { 'id': value },
            type: "post",
            cache: false,
            success: function (result) {
                $('#EditorRowForm #lblER' + ElementId).remove();
                if (result == "Error") {
                    $('#EditorRowForm #' + ElementId).val(' Error');
                    $('#EditorRowForm #' + ElementId).after('<label id=lblER' + ElementId + ' style="color:Red;width:auto;">' + ErrMsg + '</label>');
                    //debugger;
                    AddToAjaxReqErr(ElementId);

                    $('#EditorRowForm #' + ElementId).attr("style", "color:Red");
                }
                else {
                    //$('#EditorRowForm #' + ElementId).val(value.replace(" ",""));

                    $('#EditorRowForm #' + ElementId).after('<label id=lblER' + ElementId + ' style="color:Green;width:auto;">' + result + '</label>');
                    $('#EditorRowForm #' + ElementId).attr("style", "color:Green");
                    RemoveFromAjaxReqErr(ElementId);
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                errorMessage(ElementId, faild_Inquiry);

            }
        });

    }
    else {

        $('#EditorRowForm #lblER' + ElementId).remove();
    }

}

function AjaxValidateEmailStr(ElementId, urlValue, ErrMsg) {

    var value = $('#EditorRowForm #' + ElementId).val();
    if (value != "") {
        $.ajax({
            url: urlValue,
            data: { 'id': value },
            type: "post",
            cache: false,
            success: function (result) {
                $('#EditorRowForm #lblER' + ElementId).remove();
                if (result == "Error") {
                    $('#EditorRowForm #' + ElementId).val(' Error');
                    $('#EditorRowForm #' + ElementId).after('<label id=lblER' + ElementId + ' style="color:Red;width:auto;">' + ErrMsg + '</label>');
                    //debugger;
                    AddToAjaxReqErr(ElementId);

                    $('#EditorRowForm #' + ElementId).attr("style", "color:Red");
                }
                else {
                    $('#EditorRowForm #' + ElementId).val(value.replace(" ", ""));

                    $('#EditorRowForm #' + ElementId).after('<label id=lblER' + ElementId + ' style="color:Green;width:auto;">' + result + '</label>');
                    $('#EditorRowForm #' + ElementId).attr("style", "color:Green");
                    RemoveFromAjaxReqErr(ElementId);
                }

            },
            error: function (xhr, ajaxOptions, thrownError) {
                errorMessage(ElementId, faild_Inquiry);

            }
        });

    }
    else {

        $('#EditorRowForm #lblER' + ElementId).remove();
    }

}
/////////////////////////////////////////// ajax validation                  type: "POST",  dataType: "json",
function AjaxDropdownAtouPostback(ElementId, urlValue, value2, ElementIdResult, WaitMessage) {
    var value = $('#EditorRowForm #' + ElementId).find('option:selected').val();
    if (value != "") {

        if (!isNaN(value)) {
            AddToAjaxReqErr(ElementId);
            Message(ElementId, WaitMessage);

            $.ajax({
                url: urlValue,
                data: { 'id': value, 'value2': value2 },
                type: "post",
                cache: false,
                success: function (result) {
                    $('#EditorRowForm #' + ElementIdResult).val(result);
                    // $('#EditorRowForm #' + ElementIdResult).attr("style", "color:Green");
                    RemoveFromAjaxReqErr(ElementId);
                    $('#EditorRowForm #lblMe' + ElementId).remove();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    errorMessage(ElementId, faild_Inquiry);

                }
            });

        }
    }

}

function AjaxDrdAtuPostRutHtm(ElementId, urlValue, value2, ElementIdResult, WaitMessage) {
    var value = $('#EditorRowForm #' + ElementId).find('option:selected').val();
    if (value != "") {

        if (!isNaN(value)) {
            AddToAjaxReqErr(ElementId);
            Message(ElementId, WaitMessage);

            $.ajax({
                url: urlValue,
                data: { 'id': value, 'value2': value2 },
                type: "post",

                cache: false,
                success: function (result) {
                    $('#EditorRowForm #' + ElementIdResult).html(result);
                    // $('#EditorRowForm #' + ElementIdResult).attr("style", "color:Green");
                    RemoveFromAjaxReqErr(ElementId);
                    $('#EditorRowForm #lblMe' + ElementId).remove();
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    errorMessage(ElementId, faild_Inquiry);

                }
            });

        }
    }

}

//autocomplete textbox  کامل نشده
function AjaxAutocomplete(ElementId, urlValue) {
    var value = $('#' + ElementId).val();
    var xx = "";
    if (value != "") {
        $.ajax({
            url: urlValue,
            data: { 'id': value },
            type: "post",
            cache: false,
            success: function (result) {
                xx = result;
                $("#" + ElementId).autocomplete({
                    source: result
                });
            },
            error: function (xhr, ajaxOptions, thrownError) {
            }
        });
        //$("#" + ElementId).autocomplete({
        //    source: xx
        //});
    }
    //var availableTags = [
    //  "ActionScript",
    //  "AppleScript",
    //  "Asp",
    //  "BASIC",
    //  "C",
    //  "C++",
    //  "Clojure",
    //  "COBOL",
    //  "ColdFusion",
    //  "Erlang",
    //  "Fortran",
    //  "Groovy",
    //  "Haskell",
    //  "Java",
    //  "JavaScript",
    //  "Lisp",
    //  "Perl",
    //  "PHP",
    //  "Python",
    //  "Ruby",
    //  "Scala",
    //  "Scheme"
    //];
    //$("#" + ElementId).autocomplete({
    //    source: availableTags
    //});
}
/*
$(function () {
var availableTags = [
  "ActionScript",
  "AppleScript",
  "Asp",
  "BASIC",
  "C",
  "C++",
  "Clojure",
  "COBOL",
  "ColdFusion",
  "Erlang",
  "Fortran",
  "Groovy",
  "Haskell",
  "Java",
  "JavaScript",
  "Lisp",
  "Perl",
  "PHP",
  "Python",
  "Ruby",
  "Scala",
  "Scheme"
];
$( "#tags" ).autocomplete({
    source: availableTags
});
});
*/

///Sum two value
function SumElement(ElementId1, ElementId2, ElementIdResult) {
    var value1s = $('#EditorRowForm #' + ElementId1).val().replace(/\,/g, '');
    var value1 = parseInt(value1s);
    if (isNaN(value1)) { value1 = 0; }

    var value2s = $('#EditorRowForm #' + ElementId2).val().replace(/\,/g, '');
    var value2 = parseInt(value2s);
    if (isNaN(value2)) { value2 = 0; }
    var sumStr = GenDigVal((value1 + value2).toString());
    $('#EditorRowForm #' + ElementIdResult).val(sumStr);
}

///Sum two value
function SubElement(ElementId1, ElementId2, ElementIdResult) {
    var value1 = parseInt($('#EditorRowForm #' + ElementId1).val().replace(/\,/g, ''));
    if (isNaN(value1)) { value1 = 0; }
    var value2 = parseInt($('#EditorRowForm #' + ElementId2).val().replace(/\,/g, ''));
    if (isNaN(value2)) { value2 = 0; }
    $('#EditorRowForm #' + ElementIdResult).val((value1 - value2));
}

///Sum and sum 
function SubElements(ElementIdBase, ElementIdSum, ElementIdSub, ElementIdResult, ElementIdResult1) {
    var valueBase = parseInt($('#EditorRowForm #' + ElementIdBase).val().replace(/\,/g, ''));
 
    if (isNaN(valueBase)) { valueBase = 0; }
    var valueSum = parseInt($('#EditorRowForm #' + ElementIdSum).val().replace(/\,/g, ''));
    if (isNaN(valueSum)) { valueSum = 0; }
    var valueSub = parseInt($('#EditorRowForm #' + ElementIdSub).val().replace(/\,/g, ''));
    if (isNaN(valueSub)) { valueSub = 0; }
    $('#' + ElementIdResult).val((valueBase + valueSum - valueSub));
    $('#' + ElementIdResult1).val((valueBase + valueSum - valueSub));
    $('#RemainInvoice').val((0));
}



function ConfirimMessage(message) {
    var answer = confirm(message);
    return answer;

}

function print() {
    window.print();
}
function datePiker(ElementId, langugeName, langugeCode) {
    $('#EditorRowForm #' + ElementId).click(function () { $('#EditorRowForm #' + ElementId).focus(); });
    $('#EditorRowForm #' + ElementId).calendarsPicker({
        calendar: $.calendars.instance(langugeName, langugeCode), dateFormat: 'yyyy/mm/dd'
    });
}
function AlessThanB(ElementIdA, ElementIdB, message) {
    var valueA = $('#EditorRowForm #' + ElementIdA).val();
    var valueB = $('#EditorRowForm #' + ElementIdB).val();

    if (valueB <= valueA) { errorMessage(ElementIdB, message); }
    else { $('#EditorRowForm #lblER' + ElementIdB).remove(); }
}
/////////////////////////////////// custom validation  
function ValidationWithIdElement(ElementId, this_fieled_cannot_null, min_value_this_field, max_value_this_field, this_field_must_be_number, NationalCode_Is_Wrong) {

    switch (ElementId) {

        case "AcceptAgreement": ValidationCheckBox(ElementId, this_fieled_cannot_null); break;
        case "Mobile": { ValidationNumberLength(ElementId, 11, 13, this_fieled_cannot_null, min_value_this_field, max_value_this_field, this_field_must_be_number); break; }
        case "NationalCode": { ValidationNationalCode(ElementId, NationalCode_Is_Wrong); break; }
        case "BankAccunt": { ValidationNumberLength(ElementId, 16, 20, this_fieled_cannot_null, min_value_this_field, max_value_this_field, this_field_must_be_number); break; }
        default: StringValidation(ElementId, 1, 50, this_fieled_cannot_null, min_value_this_field, max_value_this_field); break;

    }

}
///////////////////////
function goBack() {
    window.history.back()
}
//////////////////////////image zoomer  ///////////////////////

function ChngSrcVal(ElmIdSrc, ElmIdDes) {
    var valSrc = $('#' + ElmIdSrc).attr("src");
    var valDes = $('#' + ElmIdDes).attr("src");
    $('#' + ElmIdDes).attr("src", valSrc);
    $('#' + ElmIdSrc).attr("src", valDes);
    setSrcVal(ElmIdDes);
}
function setSrcVal(ElmIdSrc) {
    var valSrc = $('#' + ElmIdSrc).attr("src");
    var ur = "url(" + valSrc + ") no-repeat";
    $(".large").css("background", ur);
}

window.onload = function () { $(".dig").trigger('keyup'); };
$(document).ready(function () {
    //image zoomer 
    var native_width = 0;
    var native_height = 0;
    setSrcVal('imgSh');
    $(".magnify").mousemove(function (e) {
        if (!native_width && !native_height) {
            var image_object = new Image();
            image_object.src = $(".small").attr("src");
            native_width = image_object.width;
            native_height = image_object.height;
        }
        else {
            var magnify_offset = $(this).offset();
            var mx = e.pageX - magnify_offset.left;
            var my = e.pageY - magnify_offset.top;
            if (mx < $(this).width() && my < $(this).height() && mx > 0 && my > 0) {
                $(".large").fadeIn(100);
            }
            else {
                $(".large").fadeOut(100);
            }
            if ($(".large").is(":visible")) {
                var rx = Math.round(mx / $(".small").width() * native_width - $(".large").width() / 2) * -1;
                var ry = Math.round(my / $(".small").height() * native_height - $(".large").height() / 2) * -1;
                var bgp = rx + "px " + ry + "px";
                var px = mx - $(".large").width() / 2;
                var py = my - $(".large").height() / 2;
                $(".large").css({ left: px, top: py, backgroundPosition: bgp });
            }
        }
    });
    //image zoomer   change 

    $(".dig").keyup(function (e) {
        GenDig(this);
    });
   //if in intialview express buy exist show bank
    if ($("#ExprsIn2").length > 0 | $("#ExprsIn32").length > 0)
    { $('#trDghId').show(); }
    disChkBox();

    $("#EditorRowForm").submit(function (e) {
        //e.preventDefault();
        //alert('event');
        ValidateForm('لطفا فیلد های بالا را به درستی پر نمایید', e);
        
    });
    //$('#EditorRowForm').keyup(function (e) {
    //    if (e.which == 13) {
    //        ValidateForm('لطفا فیلد های بالا را به درستی پر نمایید');
    //    }
    // })
})
function animateToCart(ElmId, MovetoDes,fk_busProduct) {
    var MovDesPos = $("#" + MovetoDes).offset();
    var ElmPos = $("#" + ElmId).offset();
    var selPr = $("#" + ElmId).clone();
    var Dur = (MovDesPos.left - ElmPos.left);
    var Current_Fk_bus = $('#Current_FK_BusinessOwner').val();
    if (fk_busProduct == Current_Fk_bus) {
        alert('این فروشگاه معتلق به شما می باشد و خرید از آن برای شما امکان پذیر نیست ');
        return;
    }
    selPr.appendTo("body");
    selPr.css("left", ElmPos.left);
    selPr.css("top", ElmPos.top);
    selPr.css("position", "absolute");
    selPr.css({width:200,height:200});
    selPr.animate({
        "left": ElmPos.left += ((MovDesPos.left-ElmPos.left)-100),
        "top": ElmPos.top -= (ElmPos.top - MovDesPos.top+100),
        opacity: '0.5',
        index: '3'
    }, {
        "duration":  2000,
        queue: false,
        complete: function () { selPr.fadeOut(1000); }
    });

}
//data-src=""  <img id='imgLoader' class='CssLoder' src='/Images/ajaxLoader.gif' />
function PopupImgViewer( srcMain, src1, src2,hed) {
    var div = " <a href=\"javascript:closePop('popimg')\" class='clsPop'></a> <h1 class='popHed'>" + hed + "</h1>"
        + "<br/><img id='idS1' onclick=\"fullImgCh('idS1','idM')\" class='fineImg finePop'   src=" + src1 + " />"
        + "<img id='idS2' onclick=\"fullImgCh('idS2','idM')\" class='fineImg finePop'   src=" + src2 + " />"
    + "<br clear='both' /> <img id='idM' class='fullImg'   src=" + srcMain + " />";
    $("#popimg").prepend($(div));
    $("#popimg").fadeIn(0500); // fadein popup div position: absolute; 
    $("#popimg").attr("tabindex", "0");
    //SetRigMargPop();
    $("#popimg").keyup(function (e) {
        if (e.which == 13 | e.which == 27) {
            closePop("popimg");
        }
    });
}
function closePop(elmId) {
    document.getElementById(elmId).innerHTML = "";
    $("#" + elmId).fadeOut();
}

function PopCreteWin(aId, hrfA,title) {
    var addId = "popHlp";
    if ($("#" + addId).length > 0) { $("#" + addId).remove(); }
    $("#" + aId).after("<div  id='" + addId + "'  class='popWin popWinSmal'></div>");
    
    //$("#popimg").prepend($(div));<div id="popimg"  class="popWin">   </div>
    // fadein popup div position: absolute; 
    $("#" + addId).attr("tabindex", "0");
    AjaxGetCntAdToDiv(aId, hrfA, addId, title);
    $("#" + addId).fadeIn(0500);
    //SetRigMargPop();
    $("#" + addId).keyup(function (e) {
        if (e.which == 13 | e.which == 27) {
            closePop(addId);
        }
    });
    $("#mainDiv").mousedown(function (e) {
        if ($("#" + addId).length>0) {
            closePop(addId);
        }
    });
}
function LodingImg(afteId) {
    $("#" + afteId).after('<img id="imgloadwait" class="lodImg" src="/Images/ajax-loader.gif" />');
}
function remLodImg() {
    $("#imgloadwait").remove();
}
function AjaxGetCntAdToDiv(aId,hrfA,addId,title) {
    //  if ($("#" + addId).html() == "") {
    var aCls = "<b>" + title + "</b> <a href=\"javascript:closePop('" + addId + "')\" class='clsPop'></a><br clear='both' /> ";
            $("#" + aId).after('<img id="imgloadwait" style="margin-top: 15px;" width="40" height="40" src="/Images/ajaxLoader.gif" />');
            $.ajax({
                url: hrfA,
                dataType: "html",
                type: "post",
                success: function (result) {
                    $("#imgloadwait").remove();
                    $("#" + addId).prepend($(aCls+result));
                  //  $('#' + addId).toggle();
                  //  $("#" + aId).after('<div id=' + addId + ' style="border:1px solid rgb(162, 162, 162);margin-right: 3px;">' + result + '</div>');
                },
                error: function (xhr, status) {
                    $("#imgloadwait").remove();
                }
            });
        //}
        //else {
        //    $('#' + addId).toggle();
        //}

}

function SetRigMargPop() {
    var widthPop = $("#popimg").css("width");
    var widthImg = window.innerWidth;
    widthPop = widthPop.replace(/[^-\d\.]/g, '');
    var marginRht = ((widthImg - widthPop) / 2);
    $("#popimg").css("margin-right", marginRht);
}
function fullImgCh(ElmIdSrc, ElmIdSho) {
    var valSrc = $('#' + ElmIdSrc).attr("src");
    valSrc = valSrc.substring(0, valSrc.length - 5) + valSrc.substring(valSrc.length - 4, valSrc.length );
    var valSho = $('#' + ElmIdSho).attr("src");
    valSho = valSho.substring(0, valSho.length - 4) +"2"+ valSho.substring(valSho.length - 4, valSho.length);
    if (valSrc.length > 10) { $('#' + ElmIdSho).attr("src", valSrc); }
    if (valSho.length > 10) { $('#' + ElmIdSrc).attr("src", valSho); }
}

function enableOnCheaked() {
    if ($('#notMngInp').is(':checked')) {
        $('.chBox').removeAttr('disabled')
    }
    else {
        $('.chBox').attr('disabled', 'disabled');
    }
}
function ValdatChkBoxTypSell(Minimum_click_one_checkbox) {
    var checked = false;
    $('.chBox').each(function () {
        if ($(this).is(':checked')) { checked = true; }
    });
    if ($('#mangSelSit').is(':checked')) { checked = true; }
    if (!checked) { errorMessage("notMngSpn", Minimum_click_one_checkbox); }
    else { $('#EditorRowForm #lblER' + "notMngSpn").remove(); }
}


function disChkBox() {
    if (!($('#notMngInp').is(':checked'))) {
        $('.chBox').each(function () {
            $(this).attr('disabled', 'disabled');
        });
    }
}
function ShowInChk(ElmId, ShoElmId) {
    if ($('#' + ElmId).is(':checked'))
    { $('#' + ShoElmId).show(); }
    else
    { $('#' + ShoElmId).hide(); }
}
function HideInChk(ElmId, ShoElmId) {
    if ($('#' + ElmId).is(':checked'))
    { $('#' + ShoElmId).hide(); }
    else
    { $('#' + ShoElmId).show(); }
}
function ValdatRedioTypSell(ElmIdErr, clsSctr, click_one_redio) {
    var checked = false;
    $('.' + clsSctr).each(function () {
        if ($(this).is(':checked')) { checked = true; }
    });
    if (!checked) { errorMessage(ElmIdErr, click_one_redio); }
    else { $('#EditorRowForm #lblER' + ElmIdErr).remove(); }
}
function VldtChkBoxACls(ElmIdErr, clsSctr, click_one_redio) {
    var checked = false;
    $('.' + clsSctr).each(function () {
        if ($(this).is(':checked')) { checked = true; }
    });
    
    if (!checked) { errorMessage(ElmIdErr, click_one_redio); }
    else { $('#EditorRowForm #lblER' + ElmIdErr).remove(); }
}