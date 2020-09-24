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

let isConnected = false;
let deviceType;

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
    let json;

    try {
        json = JSON.parse(msg.toString());
      } catch (e) {
        console.log(msg.toString());
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
            setupSubscriber();
            rl.question('Enter topic:', (topic) => {
                subscribeOn(topic);
            })
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

rl.on('line', (topic) => {
    let arr = topic.split(' ');
    if(arr.shift() === 'rem') {
        unsubscribeOn(arr.join(' '));
    } else {
        subscribeOn(topic);
    }
})

client.on('error', () => {
    console.log('Cannot connect');
    rl.close();
    client.destroy();
})

client.on('data', (data) => {
    let json = JSON.parse(data.toString());
    let statusCode = json.statusCode;

    switch(statusCode) {
        case 200:
            console.log('Successfull operation')
            break;
        case  401: 
            console.log('Topic already exists')
            break;
        case 402:
            console.log('Topic doesn\'t exists')
            break;
    }

    if(typeof statusCode === 'undefined') {
        console.log(json.username + ' ' + json.topic + ' ' + json.message);
    }
})

function subscribeDeviceOn(location, category) {
    let dataToSend = {
        operation: 'subscribeDevice',
        operationInfo: {
            location: location,
            category: category
        }
    }
    let jsonData = JSON.stringify(dataToSend);
    client.write(jsonData);
}

function subscribeOn(keyWord) {
    let dataToSend = {
        operation: 'subscribe',
        operationInfo: {
            keyWord: keyWord
        }
    }
    let jsonData = JSON.stringify(dataToSend);
    client.write(jsonData);
}

function unsubscribeOn(keyWord) {
    let dataToSend = {
        operation: 'unsubscribe',
        operationInfo: {
            keyWord: keyWord
        }
    }
    let jsonData = JSON.stringify(dataToSend);
    client.write(jsonData);
}

function setupSubscriber() {
    selectDevice();
    switch(deviceType) {
        case '1':
            subscribeDeviceOn('temperature', 'kitchen')
            break;
        case '2': 
            subscribeDeviceOn('brightness', 'bedroom')
            break;
        case '3':
            startUserFlow();
            break;
    }
}

function selectDevice() {
    rl.question('Select device \n1. Smart window \n2. Lamp \n3. Cellphone', (answer) => {
        if(answer === '1' || answer === '2' || answer === '3') {
            deviceType = answer;
            
        } else {
            console.log('Wrong answer');
            return setupSubscriber();
        }
    })
}

function startUserFlow() {
    while(true) {
        rl.question('Select option \n1. Subscribe \n2. Unsubscribe', (answer) => {
            switch(answer) {
                case '1':
                    rl.question('', (answer) => {
                        subscribeOn(answer);
                    });
                    break;
                case '2':
                    rl.question('', (answer) => {
                        unsubscribeOn(answer);
                    });
                default:
                    return;
            }
        })
    }
}

startServer();