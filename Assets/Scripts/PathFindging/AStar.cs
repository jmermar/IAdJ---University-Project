using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private enum Direction {
        Left, Right, Up, Down
    }

    private Map map;
    private Vector3 start, end;
    private Vector2Int startCell, endCell;
    private Vector3[] path;

    private Vector2Int GetNextTile(Vector2Int tile, Direction move) {
        if (move == Direction.Left) return tile + Vector2Int.left;
        if (move == Direction.Right) return tile + Vector2Int.right;
        if (move == Direction.Up) return tile + Vector2Int.up;
        if (move == Direction.Down) return tile + Vector2Int.down;

        return Vector2Int.zero;
    }

    private float CostMove(Vector2Int tile, Direction move) {
        return map.CanPass(GetNextTile(tile, move)) ? 1 : float.PositiveInfinity;
    }

    private float Heuristic(Vector2Int tile) {
        return (endCell - tile).magnitude;
    }

    private Vector3[] ReconstructPath(int[] path) {
        int i = endCell.y * map.Columns + endCell.x;
        
        List<Vector3> reconstructed = new List<Vector3>();
        if (path[i] != -1) {
            reconstructed.Add(end);
            Vector2Int tile = new Vector2Int(i % map.Columns, i / map.Columns);
            i = path[i];
            while(path[i] != -1) {
                tile = new Vector2Int(i % map.Columns, i / map.Columns);
                reconstructed.Add(map.Map2World(tile));
                i = path[i];
            }
        }

        reconstructed.Reverse();

        return reconstructed.ToArray();
    }

    private AStar(Vector3 start, Vector3 end, Map map) {
        this.start = start;
        this.end = end;
        startCell = map.World2Map(start);
        endCell = map.World2Map(end);
        this.map = map;

        if (map.IsOutOfBounds(startCell) || map.IsOutOfBounds(endCell)) return;

        //Debug.Log(origin.x + "x" + origin.y);

        // Heurística de cada nodo
        float[] h = new float[map.Columns * map.Rows];
        for(int y = 0; y < map.Rows; y++) {
            for(int x = 0; x < map.Columns; x++) {
                h[y * map.Columns + x] = Heuristic(new Vector2Int(x, y));
            }
        }

        // Coste a cada nodo
        float[] g = new float[map.Columns * map.Rows];
        for(int y = 0; y < map.Rows; y++) {
            for(int x = 0; x < map.Columns; x++) {
                g[y * map.Columns + x] = float.PositiveInfinity;
            }
        }
        g[startCell.y * map.Columns + startCell.x] = 0;

        // Coste estimado a la solución pasando por un nodo
        float[] f = new float[map.Columns * map.Rows];
        for(int y = 0; y < map.Rows; y++) {
            for(int x = 0; x < map.Columns; x++) {
                f[y * map.Columns + x] = float.PositiveInfinity;
            }
        }
        f[startCell.y * map.Columns + startCell.x] = h[startCell.y * map.Columns + startCell.x];

        // Camino óptimo hasta cada nodo
        int[] path = new int[map.Columns * map.Rows];
        for(int i = 0; i < map.Columns * map.Rows; i++) path[i] = -1;

        HashSet<int> open = new HashSet<int>();
        open.Add(startCell.y * map.Columns + startCell.x);

        int idGoal = endCell.y * map.Columns + endCell.x;

        Direction[] moves = new Direction[] {Direction.Left, Direction.Right, Direction.Up, Direction.Down};

        while(open.Count > 0) {
            Vector2Int selected = Vector2Int.zero;
            int selId = -1;
            float min = float.PositiveInfinity;
            foreach(int node in open) {
                float fNode = f[node];
                if (fNode <= min) {
                    selected = new Vector2Int(node % map.Columns, node / map.Columns);
                    selId = node;
                    min = fNode;
                }
            }

            if (selId == idGoal) {
                this.path = ReconstructPath(path);
                return;
            }

            open.Remove(selId);

            foreach(Direction move in moves) {
                if (CostMove(selected, move) == float.PositiveInfinity) continue;
                Vector2Int node = GetNextTile(selected, move);
                float newG = g[selected.y * map.Columns + selected.x] + 1;

                if (newG < g[node.y * map.Columns + node.x]) {
                    int nodeId = node.y * map.Columns + node.x;
                    g[nodeId] = newG;
                    f[nodeId] = newG + h[nodeId];
                    path[nodeId] = selId;
                    open.Add(nodeId);
                }
            }
            
        }

    }

    public static Vector3[] GetPath(Vector3 start, Vector3 end, Map map) {
        AStar instance = new AStar(start, end, map);

        return instance.path;
    }
}
