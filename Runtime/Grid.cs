using Grid2D.Core.Types;

namespace Grid2D.Runtime;

public readonly struct Grid {
    public static readonly Grid EMPTY = new(new Vec2I(0, 0), Blocks.AIR, 0);

    public readonly Vec2I Pos;
    public readonly Block Block;
    public readonly int Layer;

    public Grid(Vec2I pos, Block block, int layer) {
        this.Pos = pos;
        this.Block = block;
        this.Layer = layer;
        this.Init();
    }

    private void Init() {
        // TODO 此处执行瓦片自己和 Block 的初始化逻辑
    }

    // TODO 后续暴露更多公共方法帮助 Block 和其他模块操作瓦片
}
