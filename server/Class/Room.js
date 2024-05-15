var ShortID = require('shortid');
module.exports = class Room 
{
    constructor() {
        this.id = ShortID.generate();
        this.mode;
        this.connections = new Array();
        this.map;
        this.isFull = false;
        this.maxPlayer;
        this.currentPlayer;
    }

    // GameSettings(){
    //     if(this.mode == "teamDeathMatch"){
    //         this.maxPlayer = 2;

    //     }
    // }

    initializer(){
        
        if(this.connections.length == this.maxPlayer){
            this.isFull = true;
            console.log("room is full")
        }
        else{
            this.isFull = false;
            console.log("the max player is " + this.maxPlayer + "and there is still " + (this.maxPlayer -  this.connections.length) )
        }
    }

}