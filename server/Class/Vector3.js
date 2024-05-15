module.exports = class Vector3 
{
    constructor(x=0, y=0, z=0) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    Magnitude() {
        return Math.sqrt((this.x * this.x) + (this.y * this.y) + (this.z * this.z));
    }
    Normalize(){
        var magnitude = this.Magnitude();
        return new Vector3(this.x / magnitude, this.y / magnitude,this.z / magnitude);
    }

    Distance(secondVector = Vector3) {
        var direction = new Vector3(secondVector.x -this.x, secondVector.y -this.y, secondVector.z -this.z);
        return direction.Magnitude();
    }

    ToString() {
        return '(' + this.x + ',' + this.y + ',' + this.z + ')';
    }


}