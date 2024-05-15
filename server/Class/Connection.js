require('./GameLobby')
let EventUpdate = require('./EventUpdate');
let Event = require('../custom/event')
require('./Room')
module.exports = class Connection {
    constructor() {
        this.ws;
        this.player;
        this.room;
        this.lobby;
        this.server;
        this.evup;
        this.players = {};
        this.ev = new Event();
        this.messageData;
        this.lobbyStatus;


    }

    Connected(ws, player) {
        let ev = this.ev;
        this.evup = new EventUpdate();
        ev.send(this.ws, 'register', { id: this.player.id });
        this.evup.player = this.player;
        this.evup.ev = ev;
        this.evup.ws = this.ws;
        this.evup.Connection = this;
    }

    messageReciver() {
        let ws = this.ws;

        let ev = this.ev;
        
        
        ws.on('message', (data) => {
            this.messageData = data.toString();

            if (this.lobbyStatus == "PlayGame") {
                
                try {
                    this.evup.room = this.room;
                    this.evup.eventUpdate(this.messageData,this.ws);
                }
                catch (error) {
                    console.log(error);
                }
                
                
                
            }

            ev.on('latency', this.messageData, (data) => {

                ev.send(this.ws, 'latency',{id: this.player.id});
            })

            ev.on('mapLoaded', this.messageData, (data) => {
                if (this.room.connections[this.room.connections.length - 1].player.id == data.id) {
                    
                    this.lobby.spawnPlayers(this.room);
                }

            })

            ev.on('initiateGame', this.messageData, (data) => {
                
                if (this.player.id == data.id) {
                    this.player.map = data.map;
                    this.player.mode = data.mode;
                    console.log("hello  " + data.id)
                    
                    this.addedToRoom(this);
                    this.lobby.OnstartLobby(this);
                    
                }



            })

        })

    }

    addedToRoom(con) {
        let ev = con.ev;
        let ws = con.ws;
        
        let data = {

            'playerID': con.player.id
        }
        console.log(con.player.id)
        ev.send(ws, "joinedRoom", data);
        
    }

    // EventUpdate() {
    //     let player = this.player;
    //     let ev = this.ev;

    //     let data = this.messageData;
    //     let ws = this.ws;



    //     ev.on('updatePosition', data, (data) => {
    //         player.position.x = parseFloat(data.position.x);
    //         player.position.y = parseFloat(data.position.y);
    //         player.position.z = parseFloat(data.position.z);
    //         ev.lobbyBroadcast(ws, this.room.connections, 'updatePosition', player);

    //     })

    //     ev.on('updateRotation', data, (data) => {

    //         player.rotation.x = parseFloat(data.rotation.x);
    //         player.rotation.y = parseFloat(data.rotation.y);
    //         player.rotation.z = parseFloat(data.rotation.z);
    //         ev.lobbyBroadcast(ws, this.room.connections, 'updateRotation', player);
    //     })
    //     ev.on('updateGunRotation', data, (data => {

    //         player.gunRotation = parseFloat(data.gunRotation);
    //         ev.lobbyBroadcast(ws, this.room.connections, 'updateGunRotation', player);
    //     }))
    //     ev.on('updateHealth', data, (data => {
    //         // for (var id in players) {
    //         //     if (id == data.hitedID) {
    //         //         players[id].health -= data.gunDamageAmount;
    //         //         console.log('player ' + players[id].id + " hitted. his health is " + players[id].health)
    //         //     }
    //         // }

    //         this.room.connections.forEach(con => {
    //             if (con.player.id == data.hitedID) {
    //                 con.player.health -= data.gunDamageAmount;
    //                 console.log('player ' + con.player.id + " hitted. his health is " + con.player.health)
    //                 con.lobby.UpdateDeadPlayer(con);
    //             }
    //         })

    //     }))










    // }

    roomIsReady() {
        this.room.connections.forEach(con => {
            con.ev.send(con.ws, 'roomIsReady', con.player);
        })





    }


    disconnected(ws) {
        if (this.room != undefined) {
            this.room.connections.forEach(con => {




                if (con.ws == ws) {
                    console.log('the player with ID ' + this.player.id + " has been disconnected")
                    this.room.connections.splice(this.room.connections.indexOf(this), 1);
                    this.ev.lobbyBroadcast(ws, this.room.connections, "disconnected", this.player);
                    if(this.room.connections.length == 0){
                        console.log('destroying the room...')
                        delete this.lobby.rooms[this.room.id];
                        console.log('room destroyed')
                    }
                    //console.log(this.room.connections.length)

                }
            })
        }

    }


}