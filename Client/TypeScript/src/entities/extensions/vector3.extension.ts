import { Vector3 } from "alt-client"
import { getRandom } from "../../datas/utils";

declare module "alt-client" {
    interface Vector3 {
        added(other: Vector3 | number): Vector3;
        substracted(other: Vector3 | number): Vector3;
        multiplied(other: Vector3 | number): Vector3;
        divided(other: Vector3 | number): Vector3;

        normalized(): Vector3;
        distanceTo(other: Vector3): number;
        distanceToSquared(other: Vector3): number;
        distanceTo2D(other: Vector3): number;
        distanceToSquared2D(other: Vector3): number;
        length(): number;
        lengthSquared(): number;
        addedToZ(z: number): Vector3;
        around(distance: number, considerZ?: boolean): Vector3;
    }
}

Vector3.prototype.added = function (other: Vector3 | number) {
    const me = (this as Vector3);
    if (typeof other === "number") {
        other = new Vector3(other, other, other);
    }
    return new Vector3(me.x + other.x, me.y + other.y, me.z + other.z);
}

Vector3.prototype.substracted = function (other: Vector3 | number) {
    const me = (this as Vector3);
    if (typeof other === "number") {
        other = new Vector3(other, other, other);
    }
    return new Vector3(me.x - other.x, me.y - other.y, me.z - other.z);
}

Vector3.prototype.multiplied = function (other: Vector3 | number) {
    const me = (this as Vector3);
    if (typeof other === "number") {
        other = new Vector3(other, other, other);
    }
    return new Vector3(me.x * other.x, me.y * other.y, me.z * other.z);
}

Vector3.prototype.divided = function (other: Vector3 | number) {
    const me = (this as Vector3);
    if (typeof other === "number") {
        other = new Vector3(other, other, other);
    }
    return new Vector3(me.x / other.x, me.y / other.y, me.z / other.z);
}



Vector3.prototype.normalized = function () {
    const me = this as Vector3;
    return me.divided(me.length());
}

Vector3.prototype.distanceTo = function (other: Vector3) {
    if (!other) return 0;

    return Math.sqrt((this as Vector3).distanceToSquared(other));
}

Vector3.prototype.distanceToSquared = function (other: Vector3) {
    const me = this as Vector3;

    var nX = me.x - other.x;
    var nY = me.y - other.y;
    var nZ = me.z - other.z;

    return nX * nX + nY * nY + nZ * nZ;
}

Vector3.prototype.distanceTo2D = function (other: Vector3) {
    if (!other) return 0;

    return Math.sqrt((this as Vector3).distanceToSquared2D(other));
}

Vector3.prototype.distanceToSquared2D = function (other: Vector3) {
    const me = this as Vector3;

    var nX = me.x - other.x;
    var nY = me.y - other.y;

    return nX * nX + nY * nY;
}

Vector3.prototype.length = function () {
    return Math.sqrt((this as Vector3).lengthSquared());
}

Vector3.prototype.lengthSquared = function () {
    const me = this as Vector3;
    return me.x * me.x + me.y * me.y + me.z * me.z;
}

Vector3.prototype.addedToZ = function (z: number) {
    const me = this as Vector3;
    return new Vector3(me.x, me.y, me.z + z); 
}

Vector3.prototype.around = function (distance: number, considerZ: boolean = false) {
    const me = this as Vector3;
    let [x, y, z] = [me.x, me.y, me.z];

    const addToX = Math.randomBetweenIncluded(-distance, distance);
    x += addToX;
    distance -= Math.abs(addToX);

    if (distance == 0)
        return this;

    if (!considerZ) {
        y += getRandom(true, false) ? distance : -distance;
        return this;
    }

    const addToY = Math.randomBetweenIncluded(-distance, distance);
    y += addToY;
    distance -= addToY;

    if (distance == 0)
        return this;

    z += getRandom(true, false) ? distance : -distance;

    return new Vector3(x, y, z);
}


/*

        public static Position3D Lerp(Position3D start, Position3D end, float n)
        {
            return new Position3D()
            {
                X = start.X + (end.X - start.X) * n,
                Y = start.Y + (end.Y - start.Y) * n,
                Z = start.Z + (end.Z - start.Z) * n,
            };
        }


        #endregion Public Methods
    }
}
*/
