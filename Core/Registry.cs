namespace Grid2D.Core;

public record RegisterEntry<TItem>(Identifier Identifier, Func<TItem> Factory) { }

public interface IRegistry {
    Identifier Identifier { get; }
}

public class Registry<TItem> : IRegistry where TItem : IRegistryItem<TItem> {
    public Identifier Identifier { get; private set; }
    private readonly Dictionary<Identifier, Func<TItem>> _items;
    public IReadOnlyDictionary<Identifier, Func<TItem>> Items => this._items;

    public Registry(string group) {
        this.Identifier = IRegistryItem<TItem>.GetIdentifier(group, "register");
        this._items = [];
    }
    public RegisterEntry<TItem> Register(string group, string name, Func<TItem> factory) {
        Identifier identifier = IRegistryItem<TItem>.GetIdentifier(group, name);
        if (this._items.ContainsKey(identifier)) {
            throw new ArgumentException($"Identifier {identifier} is already registered");
        }
        this._items[identifier] = factory;
        return new RegisterEntry<TItem>(identifier, factory);
    }
    public RegisterEntry<TItemChild> Register<TItemChild>(string group, string name, Func<TItemChild> factory) where TItemChild : class, TItem {
        Identifier identifier = IRegistryItem<TItem>.GetIdentifier(group, name);
        if (this._items.ContainsKey(identifier)) {
            throw new ArgumentException($"Identifier {identifier} is already registered");
        }
        this._items[identifier] = factory;
        return new RegisterEntry<TItemChild>(identifier, factory);
    }
    public Func<TItem>? Get(Identifier identifier) {
        return this._items.GetValueOrDefault(identifier);
    }
    public Func<TItem> GetOrDefault(Identifier identifier, Func<TItem> defaultFactory) {
        return this._items.GetValueOrDefault(identifier, defaultFactory);
    }
    public TItem? Instance(Identifier identifier) {
        var factory = this.Get(identifier);
        if (factory == null) {
            return default;
        }
        return factory();
    }
    public TItem InstanceOrDefault(Identifier identifier, TItem defaultValue) {
        var factory = this.Get(identifier);
        if (factory == null) {
            return defaultValue;
        }
        return factory();
    }
    public bool Contains(Identifier identifier) {
        return this._items.ContainsKey(identifier);
    }
}
