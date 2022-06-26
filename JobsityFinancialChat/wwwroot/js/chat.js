class Message {
    constructor(username, text, when) {
        this.userName = username;
        this.text = text;
        this.when = when;
    }
}

const username = userName;
const textInput = document.getElementById('messageText');
const whenInput = document.getElementById('when');
const chat = document.getElementById('chat');
const messagesQueue = [];

document.getElementById('submitButton').addEventListener('click', () => {
    var currentdate = new Date();
    when.innerHTML =
        (currentdate.getMonth() + 1) + "/"
        + currentdate.getDate() + "/"
        + currentdate.getFullYear() + " "
        + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })
});

function clearInputField() {
    messagesQueue.push(textInput.value);
    textInput.value = "";
}

function sendMessage() {
    let text = messagesQueue.shift() || "";
    if (text.trim() === "") return;

    let when = new Date();
    let message = new Message(username, text);
    sendMessageToHub(message);
}

function addMessageToChat(message) {
    let isCurrentUserMessage = message.userName === username;

    let container = document.createElement('div');
    container.className = isCurrentUserMessage ? "container darker bg-primary" : "container bg-light";

    let row = document.createElement('div');
    row.className = "row";
    let offset = document.createElement('div');
    offset.className = isCurrentUserMessage ? "col-md-6 offset-md-6" : "";

    let sender = document.createElement('p');
    sender.className = `sender ${isCurrentUserMessage ? "text-right text-white" : "text-left"}`;

    sender.innerHTML = message.userName;
    let text = document.createElement('p');
    text.style = "font-weight:bold";
    text.innerHTML = message.text;
    text.className = isCurrentUserMessage ? "text-right text-white" : "text-left";

    let when = document.createElement('span');
    when.className = isCurrentUserMessage ? "time-right text-light" : "time-left";
    var now = new Date();
    when.innerHTML =
        (now.getMonth() + 1) + "/"
        + now.getDate() + "/"
        + now.getFullYear() + " "
        + now.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true })

    container.appendChild(text);
    container.appendChild(sender);
    container.appendChild(when);
    offset.appendChild(container);
    row.appendChild(offset);
    chat.appendChild(row);
}
