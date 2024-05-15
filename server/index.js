const Websocket = require('ws');
let Connection = require('./Class/Connection')
var Player = require('./Class/Player');

const { system } = require('nodemon/lib/config');

let Server = require('./Class/Server');

var wss = new Websocket.Server({ port: 3000 }, () => {
	console.log('server has started');
});


let server = new Server();


wss.on('connection', (ws) => {
	
	var connection = new Connection();
	
	console.log('connection received')
	var player = new Player();

	server.Connected(connection,ws,player);

	ws.on('close', () => {
		console.log('connoction closed')

		connection.disconnected(ws);



	})




})
