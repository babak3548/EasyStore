"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ExtentionMethods_1 = require("./ExtentionMethods");
var urlAddToCart;
var help = new ExtentionMethods_1.helpTools();
function TSBtnSelectColorDetails(_color) {
    // let name: string = "Fred";
    // debugger;
    // alert(_color);
    document.getElementById("hiddInputColore").setAttribute("value", _color);
}
var Student = /** @class */ (function () {
    function Student(firstName, middleInitial, lastName) {
        this.firstName = firstName;
        this.middleInitial = middleInitial;
        this.lastName = lastName;
        this.fullName = firstName + " " + middleInitial + " " + lastName;
    }
    return Student;
}());
function greeter(person) {
    return "Hello, " + person.firstName + " " + person.lastName;
}
var user = new Student("Fred", "M.", "Smith");
var PaymentViewField = /** @class */ (function () {
    function PaymentViewField() {
    }
    return PaymentViewField;
}());
function TSGetTokenPasargad(formUrl) {
    var pasargad = new PaymentPasargad(formUrl);
    pasargad.PostData();
}
function setBIPC(bipId) {
    var cn = $("#cn" + bipId).val();
    $("#hd" + bipId).val(bipId + "," + cn);
}
var PaymentPasargad = /** @class */ (function () {
    function PaymentPasargad(_formUrl) {
        this.formUrl = _formUrl;
        this.paymentViewField = new PaymentViewField();
    }
    PaymentPasargad.prototype.inCallbackGetToken = function (result) {
        console.log("inCallbackGetToken.result:", result);
        $("#invoiceNumber").val(result.invoiceNumber);
        $("#invoiceDate").val(result.invoiceDate);
        $("#amount").val(result.amount);
        $("#terminalCode").val(result.terminalCode);
        $("#merchantCode").val(result.merchantCode);
        $("#redirectAddress").val(result.redirectAddress);
        $("#timeStamp").val(result.timeStamp);
        $("#action").val(result.action);
        $("#sign").val(result.sign);
        $("#formSubmitToBank").submit();
    };
    PaymentPasargad.prototype.PostData = function () {
        var _this = this;
        this.paymentViewField.fkProvince = parseInt(document.getElementById("fkProvince").getAttribute("value"));
        this.paymentViewField.address = document.getElementById("address").getAttribute("value");
        this.paymentViewField.noteForBussiness = document.getElementById("noteForBussiness").getAttribute("value");
        this.paymentViewField.invoiceId = parseInt(document.getElementById("invoiceId").getAttribute("value"));
        TSPostObject(this.paymentViewField, this.formUrl, function (result) { return _this.inCallbackGetToken(result); });
    };
    return PaymentPasargad;
}());
function TSPostForm(fromId, formUrl, inCallback) {
    //Serialize the form datas.  
    var valdata = $("#" + fromId).serialize();
    //to get alert popup  
    alert(valdata);
    $.ajax({
        url: formUrl,
        type: "POST",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: valdata,
        success: function (result) { return inCallback(result); }
    });
}
function TSPostObject(valdata, formUrl, inCallback) {
    //to get alert popup  
    alert(valdata);
    $.ajax({
        url: formUrl,
        type: "POST",
        dataType: 'json',
        //  contentType: "application/json;charset=utf-8",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: valdata,
        success: function (result) { return inCallback(result); }
        //    function (result) {
        //    console.log("inCallbackGetToken.result1:", result);
        //}
    });
}
var PromotionProductModel = /** @class */ (function () {
    function PromotionProductModel() {
    }
    return PromotionProductModel;
}());
var arrPromotionProductModels;
function setArrPromotionProductModels(arrPro) {
    console.log("arrPro:", arrPro);
    arrPromotionProductModels = arrPro;
    console.log("arrPromotionProductModels:", arrPromotionProductModels);
}
function TSrenderHtmlModal(productId) {
    var PromSelected;
    var colorHtml;
    var firstColor;
    var BeforDiscountPriceHtml;
    firstColor = "";
    for (var _i = 0, arrPromotionProductModels_1 = arrPromotionProductModels; _i < arrPromotionProductModels_1.length; _i++) {
        var item = arrPromotionProductModels_1[_i];
        if (item.FkProduct === productId) {
            PromSelected = item;
            //  return; to do no nedd to continue
        }
    }
    if (PromSelected.AvailableColors !== null && PromSelected.AvailableColors !== "" && PromSelected.AvailableColors !== undefined) {
        firstColor = PromSelected.AvailableColors.split(",")[0];
        for (var _a = 0, _b = PromSelected.AvailableColors.split(","); _a < _b.length; _a++) {
            var _color = _b[_a];
            colorHtml = colorHtml + ("<li id=\"" + _color + "\" style = \"background:" + _color + "\" > <a onclick=\"TSBtnSelectColorDetails('" + _color + "')\" > </a></li >");
        }
    }
    if (PromSelected.BeforDiscountPrice > 0) {
        BeforDiscountPriceHtml = "<span class=\"old_price\">" + PromSelected.BeforDiscountPrice + " \u062A\u0648\u0645\u0627\u0646</span>";
    }
    var modal_boxIdStr = "modal_box" + productId;
    var modalStrHtml = " <div class=\"modal fade\" id=\"" + modal_boxIdStr + "\" tabindex=\"-1\" role=\"dialog\" aria-hidden=\"true\">\n        <div class=\"modal-dialog modal-dialog-centered\" role=\"document\">\n            <div class=\"modal-content\">\n                <button type=\"button\" class=\"close\" data-dismiss=\"modal\" aria-label=\"Close\">\n                    <span aria-hidden=\"true\">\u00D7</span>\n                </button>\n                <div class=\"modal_body\">\n                    <div class=\"container\">\n                        <div class=\"row\">\n                            <div class=\"col-lg-5 col-md-5 col-sm-12\">\n                                <div class=\"modal_tab\">\n                                    <div class=\"tab-content product-details-large\">\n                                        <div class=\"tab-pane fade show active\" id=\"tab1\" role=\"tabpanel\">\n                                            <div class=\"modal_tab_img\">\n                                                <a href=\"#\"><img src=\"" + help.GetUrlDetailsImg(PromSelected.ProductImage) + "\" alt=\"\"></a>\n                                            </div>\n                                        </div>\n                                        <div class=\"tab-pane fade\" id=\"tab2\" role=\"tabpanel\">\n                                            <div class=\"modal_tab_img\">\n                                                <a href=\"#\"><img src=\"" + help.GetUrlDetailsImg(PromSelected.ProductImage1) + "\" alt=\"\"></a>\n                                            </div>\n                                        </div>\n                                        <div class=\"tab-pane fade\" id=\"tab3\" role=\"tabpanel\">\n                                            <div class=\"modal_tab_img\">\n                                                <a href=\"#\"><img src=\"" + help.GetUrlDetailsImg(PromSelected.ProductImage2) + "\" alt=\"\"></a>\n                                            </div>\n                                        </div>\n                                        <div class=\"tab-pane fade\" id=\"tab4\" role=\"tabpanel\">\n                                            <div class=\"modal_tab_img\">\n                                                <a href=\"#\"><img src=\"" + help.GetUrlDetailsImg(PromSelected.ProductImage3) + "\" alt=\"\"></a>\n                                            </div>\n                                        </div>\n                                    </div>\n                                    <div class=\"modal_tab_button\">\n                                        <ul class=\"nav product_navactive owl-carousel\" role=\"tablist\">\n                                            <li>\n                                                <a class=\"nav-link active\" data-toggle=\"tab\" href=\"#tab1\" role=\"tab\" aria-controls=\"tab1\" aria-selected=\"false\"><img src=\"" + help.GetUrlDetailsImg(PromSelected.ProductImage) + "\" alt=\"\"></a>\n                                            </li>\n                                            <li>\n                                                <a class=\"nav-link\" data-toggle=\"tab\" href=\"#tab2\" role=\"tab\" aria-controls=\"tab2\" aria-selected=\"false\"><img src=\"" + help.GetUrlDetailsImg(PromSelected.ProductImage1) + "\" alt=\"\"></a>\n                                            </li>\n                                            <li>\n                                                <a class=\"nav-link button_three\" data-toggle=\"tab\" href=\"#tab3\" role=\"tab\" aria-controls=\"tab3\" aria-selected=\"false\"><img src=\"" + help.GetUrlDetailsImg(PromSelected.ProductImage2) + "\" alt=\"\"></a>\n                                            </li>\n                                            <li>\n                                                <a class=\"nav-link\" data-toggle=\"tab\" href=\"#tab4\" role=\"tab\" aria-controls=\"tab4\" aria-selected=\"false\"><img src=\"" + help.GetUrlDetailsImg(PromSelected.ProductImage3) + "\" alt=\"\"></a>\n                                            </li>\n\n                                        </ul>\n                                    </div>\n                                </div>\n                            </div>\n                            <div class=\"col-lg-7 col-md-7 col-sm-12\">\n                                <div class=\"modal_right\">\n                                    <div class=\"modal_title mb-10\">\n                                        <h2>" + PromSelected.ProductName + "</h2>\n                                    </div>\n                                    <div class=\"modal_price mb-10\">\n                                        <span class=\"new_price\">" + PromSelected.Price + " \u062A\u0648\u0645\u0627\u0646</span>\n                                       " + BeforDiscountPriceHtml + "\n                                    </div>\n                                    <div class=\"modal_description mb-15\">\n                                        <p>" + PromSelected.Discription + " </p>\n                                    </div>\n                                    <div class=\"variants_selects\">\n\n                                        <div class=\"variants_color\">\n                                            <h2>\u0631\u0646\u06AF</h2>\n                                             <ul>\n                                                   " + colorHtml + "\n                                             </ul>\n                                        </div>\n                                        <div class=\"modal_add_to_cart\">\n                                        <form action=\"" + urlAddToCart + "\" method=\"post\">\n                                            <input name=\"productCount\" min=\"1\" max=\"" + PromSelected.Available + "\" value=\"1\" type=\"number\">\n                                            <input name=\"productColor\" id=\"hiddInputColore\" type=\"hidden\" value=\"" + firstColor + "\" />\n                                            <input name=\"ProductId\" type=\"hidden\" value=\"" + PromSelected.FkProduct + "\" />\n                                            <button type=\"submit\">\u0627\u0641\u0632\u0648\u062F\u0646 \u0628\u0647 \u0633\u0628\u062F</button>\n                                        </form>\n                                        </div>\n                                    </div>\n                                    <div class=\"modal_social\">\n                                        <h2>\u0627\u0634\u062A\u0631\u0627\u06A9 \u06AF\u0630\u0627\u0631\u06CC \u0627\u06CC\u0646 \u0645\u062D\u0635\u0648\u0644</h2>\n                                        <ul>\n                                            <li class=\"facebook\"><a href=\"#\"><i class=\"fa fa-facebook\"></i></a></li>\n                                            <li class=\"twitter\"><a href=\"#\"><i class=\"fa fa-twitter\"></i></a></li>\n                                            <li class=\"pinterest\"><a href=\"#\"><i class=\"fa fa-pinterest\"></i></a></li>\n                                            <li class=\"google-plus\"><a href=\"#\"><i class=\"fa fa-google-plus\"></i></a></li>\n                                            <li class=\"linkedin\"><a href=\"#\"><i class=\"fa fa-linkedin\"></i></a></li>\n                                        </ul>\n                                    </div>\n                                </div>\n                            </div>\n                        </div>\n                    </div>\n                </div>\n            </div>\n        </div>\n    </div>";
    modalStrHtml = modalStrHtml.replace("modal_boxIdStr", modal_boxIdStr);
    //console.info(modalStrHtml);
    if ($('#' + modal_boxIdStr).length <= 0) {
        $('#modalAreaId').append(modalStrHtml);
    }
    // return xxx2;
}
//# sourceMappingURL=app.js.map