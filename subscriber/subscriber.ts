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
    rl.question('Enter topic:', (topic) => {
        subscribeOn(topic);
    })
})

rl.on('line', (topic) => {
    console.log(topic);
    let arr = topic.split(' ');
    if(arr.shift() === 'rem') {
        unsubscribeOn(arr.join(' '));
    } else {
        subscribeOn(topic);
    }
})

client.on('data', function(data) {
    let json = JSON.parse(data.toString());
    let statusCode = json.statusCode;

    switch(statusCode) {
        case 200:
            console.log('Successfull subscribtion')
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