let Event = require('../custom/event')
let GameSettings = require('./GameSettings')
let GameModes = require('./GameModes')
let Room = require('./Room');
require('./EventUpdate')

module.exports = class GameLobby {
    constructor() {


        this.deathMatchMaxPlayer = 2;
        this.currentPlayerCount = 0;
        this.connection;
        this.rooms = {};
        this.server;


    }
    OnstartLobby(connection) {

        let map = connection.player.map;
        let mode = connection.player.mode;
        let gameMode = new GameModes();
        gameMode.map = map;
        gameMode.mode = mode;
        connection.evup.gameMode = gameMode;
        if (Object.values(this.rooms).length == 0) {

            let room = new Room();
            console.log("the room created.");
            console.log("mode is: " + mode + " map is: " + map)
            room.mode = mode;
            room.map = map;
            

            room.connections.push(connection);
            gameMode.SetupGameMode(room);
            room.initializer();
            this.rooms[room.id] = room;
            //console.log(this.rooms)
            //console.log(room)
            connection.room = room;
            connection.evup.room = room;
            
            //connection.addedToRoom(connection);
            console.log('added by' + room.connections.length)
        }
        else {
            for (let index = 0; index < Object.values(this.rooms).length; index++) {
                const room = Object.values(this.rooms)[index];
                var isInLobby;




                if (room.map == map && room.mode == mode && room.isFull == false) {


                    room.connections.push(connection);
                    connection.room = room;
                    room.initializer();

                    console.log('added by middle' + room.connections.length)
                    //connection.addedToRoom(connection);
                    //console.log(room)
                    if (room.isFull == true) {
                        console.log("starting game...")
                        //console.log(room)
                        //this.spawnPlayers(room);
                        connection.roomIsReady();

                        room.connections.forEach(con => {
                            con.gameMode = gameMode;
                            con.gameMode.StartGameMode(room);
                        });
                        
                        
                    }




                }
                else if (index == Object.values(this.rooms).length - 1) {
                    let room = new Room();

                    room.mode = mode;
                    room.map = map;
                    room.isFull = false;

                    room.connections.push(connection);
                    room.isFull = false;
                    gameMode.SetupGameMode(room);
                    console.log(room.connections.length)
                    this.rooms[room.id] = room;
                    //console.log(this.rooms)
                    //console.log(room)
                    connection.room = room;
                    //connection.addedToRoom(connection);

                    //console.log(room)
                }



            }
        }
    }


    spawnPlayers(room) {

        let ev = new Event();
        //console.log("the player is " + connection.player.id)
        console.log("spawning...   " + room.connections)



        room.connections.forEach(connection => {

            let ws = connection.ws;

            connection.lobbyStatus = "PlayGame";
            let player = connection.player;
            let thisplayerID = connection.player.id;
            let response = {
                "id": player.id,
                "team": player.team,
                "pos": player.spawnPoint
            }
            ev.send(ws, 'spawn', response); // tell myself I have spawned
            console.log("spawning players");
            ev.lobbyBroadcast(ws, room.connections, 'spawn', response); //tell other players that I have spawned

            let players = connection.players;

            for (var playerID in players) {
                if (playerID != thisplayerID) {
                    console.log("other player is " + players[playerID].id)
                    ev.send(ws, 'spawn', players[playerID]);
                }
            }
        });





    }


    


}