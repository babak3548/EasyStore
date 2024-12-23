//console.log(gapi.auth.getToken());
//immediate
var apiKeyG = 'AIzaSyD21DNS9wpUCv9OLrKULU3KhhS-pFIhwDU';
var token;
var emails;
var SelectEmls;
var cntManE = 0;
var config = {
    'client_id': '354900158341-8e1ab3or7c5eq3lgrq7oaoiejl2qk9fu.apps.googleusercontent.com',
    'immediate': 'false',
    'scope': 'https://www.google.com/m8/feeds'
};

var emailReg = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
function callBakAuthG() {
    Reset();
    LodingImg("hedMng");
    token = gapi.auth.getToken();
    try {
        $.get("https://www.google.com/m8/feeds/contacts/default/full?alt=json&access_token=" + token.access_token + "&max-results=10&v=3.0",
    function (response) {
        emails = EmailToArray(response.feed.entry);
        emails = DelRepEmilTotal(emails);
        AddEmailToDiv(emails);
        remLodImg();
    });
    } catch (e) {
        remLodImg();
    }

}
function Reset() {
    emails = null;
    $('.ED').each(function () {
        $(this).remove();
    }, this);
}
//var callbakPG = function () {
//    AddEmailToDiv(emails);
//}
function Email() {
    address = "";
    fullName = "";
}

function EmailToArray(entries) {
    emails1 = new Array();
    emails2 = new Array();
    emails = new Array();
    var j = 0;
    var k = 0;
    for (var i = 0; i < entries.length; i++) {
        try {
            var email = new Email();
            email.address = entries[i].gd$email[0].address;
            if (entries[i].gd$name != null) {
                email.fullName = entries[i].gd$name.gd$fullName.$t;
                emails1[j] = email;
                j++;
            }
            else {
                email.fullName = "";
                emails2[k] = email;
                k++;
            }
        } catch (e) { }
    }
    emails = emails1.concat(emails2);
    return emails;
}

function DelRepEmilTotal(emls) {
    emails1 = new Array();
    var noExist = true;
    var k = 0;
    for (var i = 0; i < emls.length; i++) {
        for (var j = 0; j < emails1.length; j++) {
            if (emls[i].address == emails1[j].address) {
                noExist = false;
            }
        }
        if (noExist) {
            var nEmail = new Email();
            nEmail.address = emls[i].address;
            nEmail.fullName = emls[i].fullName;
            emails1[k] = nEmail;
            k++;
        }
        noExist = true;
    }

    return emails1;
}

function AdEmToDiv(em) {
    var div = '';
    var fullname = "";
    if (em.fullName != "") {
        fullname = em.fullName;
    }
    else {
        fullname = em.address;
    }
    div = "<div class='ED'><input class='inE' value=" + em.address + " type='checkbox' checked='checked' /><span title='" + em.address + "'>" + fullname + "</span> </div>"
    $("#AD").after(div);
}

function AddEmailToDiv(emils) {

    for (var i = emils.length - 1; i >= 0 ; i--) {
        AdEmToDiv(emils[i]);
    }
}

function chkAll(elIid) {
    try {
        if ($('#' + elIid).get(0).checked) {
            $('.inE').each(function () {
                this.checked = true;
            }, this);
        }
        else {
            $('.inE').each(function () {
                this.checked = false;
            }, this);
        }
    } catch (e) {
        alert(e.message);
    }
}

function MaAdd() {
    $('#ManualAdd').toggle();
}
function AddEm() {
    var value = $('#TEmail').val();
    var email = new Email();
    if (emailReg.test(value)) {
        if (cntManE < 10) {
            cntManE++;
            email.address = value;
            email.fullName = value;
            AdEmToDiv(email);
            $('#TEmail').val('');
        }
        else {
            alert('به صورت دستی بیشتر از 10 ایمیل نمی توانید وارد نمایید');
        }
    }
    else {
        alert('لطفا ایمیل را به صورت صحیح وارد نمایید');
    }
}

function PosToSe() {
    var emailsSend = new Array();
    for (var i = 1; i < $('.inE:checked').length; i++) {//خط اول 'همه' حذف
        emailsSend[i - 1] = $('.inE')[i].value;
    }
    SelectEmls = emailsSend;
    LodingImg("hedMng");
    $.ajax({
        url: "http://localhost:8120/ManageM/GetBodyEmailAdsBuss",
        type: "Post",
        data:{"emailsTotal": JSON.stringify(emails)},
        success: function (result) {
            $('#bdyMng').get(0).innerHTML = result;
            $("#inputCleditor").cleditor({ width: 778, height: 500 });
            remLodImg();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
            remLodImg();
        }
    });
 //   $.post("", { emailsTotal: JSON.stringify(emails) }, callbakPG, 'json');
}

var callbakPG = function (result) {
    $('#bdyMng').get(0).innerHTML = htmlBodyStr;
}

function SendsEml() {//emailsSend, string htmBody, int IdMessage
    var mesId = $("#MessageId").val();
    var htmlBdy = $("#inputCleditor").get(0).innerHTML;
    LodingImg("hedMng");
    $.ajax({
        url: "http://localhost:8120/ManageM/sendAdsEmailBus",
        type: "Post",
        data: { "emailsSend": JSON.stringify(SelectEmls), "IdMessage": mesId, "htmBody": htmlBdy },
        success: function (result) {
            $('#bdyMng').get(0).innerHTML = result;
            remLodImg();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
            remLodImg();
        }
    });
}

$(document).ready(function () {
    $("#TEmail").keyup(function (e) {
        if (e.which == 13) {
            AddEm();
        }
    });

    $("#rghMng a").click(function (e) {
        $("#rghMng a").each(function () {
            $(this).attr('class', 'aMu');
        });
        $(this).attr('class', 'aMu sAMu');
    });
});
