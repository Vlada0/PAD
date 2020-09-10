"use strict";
exports.__esModule = true;
var net = require("net");
var readline = require('readline');
var client = new net.Socket();
var port = 3000;
var host = '127.0.0.1';
var userName = "";
var rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});
client.connect(port, host, function () {
    console.log('Connected');
    rl.question('Enter username ', function (answer) {
        userName = answer;
        enterData();
    });
});
client.on('data', function (data) {
    var json = JSON.parse(data.toString());
    var statusCode = json.statusCode;
    if (statusCode !== 200) {
        console.log('Cannot send your message');
    }
    askIfContinue();
});
client.on('error', function (error) {
    console.log('Error');
    rl.close();
    client.destroy();
});
function enterData() {
    rl.question('Enter topic: ', function (answer) {
        rl.question('Enter message: ', function (messageAnswer) {
            sendMessage(answer, messageAnswer);
        });
    });
}
function sendMessage(topic, message) {
    var data = {
        name: userName,
        topic: topic,
        message: message
    };
    var jsonData = JSON.stringify(data);
    client.write(jsonData);
}
function askIfContinue() {
    rl.question('Continue? ', function (answer) {
        if (answer === 'y' || answer === 'Y') {
            enterData();
        }
        else {
            rl.close();
            client.destroy();
        }
    });
}
