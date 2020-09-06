import * as net from "net";
import { Socket } from "dgram";
const readline = require('readline');
const client = new net.Socket();
const port = 3000;
const host = '127.0.0.1';

let userName = "";

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});


client.connect(port, host, function() {
    console.log('Connected');
    rl.question('Enter username ', (answer) => {
        userName = answer;
        enterData();
      });   
});

client.on('data', function(data){
    let json = JSON.parse(data.toString());
    let statusCode = json.statusCode;

    if(statusCode !== 200) {
        console.log('Cannot send your message');
    } 
    askIfContinue();
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
        name: userName,
        topic: topic,
        message: message
    }
    let jsonData = JSON.stringify(data);
    client.write(jsonData);
}

function askIfContinue() {
    rl.question('Continue? ', (answer) => {
        if(answer === 'y' || answer === 'Y') {
            enterData();
        } else {
            rl.close();
            client.destroy();
        }
    })
}