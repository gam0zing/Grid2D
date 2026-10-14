using Grid2D;
using Grid2D.Core;
using Grid2D.Runtime;

public class Program {
    public static readonly Registry<Block> BLOCKS = RegistryCenter.Register<Block>(MainGlobal.GROUP);

    public static readonly RegisterEntry<BaseBlock> BASE_BLOCK = BLOCKS.Register(MainGlobal.GROUP, "base_block", () => new BaseBlock());

    public static void Main(string[] args) {
    }
}