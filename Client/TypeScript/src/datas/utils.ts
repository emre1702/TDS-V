import alt from "alt-client";
import game from "natives";
import Camera from "../entities/cameras/camera.entity";

export function hashPasswordClient(password: string): string {
    return password;
}

export function getRandom<T>(...elements: T[]) {
    const rndIndex = Math.randomIntBetween(0, elements.length);
    return elements[rndIndex];
}

export function getRandomAny(...elements: any[]): any {
    const rndIndex = Math.randomIntBetween(0, elements.length);
    return elements[rndIndex];
}

export function getRGBAFromString(rgba: string): alt.RGBA | undefined {
    if (!rgba || !rgba.length) {
        return undefined;
    }

    alt.on()

    const left = rgba.indexOf("(");
    const right = rgba.indexOf(")");

    if (left < 0 || right < 0) {
        return new alt.RGBA(255, 255, 255, 255);
    }

    const rgbaContent = rgba.substring(left + 1, right);
    let [r, g, b, a] = rgbaContent.split(",");

    return new alt.RGBA(
        Number(r || "255"),
        Number(g || "255"),
        Number(b || "255"),
        Math.min(Number(a || "1") * 255, 255));

}

export function closestDistanceBetweenLines(a0: alt.Vector3, a1: alt.Vector3, b0: alt.Vector3, b1: alt.Vector3)
    : [alt.Vector3, alt.Vector3] {
    const A = a1.substracted(a0);
    const B = b1.substracted(b0);
    const magA = A.length();
    const magB = B.length();

    const _A = A.divided(magA);
    const _B = B.divided(magB);

    const cross = getCrossProduct(_A, _B);
    const denom = cross.length() * cross.length();

    let closest1: alt.Vector3;
    let closest2: alt.Vector3;
    if (denom == 0) {
        const d0 = getDotProduct(_A, b0.substracted(a0));
        const d1 = getDotProduct(_A, b1.substracted(a0));
        if (d0 <= 0 && 0 >= d1) {
            if (Math.abs(d0) < Math.abs(d1)) {
                closest1 = a0;
                closest2 = b0;
                return [closest1, closest2];
            }
            closest1 = a0;
            closest2 = b1;
            return [closest1, closest2];
        }
        else if (d0 >= magA && magA <= d1) {
            if (Math.abs(d0) < Math.abs(d1)) {
                closest1 = a1;
                closest2 = b0;
                return [closest1, closest2];
            }
            closest1 = a1;
            closest2 = b1;
            return [closest1, closest2];
        }

        return [new alt.Vector3(0, 0, 0), new alt.Vector3(0, 0, 0)];
    }

    const t = b0.substracted(a0);
    const detA = determinent(t, _B, cross);
    const detB = determinent(t, _A, cross);

    const t0 = detA / denom;
    const t1 = detB / denom;

    let pA = a0.added(_A.multiplied(t0));
    let pB = b0.added(_B.multiplied(t1));  

    if (t0 < 0)
        pA = a0;
    else if (t0 > magA)
        pA = a1;

    if (t1 < 0)
        pB = b0;
    else if (t1 > magB)
        pB = b1;

    let dot;
    if (t0 < 0 || t0 > magA) {
        dot = getDotProduct(_B, pA.substracted(b0));
        if (dot < 0)
            dot = 0;
        else if (dot > magB)
            dot = magB;
        pB = b0.added(_B.multiplied(dot));
    }

    if (t1 < 0 || t1 > magB) {
        dot = getDotProduct(_A, pB.substracted(a0));
        if (dot < 0)
            dot = 0;
        else if (dot > magA)
            dot = magA;
        pA = a0.added(_A.multiplied(dot));
    }

    closest1 = pA;
    closest2 = pB;
    return [closest1, closest2];
}

export function degreesToRad(deg: number): number {
    return Math.PI * deg / 180.0;
}

export function determinent(a: alt.Vector3, b: alt.Vector3, c: alt.Vector3): number {
    return a.x * b.y * c.z + a.y * b.z * c.x + a.z * b.x * c.y - c.x * b.y * a.z - c.y * b.z * a.x - c.z * b.x * a.y;
}

