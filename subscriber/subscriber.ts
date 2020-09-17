import * as net from "net";
import { Socket } from "dgram";
const udp = require('dgram');
const readline = require('readline');
const client = new net.Socket();
const port = 11000;
const host = '127.0.0.1';
var udpPort = 6024;
var broadcastAdr = "0.0.0.0";

const rl = readline.createInterface({
    input: process.stdin,
    output: process.stdout
});

const server = udp.createSocket('udp4')

server.on('listening', () => {
    const address = server.address()
    const port = address.port
    const family = address.family
    const ipaddr = address.address

    console.log("udp_server", "info", 'Server is listening at port ' + port)
    console.log("udp_server", "info", 'Server ip :' + ipaddr)
    console.log("udp_server", "info", 'Server is IP4/IP6 : ' + family)
})

server.on('message', (msg, info) => {
    console.log("TEST!!@")
    console.log(msg.toString());
    // client.connect(json.port, host, () => {
    //     rl.question('Enter topic:', (topic) => {
    //         subscribeOn(topic);
    //     })
    // })
})

server.bind(function() {
    server.setBroadcast(true);
    var message = Buffer.from("Give me port!");
    server.send(message, 0, message.length, udpPort, broadcastAdr);
})

// const listener = udp.createSocket('udp4')

// listener.on('message', (msg, info) => {
//     console.log('Data received from server : ' + msg.toString())
//     console.log('Received %d bytes from %s:%d\n', msg.length, info.address, info.port)
// })

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

function subscribeOn(topic) {
    let data = {
        subscribe: topic
    }
    let jsonData = JSON.stringify(data);
    client.write(jsonData);
}

function unsubscribeOn(topic) {
    let data = {
        unsubscribe: topic
    }
    let jsonData = JSON.stringify(data);
    client.write(jsonData);
}