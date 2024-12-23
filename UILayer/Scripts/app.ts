
//var helpTools = require('./ExtentionMethods.js');
//import {helpTools} from "./ExtentionMethods"; 

var urlAddToCart: string;
var EMS_AppSetting_ImagePathInVirtual: string;
var EMS_ImgDetailsSize: string;
var EMS_ImgListSize: string;
var EMS_ImgListSizeUI: string;
var FullDomainUrl: string = "https://EasyStore.com";
var yektanetObj: any;

class helpTools {

    GetUrlDetailsImg(fileName: string | null | undefined) {
        if (fileName === null || fileName === "" || fileName === "undefined") { return "/Images/Product/2400d852-abc8-4f68-844c-4d9067d8427b800_800.jpg"; }
        else {
            return EMS_AppSetting_ImagePathInVirtual.concat("/", fileName, EMS_ImgDetailsSize, "_", EMS_ImgDetailsSize + ".jpg");
        }
    };

    GetUrlImgListSize(fileName: string | null | undefined) {
        if (fileName === null || fileName === "" || fileName === "undefined") { return "/Images/Product/2400d852-abc8-4f68-844c-4d9067d8427b800_800.jpg"; }
        else {
            return EMS_AppSetting_ImagePathInVirtual.concat("/", fileName, EMS_ImgListSize, "_", EMS_ImgListSize + ".jpg");
        }
    };

    GetUrlImgListSizeFull(fileName: string | null | undefined) {
        if (fileName === null || fileName === "" || fileName === "undefined") { return FullDomainUrl + "/Images/Product/2400d852-abc8-4f68-844c-4d9067d8427b800_800.jpg"; }
        else {
            return EMS_AppSetting_ImagePathInVirtual.concat(FullDomainUrl + "/", fileName, EMS_ImgListSize, "_", EMS_ImgListSize + ".jpg");
        }
    };

    showToman(rial: number) {
        return (rial / 10).toLocaleString();
    }
}

let help = new helpTools();

function TSCheckMobileNum(tagIdMobileNum: string, formId: string) {
    var mobileNum = $("#" + tagIdMobileNum).val().toString();
    if (mobileNum.substr(0, 2) === "09" && mobileNum.length === 11) {
        $("#" + formId).submit();
    }
    else {
        alert("لطفا شماره موبایل را 11 رقمی و به فرمت 09XXXXXXXXX  وارد نمایید");
    }
}
function TSBtnSelectColorDetails(_color: string) {

    document.getElementById("hiddInputColore").setAttribute("value", _color);
    debugger;
    $(".selecteColore").removeClass("selecteColore");
    $('.' + _color.replace('#', '')).addClass("selecteColore");
}

class Student {
    fullName: string;
    constructor(public firstName: string, public middleInitial: string, public lastName: string) {
        this.fullName = firstName + " " + middleInitial + " " + lastName;
    }
}

interface Person {
    firstName: string;
    lastName: string;
}

function greeter(person: Person) {
    return "Hello, " + person.firstName + " " + person.lastName;
}

let user = new Student("Fred", "M.", "Smith");

class PaymentViewField {
    public invoiceId: number;
    public fkProvince: number;
    public address: string;
    public cityName: string;
    public noteForBussiness: string;

    public name: string;
    public familyName: string;
    public company: string;
    public mobile: string;
    public tel: string;
    public postCode: string;
    public shippingCompany: number;

    constructor() { }
}
function TSChangePaymentWay(bankName: string) {
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
function TSGoToPayment(PasargadUrl: string) {
        if ($("#inputPasargadId").prop("checked")) {
            GetTokenPasargad(PasargadUrl);
        }
        if ($("#inputMeliId").prop("checked")) {
            PostMeli();
        }
}

function GetTokenPasargad(formUrl: string) {
    let pasargad = new PaymentPasargad(formUrl);
    pasargad.PostData();
}
function PostMeli() {
    let pasargad = new PaymentPasargad("");
    pasargad.ReadData();
    if (pasargad.validateInputDate()) {
        $("#formPaymetToContineId").submit();
    }
}

function setBIPC(bipId: string) {
    var cn = $("#cn" + bipId).val();
    $("#hd" + bipId).val(bipId + "," + cn);
}

class PaymentPasargad {
    private formUrl: string;
    private paymentViewField: PaymentViewField;

    constructor(_formUrl: string) {
        this.formUrl = _formUrl;
        this.paymentViewField = new PaymentViewField();
    }

    public inCallbackGetToken(result: any) {
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
    }

    public PostData() {
        debugger;
        this.ReadData();
        if (this.validateInputDate()) {
            TSPostObject(this.paymentViewField, this.formUrl, (result) => this.inCallbackGetToken(result));
        }
    }

    public ReadData() {
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
    }

    public validateInputDate(): boolean {
        if (this.paymentViewField.fkProvince === 0 || this.paymentViewField.fkProvince == null) { alert("لطفا استان مورنظر را انتخاب نمایید"); return false; }
        if (this.paymentViewField.address === "" || this.paymentViewField.address == null) { alert("لطفا آدرس را وارد  نمایید"); return false; }
        if (this.paymentViewField.cityName === "" || this.paymentViewField.cityName == null) { alert("لطفا نام شهر را وارد  نمایید"); return false; }
        if (this.paymentViewField.name === "" || this.paymentViewField.name == null) { alert("لطفا نام را وارد  نمایید"); return false; }
        if (this.paymentViewField.familyName === "" || this.paymentViewField.familyName == null) { alert("لطفا نام خانوادگی را وارد  نمایید"); return false; }
        if ((this.paymentViewField.mobile === "" || this.paymentViewField.mobile == null) &&
            (this.paymentViewField.tel === "" || this.paymentViewField.tel == null)
        ) { alert("لطفا شماره تماس گیرنده را وارد  نمایید"); return false; }
        if (this.paymentViewField.postCode === "" || this.paymentViewField.postCode == null) { alert("لطفا کد پستی را وارد  نمایید"); return false; }

        return true;
    }
}


function TSPostForm(fromId: string, formUrl: string, inCallback: { (result: any): void; }) {
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
        success: (result) => inCallback(result)
    });
}

