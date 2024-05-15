let Connection = require('./Connection')
let GameLoobby = require('./GameLobby');
module.exports = class Server {

    constructor() {
        this.rooms = {};
        this.roomIndex = 0;
        this.gameLobby = new GameLoobby();
        

    }
    
    Connected(connection,ws, player) {
        
        
        connection.ws = ws;

        connection.player = player;
        connection.Connected(ws,player);
        connection.lobby = this.gameLobby;
        console.log(this.rooms);
        connection.server = this;
        this.gameLobby.server = this;
        this.gameLobby.roomIndex = this.roomIndex;
        this.gameLobby.connection = connection;

        
        connection.messageReciver(connection);
        

    }

    


    // onUpdate() {
    //     let connection = this.connection;
    //     connection.EventUpdate();
    // }


}