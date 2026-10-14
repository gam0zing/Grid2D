namespace Grid2D.Core.Types;

public record struct Vec2(float X, float Y) {
    public readonly Vec2I ToVec2I() {
        return new Vec2I((int)this.X, (int)this.Y);
    }
}
public record struct Vec2I(int X, int Y) {
    public readonly Vec2 ToVec2() {
        return new Vec2(this.X, this.Y);
    }
}
