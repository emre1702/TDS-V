export class Position4D {
  public X: number;
  public Y: number;
  public Z: number;
  public Rotation: number;

  constructor(x: number, y: number, z: number, rot: number) {
    this.X = x;
    this.Y = y;
    this.Z = z;
    this.Rotation = rot;
  }
}
