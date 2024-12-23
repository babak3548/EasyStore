//var helpTools = require('./ExtentionMethods.js');
//import {helpTools} from "./ExtentionMethods"; 
var urlAddToCart;
var EMS_AppSetting_ImagePathInVirtual;
var EMS_ImgDetailsSize;
var EMS_ImgListSize;
var EMS_ImgListSizeUI;
var FullDomainUrl = "https://EasyStore.com";
var yektanetObj;
var helpTools = /** @class */ (function () {
    function helpTools() {
    }
    helpTools.prototype.GetUrlDetailsImg = function (fileName) {
        if (fileName === null || fileName === "" || fileName === "undefined") {
            return "/Images/Product/2400d852-abc8-4f68-844c-4d9067d8427b800_800.jpg";
        }
        else {
            return EMS_AppSetting_ImagePathInVirtual.concat("/", fileName, EMS_ImgDetailsSize, "_", EMS_ImgDetailsSize + ".jpg");
        }
    };
    ;
    helpTools.prototype.GetUrlImgListSize = function (fileName) {
        if (fileName === null || fileName === "" || fileName === "undefined") {
            return "/Images/Product/2400d852-abc8-4f68-844c-4d9067d8427b800_800.jpg";
        }
        else {
            return EMS_AppSetting_ImagePathInVirtual.concat("/", fileName, EMS_ImgListSize, "_", EMS_ImgListSize + ".jpg");
        }
    };
    ;
    helpTools.prototype.GetUrlImgListSizeFull = function (fileName) {
        if (fileName === null || fileName === "" || fileName === "undefined") {
            return FullDomainUrl + "/Images/Product/2400d852-abc8-4f68-844c-4d9067d8427b800_800.jpg";
        }
        else {
            return EMS_AppSetting_ImagePathInVirtual.concat(FullDomainUrl + "/", fileName, EMS_ImgListSize, "_", EMS_ImgListSize + ".jpg");
        }
    };
    ;
    helpTools.prototype.showToman = function (rial) {
        return (rial / 10).toLocaleString();
    };
    return helpTools;
}());
var help = new helpTools();
function TSCheckMobileNum(tagIdMobileNum, formId) {
    var mobileNum = $("#" + tagIdMobileNum).val().toString();
    if (mobileNum.substr(0, 2) === "09" && mobileNum.length === 11) {
        $("#" + formId).submit();
    }
    else {
        alert("لطفا شماره موبایل را 11 رقمی و به فرمت 09XXXXXXXXX  وارد نمایید");
    }
}
function TSBtnSelectColorDetails(_color) {
    document.getElementById("hiddInputColore").setAttribute("value", _color);
    debugger;
    $(".selecteColore").removeClass("selecteColore");
    $('.' + _color.replace('#', '')).addClass("selecteColore");
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
function TSChangePaymentWay(bankName) {
    if (bankName === 'Pasargad') {
        $("#btnPayMeli").addClass("d-none");
        $('#btnPayPasrgad').removeClass("d-none");
    }
    else if (bankName === 'meli') {
        $("#btnPayPasrgad").addClass("d-none");
        $('#btnPayMeli').removeClass("d-none");
    }
}
// OR
//if ($("#radio1").is(":checked")) {
//    // do something
//}
function TSGoToPayment(PasargadUrl) {
    if ($("#inputPasargadId").prop("checked")) {
        GetTokenPasargad(PasargadUrl);
    }
    if ($("#inputMeliId").prop("checked")) {
        PostMeli();
    }
}
function GetTokenPasargad(formUrl) {
    var pasargad = new PaymentPasargad(formUrl);
    pasargad.PostData();
}
function PostMeli() {
    var pasargad = new PaymentPasargad("");
    pasargad.ReadData();
    if (pasargad.validateInputDate()) {
        $("#formPaymetToContineId").submit();
    }
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
        debugger;
        this.ReadData();
        if (this.validateInputDate()) {
            TSPostObject(this.paymentViewField, this.formUrl, function (result) { return _this.inCallbackGetToken(result); });
        }
    };
    PaymentPasargad.prototype.ReadData = function () {
        this.paymentViewField.fkProvince = parseInt($("#fkProvinceId").find(":selected").attr("value"));
        //parseInt(document.getElementById("fkProvince").getAttribute("value")); 
        this.paymentViewField.address = $("#addressId").val().toString();
        this.paymentViewField.cityName = $("#cityNameId").val().toString();
        this.paymentViewField.noteForBussiness = $("#noteForBussinessId").val().toString();
        this.paymentViewField.invoiceId = parseInt(document.getElementById("invoiceId").getAttribute("value"));
        this.paymentViewField.name = $("#nameId").val().toString();
        this.paymentViewField.familyName = $("#familyNameId").val().toString();
        this.paymentViewField.company = $("#companyId").val().toString();
        this.paymentViewField.mobile = $("#mobileId").val().toString();
        this.paymentViewField.tel = $("#telId").val().toString();
        this.paymentViewField.postCode = $("#postCodeId").val().toString();
        this.paymentViewField.shippingCompany = parseInt($('input[name="shippingCompany"]:checked').val().toString());
    };
    PaymentPasargad.prototype.validateInputDate = function () {
        if (this.paymentViewField.fkProvince === 0 || this.paymentViewField.fkProvince == null) {
            alert("لطفا استان مورنظر را انتخاب نمایید");
            return false;
        }
        if (this.paymentViewField.address === "" || this.paymentViewField.address == null) {
            alert("لطفا آدرس را وارد  نمایید");
            return false;
        }
        if (this.paymentViewField.cityName === "" || this.paymentViewField.cityName == null) {
            alert("لطفا نام شهر را وارد  نمایید");
            return false;
        }
        if (this.paymentViewField.name === "" || this.paymentViewField.name == null) {
            alert("لطفا نام را وارد  نمایید");
            return false;
        }
        if (this.paymentViewField.familyName === "" || this.paymentViewField.familyName == null) {
            alert("لطفا نام خانوادگی را وارد  نمایید");
            return false;
        }
        if ((this.paymentViewField.mobile === "" || this.paymentViewField.mobile == null) &&
            (this.paymentViewField.tel === "" || this.paymentViewField.tel == null)) {
            alert("لطفا شماره تماس گیرنده را وارد  نمایید");
            return false;
        }
        if (this.paymentViewField.postCode === "" || this.paymentViewField.postCode == null) {
            alert("لطفا کد پستی را وارد  نمایید");
            return false;
        }
        return true;
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
    //  alert(valdata);
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
var YektanetProduct = /** @class */ (function () {
    function YektanetProduct(proPd) {
        this.proPd = proPd;
        this.title = proPd.ProductName;
        this.sku = proPd.FkProduct.toString();
        this.category = [proPd.CategoryName];
        //this.category.push();
        this.price = proPd.Price / 10;
        this.brand = proPd.ProductBrand;
        this.discount = (proPd.BeforDiscountPrice > proPd.Price ? proPd.BeforDiscountPrice - proPd.Price : 0) / 10;
        this.image = help.GetUrlImgListSizeFull(proPd.ProductImage);
        this.isAvailable = proPd.Available > 0 ? true : false;
    }
    YektanetProduct.prototype.callYektanet = function () {
        yektanetObj("product", "detail", this);
    };
    return YektanetProduct;
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
    debugger;
    firstColor = "";
    colorHtml = "";
    for (var _i = 0, arrPromotionProductModels_1 = arrPromotionProductModels; _i < arrPromotionProductModels_1.length; _i++) {
        var item = arrPromotionProductModels_1[_i];
        if (item.FkProduct === productId) {
            PromSelected = item;
            //  return; to do no nedd to continue break
        }
    }
    try {
        var yekta = new YektanetProduct(PromSelected);
        yekta.callYektanet();
    }
    catch (e) {
    }
    console.log("PromSelected.AvailableColors:", PromSelected.AvailableColors);
    if (PromSelected.AvailableColors !== null && PromSelected.AvailableColors !== "" && PromSelected.AvailableColors !== undefined) {
        firstColor = PromSelected.AvailableColors.split(",")[0];
        console.log("PromSelected.AvailableColors.split[0]:", PromSelected.AvailableColors.split(",")[0]);
        for (var _a = 0, _b = PromSelected.AvailableColors.split(","); _a < _b.length; _a++) {
            var _color = _b[_a];
            if (_color === "#mmm") {
                colorHtml = colorHtml + (" <a class=\"" + _color.replace('#', '') + " modalcolora multiColor\" title=\"\u0686\u0646\u062F \u0631\u0646\u06AF\" onclick=\"TSBtnSelectColorDetails('" + _color + "')\" > </a>");
            }
            else {
                colorHtml = colorHtml + (" <a style=\"background:" + _color + "\"  class=\"" + _color.replace('#', '') + " modalcolora\"  onclick=\"TSBtnSelectColorDetails('" + _color + "')\" > </a>");
            }
        }
    }
    if (PromSelected.BeforDiscountPrice > 0) {
        BeforDiscountPriceHtml = "<span class=\"old_price\">" + help.showToman(PromSelected.BeforDiscountPrice) + " \u062A\u0648\u0645\u0627\u0646 < /span>";
    }
    debugger;
    $(".modal_image_contentClass1").attr("src", help.GetUrlDetailsImg(PromSelected.ProductImage));
    $(".modal_image_contentClass2").attr("src", help.GetUrlDetailsImg(PromSelected.ProductImage1));
    //  $(".modal_image_contentClass2").attr("src", help.GetUrlDetailsImg(PromSelected.ProductImage2));
    $(".modal_image_listClass1").attr("src", help.GetUrlImgListSize(PromSelected.ProductImage));
    $(".modal_image_listClass2").attr("src", help.GetUrlImgListSize(PromSelected.ProductImage1));
    //$(".modal_image_listClass2").attr("src", help.GetUrlImgListSize(PromSelected.ProductImage2));
    if (PromSelected.ProductImage2 !== null && PromSelected.ProductImage2 !== undefined) {
        var imgs = PromSelected.ProductImage2.split(",");
        var i = 3;
        for (var _c = 0, imgs_1 = imgs; _c < imgs_1.length; _c++) {
            var img = imgs_1[_c];
            if (img !== null && img !== undefined && img !== "" && i <= 10) {
                $(".modal_image_contentClass" + i).attr("src", help.GetUrlDetailsImg(img));
                $(".modal_image_listClass" + i).attr("src", help.GetUrlImgListSize(img));
                i += 1;
            }
            //else {
            //    $("#tab" + i).remove();
            //    $("#moLi" + i).parent("div").remove();
            //}
        }
        debugger;
        for (i; i <= 11; i++) {
            $("#tab" + i).remove();
            $("#moLi" + i).parent("div").remove();
            //   $("#moLi" + i).parent("div").remove();
        }
        $('#color_div_Id').empty();
        $('#color_div_Id').append(colorHtml);
        $('#formModalId').attr("action", urlAddToCart);
        $("#modalProductNameId").html(PromSelected.ProductName);
        $("#modalPriceId").html(help.showToman(PromSelected.Price) + "تومان");
        $("#modalBeforDiscountId").html(help.showToman(PromSelected.BeforDiscountPrice) + "تومان");
        if (PromSelected.AbstractDiscription === null || PromSelected.AbstractDiscription === "" || PromSelected.AbstractDiscription === undefined) {
            //
            $("#modalDescriptionId").html(PromSelected.Discription);
        }
        else {
            $("#modalDescriptionId").html(PromSelected.AbstractDiscription);
        }
        document.getElementById("ProductIdModalId").setAttribute("value", PromSelected.FkProduct.toString());
        // $(".modal_image_contentClass").attr("src", help.GetUrlDetailsImg(PromSelected.ProductImage));
        //console.info(modalStrHtml);
        //if ($('#' + modal_boxIdStr).length <= 0) {
        //    $('#modalAreaId').append(modalStrHtml);
        //}
        // return xxx2;
    }
}
function modalSelectColorFuncTS() {
    debugger;
    var color = $("#modalSelectColorId").find(":selected").attr("value");
    $("#modalSelectColorId").attr("style", "background: " + color);
}
//# sourceMappingURL=app.js.map