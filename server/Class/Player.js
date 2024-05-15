var ShortID = require('shortid');
var Vector3 = require('./Vector3');
module.exports = class Player 
{
    constructor()
    {
        this.username = 'player';
        this.id = ShortID.generate();
        this.position = new Vector3();
        this.rotation = new Vector3();
        this.gunRotation = 0;
        this.health = new Number(100);
        this.team;
        this.mode;
        this.map;
        this.killNumber =0;
        
    }

    
}