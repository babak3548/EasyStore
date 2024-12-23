
//   var usrGraph;

var chatDivId = "chatArea";
var chatHub;
$(function () {
     chatHub = $.connection.ChatHub;
    //chatHub.logging = true;
    registerClientMethods(chatHub);

    // Start Hub
    $.connection.hub.start().done(function () {
        registerEvents(chatHub)
        chatHub.server.receiveMessageServer("testUserId", "test message");
    });
    
   // $('#chatTest').click();
        //function () {
    //    chatHub.server.receiveMessageServer("testUserId", "test message");
    //});
});

function test() {
    alert("call another windows");
}



function syncSesToConnChatW(sestionUserId,toSestionUserId, subject) {
    chatHub.server.syncSesToConnChatWin(sestionUserId, toSestionUserId, subject);
}
function SyncConneAndCreWinPri(toSestionUserId, userName, subject) {

}

function registerClientMethods(chathub) {
    
    chathub.client.OpenWindowChat = function (toSestionUserId, subject)
    {
        openChatWindow(toSestionUserId, subject);
    }

    chathub.client.CreateWindowPravite= function (toUserConnId, subject)
    {
        CreateWindowPrivat(toUserConnId, subject)
    }

    chathub.client.ReceiveMassage = function (ToUserId, UserName, message) {
     MangeMessage(message,ToUserId, UserName, chathub);
    }

    // On User Disconnected
    chathub.client.onUserDisconnected = function (ToUserId, userName) {

        var disc = $('<div class="disconnect">"' + userName + '" خارج از دسترس</div>');
        $(disc).hide();
        $('#divusers').prepend(disc);
        $(disc).fadeIn(200).delay(2000).fadeOut(200);
        AddMessage("کاربر از دسترس خارج شد", ToUserId, userName);
    }


}

function RequestChatSestion(sestionUserId, toSestionUserId, subject) {
    chatHub.server.requestChatWith(sestionUserId, toSestionUserId, subject);
}



function CreateWindowPrivat(toUserConnId, subject) {
    var div = '<div id="' + toUserConnId + '" class="ui-widget-content draggable" rel="0">' +
               '<div class="header">' +
                  '<div  style="float:right;">' +
                      '<img id="imgDelete"  style="cursor:pointer;" src="/Images/delete.png"/>' +
                   '</div>' +

                   '<span class="selText" rel="0">' + subject +'</span>' +
               '</div>' +
               '<div id="divMessage" class="messageArea">' +

               '</div>' +
               '<div class="buttonBar">' +
                  '<input id="txtPrivateMessage" class="msgText" type="text"   />' +
                  '<input id="btnSendMessage" class="submitButton button" type="button" value="Send"   />' +
               '</div>' +
            '</div>';

    var $div = $(div);

    // DELETE BUTTON IMAGE
    $div.find('#imgDelete').click(function () {
        $('#' + toUserConnId).remove();
    });

    // Send Button event
    $div.find("#btnSendMessage").click(function () {

        $textBox = $div.find("#txtPrivateMessage");
        var msg = $textBox.val();
        if (msg.length > 0) {

            chatHub.server.ReceiveMessageServer(toUserConnId, msg);
            $textBox.val('');
        }
    });

    // Text Box event
    $div.find("#txtPrivateMessage").keypress(function (e) {
        if (e.which == 13) {
            $div.find("#btnSendMessage").click();
        }
    });

    AddDivToContainer($div);
    // return $div;
}

function registerEvents() {

}

function MangeMessage(message, ToUserId, UserName, chathub) {

    if ($('#' + ToUserId).length == 0)
    {
        CreateWindow(ToUserId, UserName, chathub);
        AddMessage(message, ToUserId, UserName);
    }
    else {
        AddMessage(message, ToUserId,UserName);
    }
}

function AddMessage(message,ToUserId,UserName) {
    $('#' + ToUserId).find('#divMessage').append('<div class="message"><span class="userName">' + UserName + '</span>: ' + message + '</div>');
    var height = $('#' + ToUserId).find('#divMessage')[0].scrollHeight;
    $('#' + ToUserId).find('#divMessage').scrollTop(height);
}



function AddDivToContainer($div) {
    $('#' + chatDivId).prepend($div);

    //$div.draggable({

    //    handle: ".header",
    //    stop: function () {

    //    }
    //});

    ////$div.resizable({
    ////    stop: function () {

    ////    }
    ////});
}


