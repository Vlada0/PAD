import * as net from "net";
import { Socket } from "dgram";
const udp = require('dgram');
const readline = require('readline');
const client = new net.Socket();
const port = 11000;
const host = '127.0.0.1';

var udpPort = 6024;
var broadcastAdr = "0.0.0.0";

const server = udp.createSocket('udp4')

server.on('listening', () => {
    const address = server.address()
    const port = address.port
    const family = address.family
    const ipaddr = address.address

    console.log("udp_server", "info", 'Server is listening at port ' + port)
    console.log("udp_server", "info", 'Server ip :' + ipaddr)
    console.log("udp_server", "info", 'Server is IP4/IP6 : ' + family)
    server.setBroadcast(true);
    var message = Buffer.from("Give me port!");
    console.log(message)
    server.send(message, 63347, broadcastAdr);
})

server.on('message', (msg, info) => {
    console.log("TEST")
    let json = JSON.parse(msg.toString());
    console.log(json);
    // client.connect(json.port, host, () => {
    //     console.log('Connected');
    //     askUsername();
    // });
})

server.bind();

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

client.on('data', (data) => {
    let json = JSON.parse(data.toString());
    let statusCode = json.statusCode;
    switch(statusCode) {
        case 200:
            askIfContinue();
            break;
        case 400:
            console.log('Invalid username');
            askUsername();
    }
})

client.on('error', (error) => {
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