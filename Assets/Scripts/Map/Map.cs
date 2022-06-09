using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    private TerrainType[] map;

    private int _columns = 100;
    private int _rows = 100;
    [SerializeField] private float _size = 0.5f;
    [SerializeField] private Vector3 _origin = new Vector3(0, 0, 0);
    [SerializeField] private float _width;
    [SerializeField] private float _height;

    public int Columns { get => _columns;}
    public int Rows { get => _rows;}
    public float Size { get => _size;}
    public Vector3 Origin { get => _origin;}
    public float Width { get => _width;}
    public float Height { get => _height;}

    void DrawMap() {
        for(int y = 0; y < Rows; y++) {
            for(int x = 0; x < Columns; x++) {
                Vector3 cell = Map2World(new Vector2Int(x, y));
                Color color = Color.green;
                if (!CanPass(new Vector2Int(x, y))) color = Color.red;
                Debug.DrawLine(cell, cell + Vector3.up, color);
            }
        }
    }

    // Calcula si una celda en concreto está bloqueada
    TerrainType GetTileType(Vector2Int tile) {
        Vector3 center = Map2World(tile) + Vector3.one * 0.5f * Size;
        center.y = 0.31f;
        
        if (Physics.CheckBox(center, new Vector3(Size * 0.5f, 0.3f, Size *0.5f), Quaternion.identity, 1 << 10)) {
            return TerrainType.Obstaculo;
        } else {
            RaycastHit hitInfo;
            bool hit = Physics.Raycast(center, Vector3.down, out hitInfo, 100, 1 << 11);
            if (!hit) return TerrainType.Obstaculo;
            return hitInfo.collider.GetComponent<Terrain>().Tipo;
        }
    }

    void GenerateMap() {
        _columns = Mathf.FloorToInt(Width / Size) + 1;
        _rows = Mathf.FloorToInt(Height / Size) + 1;
        map = new TerrainType[Columns * Rows];
        for(int y = 0; y < Rows; y++) {
            for(int x = 0; x < Columns; x++) {
                map[y * Columns + x] = GetTileType(new Vector2Int(x, y));
            }
        }
    }

    void Awake() {
        GenerateMap();
        Global.Map = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Global.Debug) DrawMap();
    }

    public Vector3 Map2World(Vector2Int coords) {
        return Origin + new Vector3(coords.x, 0, coords.y) * Size;
    }

    public Vector2Int World2Map(Vector3 position) {
        int x = (int) ((position.x - Origin.x) /  Size);
        int y = (int) ((position.z - Origin.z) /  Size);

        return new Vector2Int(x, y);
    }

    public bool CanPass(Vector2Int tile) {
        if (IsOutOfBounds(tile)) return false;
        return map[tile.y * Columns + tile.x] != TerrainType.Obstaculo;
    }

    public bool IsOutOfBounds(Vector2Int tile) {
        if (tile.x < 0 || tile.y < 0 || tile.x >= Columns || tile.y >= Rows) return true;
        return false;
    }
}
