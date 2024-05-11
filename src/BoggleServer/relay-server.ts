// start a relay websocket server to 
// communicate with the tcp

import { WebSocketServer } from "ws";
import net from 'net';

const port = 8080;

const wss = new WebSocketServer({port});

console.log(`Listening at ${port}`);

wss.on('connection', (ws) => {
    // handles new connections

    let tcpClient;
    ws.on('message', (message) => {
        const data = JSON.parse(message.toString());
        if (data.type == 'join') {
            tcpClient = new net.Socket();
            tcpClient.connect(data.tcpPort, data.tcpIp, () => {
                console.log(`connected to tcp server at ip ${data.tcpIp} and port ${data.tcpPort}`);
            });

            tcpClient.on('data', (data) => {
                console.log(`received message from tcp ${data.toString()}`);
                // send it websocket client
                ws.send(data.toString());
            });

            tcpClient.on('close', () => {
                console.log('tcp connection closed.');
                ws.send('game ended');
            });

            tcpClient.on('error', (error) => {
                console.error('tcp error', error);
                ws.send(`tcp error: ${error.message}`);
            });
        }
    })

    ws.on('close', () => {
        console.log('websocket client disconnected');
    });

    ws.on('error', (error) => {
        console.error('ws error', error);
    })
})

