using Grid2D.Core.Register;

namespace Grid2D.Runtime.World.Displayable.Model.Define;

public class Block : IRegistryItem<Block> {
    public static readonly Registry<Block> REGISTRY = RegistryCenter.Register<Block>(MainGlobal.GROUP);
    public static string Type => RegisterTypes.BLOCK;

    public readonly Identifier Identifier;
    protected Block(Identifier identifier) {
        this.Identifier = identifier;
    }
    // TODO 后续此处的返回值可以换成 builder
    public static Block Register(string group, string name) {
        Block block = new(IRegistryItem<Block>.GetIdentifier(group, name));
        REGISTRY.Register(group, name, () => block);
        return block;
    }
}
