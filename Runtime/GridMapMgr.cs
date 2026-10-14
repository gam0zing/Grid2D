
using Grid2D.Core.Types;

namespace Grid2D.Runtime;

class GridMap {
    public readonly Vec2I Pos;
    private Dictionary<int, Dictionary<Vec2I, Grid>> _grids;

    public GridMap(Vec2I pos) {
        this.Pos = pos;
        this._grids = [];
    }

    public GridMap(Vec2 fPos) : this(fPos.ToVec2I()) { }
    public Grid GetGrid(int layer, Vec2I pos) {
        if (!this._grids.TryGetValue(layer, out var inner)) {
            return Grid.EMPTY;
        }
        return inner.GetValueOrDefault(pos, Grid.EMPTY);
    }
    public Grid GetGrid(int layer, Vec2 fPos) {
        return this.GetGrid(layer, fPos.ToVec2I());
    }
    public void SetGrid(int layer, Vec2 fPos, Grid grid) {
        if (grid.Block == Blocks.AIR) {
            if (this._grids.TryGetValue(layer, out var inner)) {
                inner.Remove(fPos.ToVec2I());
                if (inner.Count == 0) {
                    this._grids.Remove(layer);
                }
            }
            return;
        }
        if (!this._grids.ContainsKey(layer)) {
            this._grids[layer] = [];
        }
        this._grids[layer][fPos.ToVec2I()] = grid;
    }
}

public static class GridMapMgr {
    // TODO 后续换成加载式
    public static readonly Vec2I MAP_SIZE = new(64, 64);
    private static readonly Dictionary<Vec2I, GridMap> _maps = [];

    public static Grid GetGrid(int layer, Vec2 fPos) {
        var gridPos = fPos.ToVec2I();
        var mapPos = ToMapPos(gridPos);
        return GetGridMap(mapPos)?.GetGrid(layer, gridPos) ?? Grid.EMPTY;
    }
    public static void SetGrid(int layer, Vec2 fPos, Grid grid) {
        var gridPos = fPos.ToVec2I();
        var mapPos = ToMapPos(gridPos);
        var map = GetGridMap(mapPos);
        if (map == null) {
            map = new GridMap(mapPos);
            _maps[mapPos] = map;
        }
        map.SetGrid(layer, fPos, grid);
    }
    public static Vec2I ToMapPos(Vec2I pos) {
        int mapX = pos.X - (pos.X % MAP_SIZE.X);
        int mapY = pos.Y - (pos.Y % MAP_SIZE.Y);
        return new Vec2I(mapX, mapY);
    }
    private static GridMap? GetGridMap(Vec2I mapPos) {
        return _maps.GetValueOrDefault(mapPos);
    }
}
