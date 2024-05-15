let Room = require('./Room');
const Vector3 = require("./Vector3");
let Event = require('../custom/event')
module.exports = class TeamDeathMatch 
{
    constructor(map="",room) {
        this.map = map;
        this.room = room;
        this.respawnTime = 5;
        this.respawnPoint1 = new Vector3(2,2,2);
        this.respawnPoint2 = new Vector3(2,2,5);
        this.teamAPoint = 0;
        this.teamBPoint = 0;
        this.AConnections = new Array();
        this.BConnections = new Array();
        
        
        
    }

    initiate(){
        let room = this.room;
        console.log("setting up the game rules for teamDeathMatch mode !!!!")
        // seprate players in two groups
        for (let index = 0; index < room.connections.length; index++) {
            const con = room.connections[index];
            if(index % 2 == 0){
                con.player.team = "A";
                this.AConnections.push(con);
                console.log("the player with ID: " + con.player.id + " is in team: " + con.player.team);
            }else{
                
                con.player.team = "B";
                this.BConnections.push(con);
                console.log("the player with ID: " + con.player.id + " is in team: " + con.player.team);
            }
            
        }     // !! end of grouping players !!
    }

    pointCalculator(){
        console.log("point calculated")
    }

    UpdateDeadPlayer(connection,Killer) {
        let player = connection.player;
        
        let room = connection.room;
        let ev = new Event();
        console.log("hi");
        if (player.health <= 0) {
            Killer.killNumber++;
            if(Killer.team == "A"){
                this.teamAPoint++;
                let data = {
                    killer: Killer,
                    died: player,
                    Apoint: this.teamAPoint
                }
                ev.lobbyBroadcast(connection.ws, room.connections, 'teamApoint', data);
                ev.send(connection.ws, 'teamApoint', data);
                
            }
            if(Killer.team == "B"){
                this.teamBPoint++;
                let data = {
                    killer: Killer,
                    died: player,
                    Bpoint: this.teamBPoint
                }
                ev.lobbyBroadcast(connection.ws, room.connections, 'teamBpoint', data);
                ev.send(connection.ws, 'teamBpoint', data);
            }
            ev.lobbyBroadcast(connection.ws, room.connections, 'playerDied', player);
            ev.send(connection.ws, 'playerDied', player);
            setTimeout(() => {

                let data = {
                    "player": player,
                    "point1": this.respawnPoint1,
                    "point2": this.respawnPoint2
                }
                player.health = 100;
                ev.lobbyBroadcast(connection.ws, room.connections, "updateDeadPlayer", data);
                ev.send(connection.ws, "updateDeadPlayer", data);


            }, 5000);

        }
    }


}