export function getAngleBetweenVectors(v1: alt.Vector3, v2: alt.Vector3) {
    return Math.acos(getDotProduct(v1.normalized(), v2.normalized()));
}

export function getCrossProduct(left: alt.Vector3, right: alt.Vector3): alt.Vector3
{
    return new alt.Vector3
    (
        left.y * right.z - left.z * right.y,
        left.z * right.x - left.x * right.z,
        left.x * right.y - left.y * right.x
    );
}

export function getDirectionByRotation(rotation: alt.Vector3): alt.Vector3
{
    const num = rotation.z * 0.0174532924;
    const num2 = rotation.x * 0.0174532924;
    const num3 = Math.abs(Math.cos(num2));
    return new alt.Vector3(-Math.sin(num) * num3, Math.cos(num) * num3, Math.sin(num2));
}

export function getDotProduct(v1: alt.Vector3, v2: alt.Vector3): number
{
    return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
}

export function getRandomColor(): alt.RGBA {
    return new alt.RGBA(
        Math.randomIntBetweenIncluded(0, 255),
        Math.randomIntBetweenIncluded(0, 255),
        Math.randomIntBetweenIncluded(0, 255),
        Math.randomIntBetweenIncluded(0, 255));
}

export function getScreenCoordFromWorldCoord(vec: alt.Vector3): alt.Vector2 | undefined {
    const [worked, x, y] = game.getScreenCoordFromWorldCoord(vec.x, vec.y, vec.z, 0, 0);
    if (worked) {
        return { x, y };
    } else {
        return undefined;
    }
}

export function getWorldCoordFromScreenCoord(pos: alt.Vector2, camera: Camera): alt.Vector3 {
    const camPos = (camera && camera.position) || game.getGameplayCamCoord() as alt.Vector3;
    const camRot = (camera && camera.rotation) || game.getGameplayCamRot(2) as alt.Vector3;
    const camForward = rotationToDirection(camRot);
    const rotUp = camRot.added(new alt.Vector3(1, 0, 0));
    const rotDown = camRot.added(new alt.Vector3(-1, 0, 0));
    const rotLeft = camRot.added(new alt.Vector3(0, 0, -1));
    const rotRight = camRot.added(new alt.Vector3(0, 0, 1));

    const camRight = rotationToDirection(rotRight).substracted(rotationToDirection(rotLeft));
    const camUp = rotationToDirection(rotUp).substracted(rotationToDirection(rotDown));

    const rollRad = -degreesToRad(camRot.y);

    const camRightRoll = camRight.multiplied(Math.cos(rollRad)).substracted(camUp.multiplied(Math.sin(rollRad)));
    const camUpRoll = camRight.multiplied(Math.sin(rollRad)).added(camUp.multiplied(Math.cos(rollRad)));

    const point3D = camPos.added(camForward.added(camRightRoll).added(camUpRoll));
    const point3DZero = camPos.added(camForward);

    const [point2DWorked, point2DX, point2DY] = game.getScreenCoordFromWorldCoord(point3D.x, point3D.y, point3D.z, 0, 0);
    if (!point2DWorked) {
        return point3DZero;
    }

    const [point2DZeroWorked, point2DZeroX, point2DZeroY] = game.getScreenCoordFromWorldCoord(point3DZero.x, point3DZero.y, point3DZero.z, 0, 0);
    if (!point2DZeroWorked) {
        return point3DZero;
    }

    const eps = 0.001;
    if (Math.abs(point2DX - point2DZeroX) < eps || Math.abs(point2DY - point2DZeroY) < eps) {
        return point3DZero;
    }
    const scaleX = (pos.x - point2DZeroX) / (point2DX - point2DZeroX);
    const scaleY = (pos.y - point2DZeroY) / (point2DY - point2DZeroY);
    const point3Dret = camPos.added(camForward).added(camRightRoll.multiplied(scaleX)).added(camUpRoll.multiplied(scaleY));
    //forwardDirection = camForward + camRightRoll * scaleX + camUpRoll * scaleY;
    return point3Dret;
}

