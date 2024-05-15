
require('./GameModes')

module.exports = class EventUpdate {
    constructor() {
        
        this.room;
        this.gameMode;
        this.ev;
        this.player;
        this.ws;
        
    }
    init(gameMode){
        this.gameMode = gameMode;
        
        
        
    }
    eventUpdate( data ) {
        let ws = this.ws;
        let player = this.player;
        let ev = this.ev;
        
        
        ev.on('updatePosition', data, (data) => {
            player.position.x = parseFloat(data.position.x);
            player.position.y = parseFloat(data.position.y);
            player.position.z = parseFloat(data.position.z);
            ev.lobbyBroadcast(ws, this.room.connections, 'updatePosition', player);

        })

        ev.on('updateRotation', data, (data) => {

            player.rotation.x = parseFloat(data.rotation.x);
            player.rotation.y = parseFloat(data.rotation.y);
            player.rotation.z = parseFloat(data.rotation.z);
            console.log(player);
            ev.lobbyBroadcast(ws, this.room.connections, 'updateRotation', player);
        })
        ev.on('updateGunRotation', data, (data => {

            player.gunRotation = parseFloat(data.gunRotation);
            ev.lobbyBroadcast(ws, this.room.connections, 'updateGunRotation', player);
        }))
        ev.on('updateHealth', data, (data => {
            // for (var id in players) {
            //     if (id == data.hitedID) {
            //         players[id].health -= data.gunDamageAmount;
            //         console.log('player ' + players[id].id + " hitted. his health is " + players[id].health)
            //     }
            // }

            this.room.connections.forEach(con => {
                if (con.player.id == data.hitedID) {
                    con.player.health -= data.gunDamageAmount;
                    console.log('player ' + con.player.id + " hitted. his health is " + con.player.health)
                    con.evup.gameMode.UpdateDeadPlayer(con,player);
                    //this.gameMode.killCalculator();

                }
            })

        }))
    }


}