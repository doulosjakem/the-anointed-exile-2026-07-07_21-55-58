using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum TerrainType
{
    Sand,
    Rock,
    Grass
}

[Serializable]
public struct HexCoord
{
    public int q; // column
    public int r; // row

    public HexCoord(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    // Axial coordinate distance (hex grid)
    public int DistanceTo(HexCoord other)
    {
        int dq = Math.Abs(q - other.q);
        int dr = Math.Abs(r - other.r);
        return Math.Max(Math.Max(dq, dr), Math.Abs(dq - dr));
    }

    public static HexCoord operator +(HexCoord a, HexCoord b)
    {
        return new HexCoord(a.q + b.q, a.r + b.r);
    }

    public static HexCoord operator -(HexCoord a, HexCoord b)
    {
        return new HexCoord(a.q - b.q, a.r - b.r);
    }

    public static bool operator ==(HexCoord a, HexCoord b)
    {
        return a.q == b.q && a.r == b.r;
    }

    public static bool operator !=(HexCoord a, HexCoord b)
    {
        return !(a == b);
    }

    public override bool Equals(object obj)
    {
        if (obj is HexCoord other)
            return this == other;
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(q, r);
    }

    public override string ToString()
    {
        return $"({q}, {r})";
    }

    // Returns the 6 neighbors of this hex in clockwise order
    public List<HexCoord> GetNeighbors()
    {
        return new List<HexCoord>
        {
            new HexCoord(q + 1, r),     // E
            new HexCoord(q + 1, r - 1), // NE
            new HexCoord(q, r - 1),     // NW
            new HexCoord(q - 1, r),     // W
            new HexCoord(q - 1, r + 1), // SW
            new HexCoord(q, r + 1)      // SE
        };
    }
}

public class HexGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int width = 8;
    [SerializeField] private int height = 8;
    [SerializeField] private float hexSize = 1.0f;

    [Header("Tile Colors")]
    [SerializeField] private Color sandColor = new Color(0.85f, 0.75f, 0.55f);
    [SerializeField] private Color rockColor = new Color(0.60f, 0.55f, 0.50f);
    [SerializeField] private Color grassColor = new Color(0.65f, 0.75f, 0.50f);
    [SerializeField] private Color borderColor = new Color(0.55f, 0.45f, 0.30f);
    [SerializeField] [Range(0f, 0.15f)] private float colorVariation = 0.05f;

    [Header("Occupancy Tracking")]
    [SerializeField] private List<Unit> allUnits = new List<Unit>();

    private Dictionary<HexCoord, GameObject> hexTiles = new Dictionary<HexCoord, GameObject>();
    private Dictionary<HexCoord, Unit> unitPositions = new Dictionary<HexCoord, Unit>();

    public int Width => width;
    public int Height => height;
    public float HexSize => hexSize;
    public IReadOnlyDictionary<HexCoord, GameObject> Tiles => hexTiles;
    public IReadOnlyDictionary<HexCoord, Unit> Units => unitPositions;

    private void Awake()
    {
        GenerateVisualGrid();
    }

    private void GenerateVisualGrid()
    {
        // Clear any existing tiles
        foreach (var tile in hexTiles.Values)
        {
            Destroy(tile);
        }
        hexTiles.Clear();

        for (int r = 0; r < height; r++)
        {
            for (int q = 0; q < width; q++)
            {
                HexCoord coord = new HexCoord(q, r);
                Vector3 worldPos = HexToWorldPosition(coord);

                // Create tile GameObject
                GameObject tile = new GameObject($"HexTile_{q}_{r}");
                tile.transform.SetParent(transform, false);
                tile.transform.position = worldPos;

                // Add the tile generator and configure it
                HexTileGenerator generator = tile.AddComponent<HexTileGenerator>();
                Color tileColor = GetTerrainColor(q, r);
                generator.Configure(hexSize * 0.45f, tileColor, borderColor, colorVariation);

                hexTiles[coord] = tile;
            }
        }
    }

    private Color GetTerrainColor(int q, int r)
    {
        // Simple pseudo-random terrain based on position
        int hash = q * 7 + r * 13;
        int terrainIndex = hash % 3;
        
        switch ((TerrainType)Math.Abs(terrainIndex))
        {
            case TerrainType.Sand:
                return sandColor;
            case TerrainType.Rock:
                return rockColor;
            case TerrainType.Grass:
                return grassColor;
            default:
                return sandColor;
        }
    }

    public bool IsValidCoord(HexCoord coord)
    {
        return coord.q >= 0 && coord.q < width &&
               coord.r >= 0 && coord.r < height;
    }

