import * as net from "net";
import { Socket } from "dgram";
const readline = require('readline');
const client = new net.Socket();
const port = 11000;
const host = '127.0.0.1';


const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});


client.connect(port, host, function() {
    console.log('Connected');
    askUsername();
});

client.on('data', function(data){
    let json = JSON.parse(data.toString());
    let statusCode = json.statusCode;
    switch(statusCode) {
        case "200":
            askIfContinue();
            break;
        case "400":
            console.log('Invalid username');
            askUsername();
            

    }
})

client.on('error', function(error){
    console.log('Error')
    rl.close();
    client.destroy();
})

function enterData() {
    rl.question('Enter topic: ', (answer) => {
        rl.question('Enter message: ', (messageAnswer) => {
            sendMessage(answer, messageAnswer);
        })
    });
}

function sendMessage(topic, message) {
    let data = {
        topic: topic,
        message: message
    }
    let jsonData = JSON.stringify(data);
    client.write(jsonData);
}

function askIfContinue() {
    rl.question('Do you want to send message? ', (answer) => {
        if(answer === 'y' || answer === 'Y') {
            enterData();
        } else {
            rl.close();
            client.destroy();
        }
    })
}
function sendUsername(userName) {
    let data = {
        publisherName: userName
    }
    let jsonData = JSON.stringify(data);
    client.write(jsonData)
}

function askUsername() {
    rl.question('Enter username ', (answer) => {
        sendUsername(answer);
      });   
}