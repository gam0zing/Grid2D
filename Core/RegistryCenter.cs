using System.Collections.Concurrent;

namespace Grid2D.Core;

public static class RegistryCenter {
    private static readonly ConcurrentDictionary<string, IRegistry> _registries = new();

    /// <summary>
    /// 按 type 获取注册表。
    /// - type 不存在：返回 null。
    /// - type 存在但 TItem 不匹配：抛 InvalidOperationException（调用方 bug）。
    /// </summary>
    public static Registry<TItem>? Get<TItem>() where TItem : IRegistryItem<TItem> {
        string type = TItem.Type;
        if (!_registries.TryGetValue(type, out IRegistry? value)) {
            return default;
        }
        if (value is not Registry<TItem> registry) {
            throw new InvalidOperationException(
                $"Registry class type error! '{type}' exists but is {DescribeRegistry(value)}, " +
                $"not Registry<{typeof(TItem).Name}>");
        }
        return registry;
    }

    /// <summary>
    /// 注册一个新注册表。
    /// - 同 type 同 TItem 重复注册：抛 ArgumentException。
    /// - 同 type 不同 TItem：抛 InvalidOperationException。
    /// </summary>
    public static Registry<TItem> Register<TItem>(string group) where TItem : IRegistryItem<TItem> {
        string type = TItem.Type;
        var registry = new Registry<TItem>(group);
        if (!_registries.TryAdd(type, registry)) {
            if (!_registries.TryGetValue(type, out IRegistry? existing)) {
                // 理论上不可达：TryAdd 失败说明 key 存在。
                throw new InvalidOperationException(
                    $"Registry '{type}' conflict but existing entry could not be retrieved");
            }
            if (existing is Registry<TItem> sameType) {
                throw new ArgumentException(
                    $"Duplicate type registry! type='{type}' already registered by group='{sameType.Identifier.Group}'");
            }
            throw new InvalidOperationException(
                $"Registry type conflict! type='{type}' already registered as {DescribeRegistry(existing)}, " +
                $"cannot register as Registry<{typeof(TItem).Name}>");
        }
        return registry;
    }

    /// <summary>
    /// 原子地“获取或注册”。
    /// - type 不存在：注册并返回新注册表。
    /// - type 存在且 TItem 匹配：返回已有注册表。
    /// - type 存在但 TItem 不匹配：抛 InvalidOperationException。
    /// </summary>
    public static Registry<TItem> GetOrRegister<TItem>(string group) where TItem : IRegistryItem<TItem> {
        string type = TItem.Type;
        if (_registries.TryGetValue(type, out IRegistry? existing)) {
            return CastOrThrow<TItem>(existing);
        }

        var created = new Registry<TItem>(group);
        // GetOrAdd(key, value) 在并发下可能丢弃 created，这是可接受的：
        // Registry 构造无副作用，丢弃的实例会被 GC。
        var result = _registries.GetOrAdd(type, created);
        return CastOrThrow<TItem>(result);
    }

    private static Registry<TItem> CastOrThrow<TItem>(IRegistry value) where TItem : IRegistryItem<TItem> {
        string type = TItem.Type;
        if (value is not Registry<TItem> registry) {
            throw new InvalidOperationException(
                $"Registry class type error! '{type}' exists but is {DescribeRegistry(value)}, " +
                $"not Registry<{typeof(TItem).Name}>");
        }
        return registry;
    }

    private static string DescribeRegistry(IRegistry registry) {
        var actual = registry.GetType().GetGenericArguments().FirstOrDefault()?.Name ?? "unknown";
        return $"Registry<{actual}>";
    }
}