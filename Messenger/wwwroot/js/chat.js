class Message {
    constructor(username, text, sendtime) {
        this.userName = username;
        this.text = text;
        this.sendtime = sendtime;
    }
}

// userName is declared in razor page.
const username = document.getElementById("user").value;
const fullname = document.getElementById("fullname").value;
const textInput = document.getElementById('messageText');
const sendTimeInput = document.getElementById('sendTime');
const chat = document.getElementById('chat');
const messagesQueue = [];

function clearInputField() {
    if (messagesQueue) {
        messagesQueue.push(textInput.value);
        textInput.value = "";
    }
}

function sendMessage() {
    let text = messagesQueue.shift() || "";
    
    let sendTime = new Date();
    let message = new Message(username, text, sendTime);
    sendMessageToHub(message);
}

function addMessageToChat(message) {
    let isCurrentUserMessage = message.userName === username;

    let row = document.createElement('div');
    row.className = "row";

    let col = document.createElement('div');
    col.className = isCurrentUserMessage ? "col-md-6 offset-md-6" : "col-md-6 offset-md-6 align-left";

    let container = document.createElement('div');
    container.className = isCurrentUserMessage ? "container darker bg-primary" : "container bg-light";

    let sender = document.createElement('p');
    sender.className = isCurrentUserMessage ? "sender text-right text-white" : "sender text-left";
    sender.innerHTML = fullname;

    let text = document.createElement('p');
    text.className = isCurrentUserMessage ? "text-right text-white" : "text-left";
    text.innerHTML = message.text;
    
    let sendtime = document.createElement('span');
    sendtime.className = isCurrentUserMessage ? "time-right text-light" : "time-left";

    var currentdate = new Date();
    sendtime.innerHTML =
        currentdate.getDate() + "."
        + ("0" + (currentdate.getMonth() + 1)).slice(-2) + "."
    + currentdate.getFullYear() + " "
    + currentdate.toLocaleString([], { hour: '2-digit', minute: '2-digit', second: '2-digit' })

    if (isCurrentUserMessage) {
        let removebtn = document.createElement('input');
        removebtn.type = "image";
        removebtn.src = "/img/remove.png";
        removebtn.className = "remove-button";
        removebtn.alt = "X";
        removebtn.id = message.sendTime;
        removebtn.onclick = function ()
        {
            $.ajax({
                url: '/Home/RemoveMes',
                type: "POST",
                data: { message: message },
                success: function (data) {
                    let mes = document.getElementById(message.sendTime);
                    mes.parentElement.parentElement.parentElement.remove();
                },
                error: function () {
                    alert("Fail");
                }
            });
        };
        container.appendChild(removebtn);
    }
    container.appendChild(sender);
    container.appendChild(text);
    container.appendChild(sendtime);
    col.appendChild(container);
    row.appendChild(col);
    chat.appendChild(row);

    var element = document.getElementById("chat");
    if (element) {
        element.scrollTop = element.scrollHeight;
    }
}

function removeMessage(id) {
    $.ajax({
        url: '/Home/Remove',
        type: "POST",
        data: { id: id },
        success: function (data) {
            document.getElementById(id).remove();
        },
        error: function () {
            alert("Fail");
        }
    });
}

window.onload = function updateScroll() {
    var element = document.getElementById("chat");
    if (element) {
        element.scrollTop = element.scrollHeight;
    }
}