    public Vector3 HexToWorldPosition(HexCoord coord)
    {
        // Flat-top hex layout
        float x = hexSize * (3.0f / 2.0f * coord.q);
        float z = hexSize * (Mathf.Sqrt(3.0f) / 2.0f * coord.q + Mathf.Sqrt(3.0f) * coord.r);
        return new Vector3(x, 0f, z);
    }

    public HexCoord WorldToHexPosition(Vector3 worldPos)
    {
        float q = (2.0f / 3.0f * worldPos.x) / hexSize;
        float r = (-1.0f / 3.0f * worldPos.x + Mathf.Sqrt(3.0f) / 3.0f * worldPos.z) / hexSize;

        // Round to nearest hex
        return HexRound(new HexCoord(Mathf.RoundToInt(q), Mathf.RoundToInt(r)));
    }

    private HexCoord HexRound(HexCoord raw)
    {
        int rq = raw.q;
        int rr = raw.r;
        int rs = -rq - rr;

        int rqRound = Mathf.RoundToInt(rq);
        int rrRound = Mathf.RoundToInt(rr);
        int rsRound = Mathf.RoundToInt(rs);

        float qDiff = Math.Abs(rqRound - rq);
        float rDiff = Math.Abs(rrRound - rr);
        float sDiff = Math.Abs(rsRound - rs);

        if (qDiff > rDiff && qDiff > sDiff)
            rqRound = -rrRound - rsRound;
        else if (rDiff > sDiff)
            rrRound = -rqRound - rsRound;

        return new HexCoord(rqRound, rrRound);
    }

    public List<HexCoord> GetHexesInRange(HexCoord center, int range)
    {
        List<HexCoord> results = new List<HexCoord>();
        for (int dq = -range; dq <= range; dq++)
        {
            for (int dr = Math.Max(-range, -dq - range); dr <= Math.Min(range, -dq + range); dr++)
            {
                HexCoord coord = new HexCoord(center.q + dq, center.r + dr);
                if (IsValidCoord(coord))
                {
                    results.Add(coord);
                }
            }
        }
        return results;
    }

    public List<HexCoord> GetReachableHexes(HexCoord from, int moveRange)
    {
        // BFS-based reachable hexes within moveRange, avoiding occupied tiles
        List<HexCoord> reachable = new List<HexCoord>();
        HashSet<HexCoord> visited = new HashSet<HexCoord>();
        Queue<(HexCoord coord, int distance)> queue = new Queue<(HexCoord, int)>();
        
        queue.Enqueue((from, 0));
        visited.Add(from);

        while (queue.Count > 0)
        {
            var (current, dist) = queue.Dequeue();

            if (dist > 0 && dist <= moveRange)
            {
                // Only add if not occupied (except the starting position)
                if (!unitPositions.ContainsKey(current) || current == from)
                {
                    reachable.Add(current);
                }
            }

            if (dist >= moveRange) continue;

            foreach (HexCoord neighbor in current.GetNeighbors())
            {
                if (!IsValidCoord(neighbor)) continue;
                if (visited.Contains(neighbor)) continue;
                
                // Can't pass through occupied tiles
                if (unitPositions.ContainsKey(neighbor)) continue;

                visited.Add(neighbor);
                queue.Enqueue((neighbor, dist + 1));
            }
        }

        return reachable;
    }

    public void PlaceUnit(Unit unit, HexCoord coord)
    {
        if (!IsValidCoord(coord)) return;

        // Remove from old position if any
        if (unitPositions.ContainsValue(unit))
        {
            foreach (var kvp in unitPositions)
            {
                if (kvp.Value == unit)
                {
                    unitPositions.Remove(kvp.Key);
                    break;
                }
            }
        }

        unit.GridPosition = coord;
        unitPositions[coord] = unit;

        if (!allUnits.Contains(unit))
        {
            allUnits.Add(unit);
        }

        // Move the unit's GameObject to world position
        unit.transform.position = HexToWorldPosition(coord);
    }

    public void RemoveUnit(Unit unit)
    {
        if (unitPositions.ContainsValue(unit))
        {
            foreach (var kvp in unitPositions)
            {
                if (kvp.Value == unit)
                {
                    unitPositions.Remove(kvp.Key);
                    break;
                }
            }
        }
        allUnits.Remove(unit);
    }

    public Unit GetUnitAt(HexCoord coord)
    {
        if (unitPositions.TryGetValue(coord, out Unit unit))
        {
            return unit;
        }
        return null;
    }

    public bool IsOccupied(HexCoord coord)
    {
        return unitPositions.ContainsKey(coord);
    }
}