export function rotationToDirection(rotation: alt.Vector3): alt.Vector3 {
    const z = this.degreesToRad(rotation.z);
    const x = this.degreesToRad(rotation.x);
    const num = Math.abs(Math.cos(x));
    return new alt.Vector3(-Math.sin(z) * num, Math.cos(z) * num, Math.sin(x));
}

/*


        public float GetCursorX()
{
    return ModAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.CursorX);
}


public float GetCursorY()
{
    return ModAPI.Control.GetDisabledControlNormal(InputGroup.MOVE, Control.CursorY);
}



public Position GetWorldCoordFromScreenCoord(float x, float y, TDSCamera tdsCamera = null)
{
    
}

        public bool LineIntersectingCircle(Position CircleCenter, Position CircleRotation, float CircleRadius, Position LineStart, Position LineEnd, ref Position HitPosition, float threshold, ref Position planeNorm)
{
    Position v2 = new Position(CircleCenter.X, CircleCenter.Y, CircleCenter.Z + CircleRadius);
    Position v3 = new Position(CircleCenter.X - CircleRadius, CircleCenter.Y, CircleCenter.Z);

    v2 -= CircleCenter;
    v2 = RotateZ(v2, CircleRotation.Z);
    v2 += CircleCenter;

    v3 -= CircleCenter;
    v3 = RotateZ(v3, CircleRotation.Z);
    v3 += CircleCenter;

    v2 -= CircleCenter;
    v2 = RotateX(v2, CircleRotation.X);
    v2 += CircleCenter;

    v3 -= CircleCenter;
    v3 = RotateX(v3, CircleRotation.X);
    v3 += CircleCenter;

    v2 -= CircleCenter;
    v2 = RotateY(v2, CircleRotation.Y);
    v2 += CircleCenter;

    v3 -= CircleCenter;
    v3 = RotateY(v3, CircleRotation.Y);
    v3 += CircleCenter;

    //RAGE.Game.Graphics.DrawPoly(CircleCenter.X, CircleCenter.Y, CircleCenter.Z, v2.X, v2.Y, v2.Z, v3.X, v3.Y, v3.Z, 0, 255, 0, 255);

    Position four = v2 - CircleCenter;
    Position five = v3 - CircleCenter;

    Position cross = GetCrossProduct(four, five);
    planeNorm = new Position(cross.X, cross.Y, cross.Z);
    cross.Normalize();
    bool hit = LineIntersectingPlane(cross, CircleCenter, LineStart, LineEnd, ref HitPosition);
    if (hit) {
        if (HitPosition.DistanceTo(CircleCenter) <= CircleRadius + threshold) {
            return true;
        }
    }
    return false;
}

        public bool LineIntersectingPlane(Position PlaneNorm, Position PlanePoint, Position LineStart, Position LineEnd, ref Position HitPosition)
{
    Position u = LineEnd - LineStart;
    float dot = GetDotProduct(PlaneNorm, u);
    if (MathF.Abs(dot) > float.Epsilon) {
        Position w = LineStart - PlanePoint;
        float fac = -GetDotProduct(PlaneNorm, w) / dot;
        u *= fac;
        HitPosition = LineStart + u;
        return true;
    }
    return false;
}

        public bool LineIntersectingSphere(Position StartLine, Position LineEnd, Position SphereCenter, float SphereRadius)
{
    Position d = LineEnd - StartLine;
    Position f = StartLine - SphereCenter;

    float c = GetDotProduct(f, f) - SphereRadius * SphereRadius;
    if (c <= 0f)
    return true;

    float b = GetDotProduct(f, d);
    if (b >= 0f)
    return false;

    float a = GetDotProduct(d, d);
    if (b * b - a * c < 0f)
    return false;

    return true;
}

        public void Notify(string msg)
{
    ModAPI.Ui.SetNotificationTextEntry("STRING");
    ModAPI.Ui.AddTextComponentSubstringPlayerName(msg);
    ModAPI.Ui.DrawNotification(false);
}

        public float RadToDegrees(float rad)
{
    return rad * (180f / MathF.PI);
}

        public RaycastHit RaycastFromTo(Position from, Position to, int ignoreEntity, int flags)
{
    int ray = ModAPI.Shapetest.StartShapeTestRay(from.X, from.Y, from.Z, to.X, to.Y, to.Z, flags, ignoreEntity, 0);
    RaycastHit cast = new RaycastHit();
    int curtemp = 0;
    cast.ShapeResult = ModAPI.Shapetest.GetShapeTestResult(ray, ref curtemp, cast.EndCoords, cast.SurfaceNormal, ref cast.EntityHit);
    cast.Hit = Convert.ToBoolean(curtemp);
    return cast;
}

        public Position RotateX(Position point, float angle)
{
    Position f1 = new Position(1, 0, 0);
    Position f2 = new Position(0, MathF.Cos(DegreesToRad(angle)), -MathF.Sin(DegreesToRad(angle)));
    Position f3 = new Position(0, MathF.Sin(DegreesToRad(angle)), MathF.Cos(DegreesToRad(angle)));

    Position final = new Position
    {
        X = (f1.X * point.X + f1.Y * point.Y + f1.Z * point.Z),
            Y = (f2.X * point.X + f2.Y * point.Y + f2.Z * point.Z),
            Z = (f3.X * point.X + f3.Y * point.Y + f3.Z * point.Z)
    };

    return final;
}

        public Position RotateY(Position point, float angle)
{
    Position f4 = new Position(MathF.Cos(DegreesToRad(angle)), 0, MathF.Sin(DegreesToRad(angle)));
    Position f5 = new Position(0, 1, 0);
    Position f6 = new Position(-MathF.Sin(DegreesToRad(angle)), 0, MathF.Cos(DegreesToRad(angle)));

    Position final = new Position
    {
        X = (f4.X * point.X + f4.Y * point.Y + f4.Z * point.Z),
            Y = (f5.X * point.X + f5.Y * point.Y + f5.Z * point.Z),
            Z = (f6.X * point.X + f6.Y * point.Y + f6.Z * point.Z)
    };
    return final;
}

        public Position RotateZ(Position point, float angle)
{
    Position f7 = new Position(MathF.Cos(DegreesToRad(angle)), -MathF.Sin(DegreesToRad(angle)), 0);
    Position f8 = new Position(MathF.Sin(DegreesToRad(angle)), MathF.Cos(DegreesToRad(angle)), 0);
    Position f9 = new Position(0, 0, 1);

    Position final = new Position
    {
        X = (f7.X * point.X + f7.Y * point.Y + f7.Z * point.Z),
            Y = (f8.X * point.X + f8.Y * point.Y + f8.Z * point.Z),
            Z = (f9.X * point.X + f9.Y * point.Y + f9.Z * point.Z)
    };
    return final;
}



        #endregion Public Methods

        #region Private Methods

        private void DisableControlActions(int _)
{
    ModAPI.Control.DisableControlAction(InputGroup.WHEEL, Control.EnterCheatCode);

    if (ModAPI.LocalPlayer.IsArmed(ArmedType.AllExceptMelee)) {
        ModAPI.Control.DisableControlAction(InputGroup.MOVE, Control.MeleeAttackLight);
        ModAPI.Control.DisableControlAction(InputGroup.MOVE, Control.MeleeAttackHeavy);
        ModAPI.Control.DisableControlAction(InputGroup.MOVE, Control.MeleeAttackAlternate);
    }
}

        private void EventsHandler_LoggedIn()
{
    ModAPI.Event.Tick.Add(new EventMethodData<TickDelegate>(HideHudOriginalComponents));
}

        private void HideHudOriginalComponents(int _)
{
    ModAPI.Ui.HideHudComponentThisFrame(HudComponent.HUD_CASH);
    ModAPI.Ui.HideHudComponentThisFrame(HudComponent.HUD_WEAPON_WHEEL_STATS);
    ModAPI.Ui.HideHudComponentThisFrame(HudComponent.HUD_WEAPON_ICON);
    ModAPI.Ui.HideHudComponentThisFrame(HudComponent.HUD_CASH_CHANGE);
    ModAPI.Ui.HideHudComponentThisFrame(HudComponent.HUD_MP_CASH);
}
*/
