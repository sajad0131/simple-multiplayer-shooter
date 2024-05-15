const Vector3 = require("./Vector3");

module.exports = class GameSettings{
    constructor(){
        this.respawnTime = 5;
        this.respawnPoint1 = new Vector3(2,2,2);
        this.respawnPoint2 = new Vector3(2,2,5);
    }
}