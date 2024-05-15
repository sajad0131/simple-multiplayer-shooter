let Room = require('./Room');
let TeamDeathMatch = require('./TeamDeathMatch');
module.exports = class GameModes 
{
    constructor(map="", mode="") {
        this.map = map;
        this.mode = mode;

        this.teamDeathMatch;

        
        
    }

    SetupGameMode(room){
        
        if(this.mode == "teamDeathMatch"){
            room.maxPlayer = 2;
        }
        if(this.mode == "captureFlag"){
            console.log("game mode is capture the flag")
        }
    }
    StartGameMode(room){
        
        if(this.mode == "teamDeathMatch"){
            let teamDeathMatch = new TeamDeathMatch(this.map,room);
            room.connections.forEach(con => {
                con.evup.gameMode.teamDeathMatch = teamDeathMatch;
            });
            //this.teamDeathMatch = teamDeathMatch;
            teamDeathMatch.initiate(room);
        }
        if(this.mode == "captureFlag"){
            console.log("game mode is capture the flag")
        }
    }

    

    UpdateDeadPlayer(connection,killer){
        if(this.mode == "teamDeathMatch"){
            connection.evup.gameMode.teamDeathMatch.UpdateDeadPlayer(connection,killer);
            
        }
    }
    

    

}