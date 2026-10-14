namespace Grid2D.Core;

public readonly record struct Identifier(
    string Group,
    string Type,
    string Name
) {
    public override string ToString() {
        return $"{this.Group}:{this.Type}:{this.Name}";
    }
    public static Identifier Parse(string identifier) {
        var parts = identifier.Split(':');
        if (parts.Length != 3) {
            throw new ArgumentException($"Invalid identifier format: {identifier}");
        }
        return new Identifier(parts[0], parts[1], parts[2]);
    }
}
