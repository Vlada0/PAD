import * as net from "net";
import { Socket } from "dgram";
import { v4 as uuidv4 } from 'uuid';
const udp = require('dgram');
const readline = require('readline');
const client = new net.Socket();
const host = '127.0.0.1';

var udpPort = 6024;
var broadcastAdr = "0.0.0.0";

const server = udp.createSocket('udp4')

let category = '';
let location = '';
let isConnected = false;
let id;

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
		if(i!==port){
			server.send(message, i, broadcastAdr);
		}
    }
})

server.on('message', (msg, info) => {
    let json;
    try {
        json = JSON.parse(msg.toString());
      } catch (e) {
        return
    }

    let port = json.port;
    if (typeof port !== 'undefined') {
        /*if (isConnected) {
            client.destroy();
            isConnected = false;
        }*/

        client.connect(port, host, () => {
			rl.resume();
            console.log('Connected');
            setupPublisher();
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
            console.log('Broker recieved data')
            break;
        case 400:
            console.log('Wrong data');
    }
})

client.on('error', (error) => {
    console.log('Missing connection with broker')
    rl.pause();
    client.destroy();
})

function sendMessage() {
    let value = Math.floor(Math.random() * 10) + 1;
    let dataToSend = {
        operation: 'publish',
        operationInfo: {
            data: value,
            category: category,
            location: location,
            id: id
        }
    }
    console.log(dataToSend);
    let jsonData = JSON.stringify(dataToSend);
    client.write(jsonData);
}

function registerDevice() {
    let dataToSend = {
        operation: "registerDevice",
        operationInfo: {
            id: id
        }
    }

    let jsonData = JSON.stringify(dataToSend);
    client.write(jsonData);
}

function sendUsername(userName) {
    let data = {
        publisherName: userName
    }
    let jsonData = JSON.stringify(data);
    client.write(jsonData)
}

function setupPublisher() {
    rl.question('Enter category: ', (answer) => {
        rl.question('Enter location: ', (locationAnswer) => {
            category = answer;
            location = locationAnswer;
            id = uuidv4();
            registerDevice()
            setInterval(sendMessage, 2000);
        })
    })
}

startServer();