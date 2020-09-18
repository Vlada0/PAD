import * as net from "net";
import { Socket } from "dgram";
const udp = require('dgram');
const readline = require('readline');
const client = new net.Socket();
const host = '127.0.0.1';

var udpPort = 6024;
var broadcastAdr = "0.0.0.0";

const server = udp.createSocket('udp4')

let isConnected = false;

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
    for(let i = 1; i < 65535; i++) {
        server.send(message, i, broadcastAdr);
    }
})

server.on('message', (msg, info) => {
    console.log("TEST")
    let json;

    try {
        json = JSON.parse(msg.toString());
      } catch (e) {
        console.log("err");
        return
    }

    let port = json.port;
    if (typeof port !== 'undefined') {
        if (isConnected) {
            client.destroy();
            isConnected = false;
        }

        client.connect(port, host, () => {
            console.log('Connected');
            askUsername();
        });
    }
});

server.on('error', (error) => {
    console.log(error);
    if (error.code === 'EADDRINUSE') {
        startServer();
    }
})

function startServer() {
    let port = Math.floor(Math.random() * 65536) + 1;
    server.bind(port, '127.0.0.1');
}


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

startServer();