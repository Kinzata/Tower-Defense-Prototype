using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class MapLoader : MonoBehaviour
{
    private Texture2D mapTexture;
    private int width;
    private int height;

    private Color grassColor = Color.green;
    private Color spawnColor = Color.black;
    private Color baseColor = Color.red;
    private Color pathColor = Color.white;

    public GameObject grassTile;
    public GameObject spawnTile;
    public GameObject baseTile;
    public GameObject pathStraightTile;
    public GameObject pathRightTurnTile;
    public GameObject crystalBase;
    public GameObject waveSpawner;

    public string spawnerTag = "SpawnerTile";
    public string pathTag = "PathTile";

    // Start is called before the first frame update
    void Start()
    {
        mapTexture = LoadPNG("D:\\Personal\\Development\\Unity Projects\\Tower-Defense-Prototype\\DevAssets\\Maps\\DemoMap1.png");
        width = mapTexture.width;
        height = mapTexture.height;

        Setup();
    }

    private void Setup()
    {
        var tilesObject = new GameObject("Tiles");
        var worldObject = new GameObject("World");
        var levelPathObject = new GameObject("LevelPath");
        levelPathObject.AddComponent<LevelPath>();

        worldObject.transform.SetParent(tilesObject.transform);
        levelPathObject.transform.SetParent(tilesObject.transform);

        var groundTiles = LoadGroundTiles();
        foreach (var groundTile in groundTiles)
        {
            groundTile.transform.SetParent(worldObject.transform);
        }

        var pathTiles = LoadPathTiles();
        foreach (var pathTile in pathTiles)
        {
            pathTile.transform.SetParent(levelPathObject.transform);
        }

        var waypoints = BuildWaypoints();
        var levelPath = levelPathObject.GetComponent<LevelPath>();
        levelPath.SetupWaypoints(waypoints.ToArray());

        Instantiate(waveSpawner, Vector3.zero, Quaternion.identity);
    }

    private List<Vector3> BuildWaypoints() {
        var points = new List<Vector3>();

        // Find Spawners - for now assume one
        var spawner = GameObject.FindGameObjectWithTag(spawnerTag);
        points.Add(spawner.transform.position);

        var pathPoints = GameObject.FindGameObjectsWithTag(pathTag).Select(obj => obj.transform.position).ToList();

        var currentPoint = spawner.transform.position;
        int i = 0;
        // While we haven't checked every path tile...
        while(i < pathPoints.Count) {
            // Find adjacent points
            var adjacentPoints = pathPoints
                .Where(point => !points.Contains(point))
                // Account for slight distance error, though it shouldn't exist
                .Where(point => Vector3.Distance(point, currentPoint) <= 1.1f)
                .Select(point => point)
                .ToList();

            // Where should only find one
            if( adjacentPoints.Count() > 1 ) {throw new System.Exception("Too many adjacent paths found..."); }

            var adjacentPoint = adjacentPoints.First();

            // Add point to the list, mark as current and continue
            points.Add(adjacentPoint);
            currentPoint = adjacentPoint;
            i++;
        }

        return points;
    }

    private GameObject[] LoadGroundTiles()
    {
        var tiles = new List<GameObject>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixel = mapTexture.GetPixel(x, y);
                if (pixel == grassColor)
                {
                    var tile = Instantiate(grassTile, new Vector3(x, 0, y), Quaternion.identity);
                    tiles.Add(tile);
                }
            }
        }
        return tiles.ToArray();
    }

    private List<GameObject> LoadPathTiles()
    {
        var tiles = new List<GameObject>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixel = mapTexture.GetPixel(x, y);
                if (pixel == spawnColor)
                {
                    var tile = CreateSpawnerAt(x, y);
                    tiles.Add(tile);
                }
                else if (pixel == baseColor)
                {
                    var tile = CreateBaseAt(x, y);
                    tiles.Add(tile);
                }
                else if (pixel == pathColor)
                {
                    var tile = CreatePathAt(x, y);
                    tiles.Add(tile);
                }
            }
        }
        return tiles;
    }

    private GameObject CreateSpawnerAt(int x, int y)
    {
        var directionOfNextPath = GetAdjacentDirectionsFrom(x, y);
        var rotationFactor = 0;
        // This rotation assumes facing up is default
        switch (directionOfNextPath)
        {
            case TilePathDirection.OneUp: rotationFactor = 0; break;
            case TilePathDirection.OneRight: rotationFactor = 1; break;
            case TilePathDirection.OneLeft: rotationFactor = -1; break;
            case TilePathDirection.OneDown: rotationFactor = 2; break;
            default: throw new System.Exception("Spawner rotation cannot handle route.");
        }
        var quat = Quaternion.Euler(0, rotationFactor * 90, 0);

        return Instantiate(spawnTile, new Vector3(x, 0, y), quat);
    }

    private GameObject CreateBaseAt(int x, int y)
    {
        var directionOfNextPath = GetAdjacentDirectionsFrom(x, y);
        var rotationFactor = 0;
        // This rotation assumes facing up is default
        switch (directionOfNextPath)
        {
            case TilePathDirection.OneUp: rotationFactor = 0; break;
            case TilePathDirection.OneRight: rotationFactor = 1; break;
            case TilePathDirection.OneLeft: rotationFactor = -1; break;
            case TilePathDirection.OneDown: rotationFactor = 2; break;
            default: throw new System.Exception("Spawner rotation cannot handle route.");
        }
        var quat = Quaternion.Euler(0, rotationFactor * 90, 0);

        Instantiate(crystalBase, new Vector3(x, .15f, y), Quaternion.identity);
        return Instantiate(baseTile, new Vector3(x, 0, y), quat);
    }

    private GameObject CreatePathAt(int x, int y)
    {
        var directionOfNextPath = GetAdjacentDirectionsFrom(x, y);
        var rotationFactor = 0;
        var pathTileToCreate = pathStraightTile;

        switch (directionOfNextPath)
        {
            case TilePathDirection.TwoLeftRight: rotationFactor = 1; break;
            case TilePathDirection.TwoUpDown: rotationFactor = 0; break;
            case TilePathDirection.TwoLeftDown: rotationFactor = -1; pathTileToCreate = pathRightTurnTile; break;
            case TilePathDirection.TwoLeftUp: rotationFactor = 0; pathTileToCreate = pathRightTurnTile; break;
            case TilePathDirection.TwoRightDown: rotationFactor = 2; pathTileToCreate = pathRightTurnTile; break;
            case TilePathDirection.TwoRightUp: rotationFactor = 1; pathTileToCreate = pathRightTurnTile; break;
            default: throw new System.Exception("Path rotation cannot handle route.");
        }
        var quat = Quaternion.Euler(0, rotationFactor * 90, 0);

        return Instantiate(pathTileToCreate, new Vector3(x, 0, y), quat);
    }

    private TilePathDirection GetAdjacentDirectionsFrom(int x, int y)
    {
        var upColor = mapTexture.GetPixel(x, y + 1);
        var downColor = mapTexture.GetPixel(x, y - 1);
        var leftColor = mapTexture.GetPixel(x - 1, y);
        var rightColor = mapTexture.GetPixel(x + 1, y);

        // This entire method feels wrong, but I can't think of a better way at the moment
        var isUp = (upColor == pathColor || upColor == baseColor || upColor == spawnColor);
        var isDown = (downColor == pathColor || downColor == baseColor || downColor == spawnColor);
        var isLeft = (leftColor == pathColor || leftColor == baseColor || leftColor == spawnColor);
        var isRight = (rightColor == pathColor || rightColor == baseColor || rightColor == spawnColor);

        if (isUp && isDown && isLeft && isRight) return TilePathDirection.Four;
        if (!isUp && isDown && isLeft && isRight) return TilePathDirection.ThreeLeftRightDown;
        if (isUp && !isDown && isLeft && isRight) return TilePathDirection.ThreeLeftUpRight;
        if (isUp && isDown && !isLeft && isRight) return TilePathDirection.ThreeRightUpDown;
        if (isUp && isDown && isLeft && !isRight) return TilePathDirection.ThreeLeftUpDown;
        if (!isUp && !isDown && isLeft && isRight) return TilePathDirection.TwoLeftRight;
        if (!isUp && isDown && !isLeft && isRight) return TilePathDirection.TwoRightDown;
        if (!isUp && isDown && isLeft && !isRight) return TilePathDirection.TwoLeftDown;
        if (isUp && !isDown && !isLeft && isRight) return TilePathDirection.TwoRightUp;
        if (isUp && !isDown && isLeft && !isRight) return TilePathDirection.TwoLeftUp;
        if (isUp && isDown && !isLeft && !isRight) return TilePathDirection.TwoUpDown;
        if (isUp && !isDown && !isLeft && !isRight) return TilePathDirection.OneUp;
        if (!isUp && isDown && !isLeft && !isRight) return TilePathDirection.OneDown;
        if (!isUp && !isDown && isLeft && !isRight) return TilePathDirection.OneLeft;
        if (!isUp && !isDown && !isLeft && isRight) return TilePathDirection.OneRight;

        throw new System.Exception("TileDirection broke");
    }

    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        return tex;
    }
}