function TSPostObject(valdata: object, formUrl: string, inCallback: { (result: any): void; }) {
    //to get alert popup  
    //  alert(valdata);
    $.ajax({
        url: formUrl,
        type: "POST",
        dataType: 'json',
        //  contentType: "application/json;charset=utf-8",
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: valdata,
        success: (result) => inCallback(result)
        //    function (result) {
        //    console.log("inCallbackGetToken.result1:", result);
        //}
    });



}

class PromotionProductModel {
    public PromotionId: number;
    public PromotionTypes: number;
    public FkProduct: number;
    public Order: number;
    public ExpireDateTime: string;
    public ProductName: string;
    public ProductImage: string;
    public ProductImage1: string;
    public FkCategory: number;
    public CategoryName: string;
    public Price: number;
    public ProductBrand: string;
    public ProductImage2: string;


    public AvailableColors: string;
    public Discription: string;
    public AbstractDiscription: string;

    public BeforDiscountPrice: number;
    public Available: number;

}

class YektanetProduct {
    public title: string;//  : "رب گوجه فرنگی قوطی بزرگ",
    public sku: string; //    : "D153-Ij89d", // شناسه محصول
    public category: string[];//    : ["مواد غذایی", "رب"],
    public price: number; //   : 11300, // تومان
    public brand: string;//    : "تبرک",
    public discount: number; //   : 30, // درصد
    public image: string; //  : 'https://www.yektanet.com/yektanet-logo.jpg',
    public isAvailable: boolean; // : true, // محصول در حال حاضر موجود است


    constructor(public proPd: PromotionProductModel) {
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
    callYektanet() {
        yektanetObj("product", "detail", this);
    }

}

var arrPromotionProductModels: Array<PromotionProductModel>;


function setArrPromotionProductModels(arrPro: Array<PromotionProductModel>) {
    console.log("arrPro:", arrPro);
    arrPromotionProductModels = arrPro;
    console.log("arrPromotionProductModels:", arrPromotionProductModels);
}


function TSrenderHtmlModal(productId: number) {
    let PromSelected: PromotionProductModel;
    let colorHtml: string;
    let firstColor: string;
    let BeforDiscountPriceHtml: string;
    debugger;
    firstColor = "";
    colorHtml = "";
    for (var item of arrPromotionProductModels) {
        if (item.FkProduct === productId) {
            PromSelected = item;
            //  return; to do no nedd to continue break
        }
    }
    try {
        var yekta = new YektanetProduct(PromSelected);
        yekta.callYektanet();
    } catch (e) {

    }


    console.log("PromSelected.AvailableColors:", PromSelected.AvailableColors);
    if (PromSelected.AvailableColors !== null && PromSelected.AvailableColors !== "" && PromSelected.AvailableColors !== undefined) {
        firstColor = PromSelected.AvailableColors.split(",")[0]
        console.log("PromSelected.AvailableColors.split[0]:", PromSelected.AvailableColors.split(",")[0]);

        for (var _color of PromSelected.AvailableColors.split(",")) {
            if (_color === "#mmm") {
                colorHtml = colorHtml + ` <a class="${_color.replace('#', '')} modalcolora multiColor" title="چند رنگ" onclick="TSBtnSelectColorDetails('${_color}')" > </a>`;
            }
            else {
                colorHtml = colorHtml + ` <a style="background:${_color}"  class="${_color.replace('#', '')} modalcolora"  onclick="TSBtnSelectColorDetails('${_color}')" > </a>`;
            }
        }
    }


    if (PromSelected.BeforDiscountPrice > 0) {
        BeforDiscountPriceHtml = `<span class="old_price">${help.showToman(PromSelected.BeforDiscountPrice)} تومان < /span>`;
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

        let i = 3;
        for (var img of imgs) {
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

