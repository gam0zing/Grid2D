using Grid2D.Core;

public interface IRegistryItem<TSelf> where TSelf : IRegistryItem<TSelf> {
    abstract static string Type { get; }
    static Identifier GetIdentifier(string group, string name) {
        return new Identifier(group, TSelf.Type, name);
    }
}
