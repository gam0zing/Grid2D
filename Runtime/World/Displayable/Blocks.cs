using Grid2D.Runtime.World.Displayable.Model.Define;

namespace Grid2D.Runtime.World.Displayable;

public static class Blocks {
    public static readonly Block AIR = Block.Register(MainGlobal.GROUP, "air");
    public static readonly Block ROCK = Block.Register(MainGlobal.GROUP, "rock");
    public static readonly Block DIRT = Block.Register(MainGlobal.GROUP, "dirt");
}
