using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    // Start is called before the first frame update
    void Start()
    {
        mapTexture = LoadPNG("D:\\Personal\\Development\\Unity Projects\\Tower-Defense-Prototype\\DevAssets\\Maps\\DemoMap1.png");
        width = mapTexture.width;
        height = mapTexture.height;

        LoadTiles();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void LoadTiles()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color pixel = mapTexture.GetPixel(x, y);
                if (pixel == grassColor)
                {
                    Instantiate(grassTile, new Vector3(x, 0, y), Quaternion.identity);
                }
                else if (pixel == spawnColor)
                {
                    CreateSpawnerAt(x, y);
                }
                else if (pixel == baseColor)
                {
                    CreateBaseAt(x, y);
                }
                else if (pixel == pathColor)
                {
                    CreatePathAt(x, y);
                }
            }
        }
    }

    private void CreateSpawnerAt(int x, int y)
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

        Instantiate(spawnTile, new Vector3(x, 0, y), quat);
    }

    private void CreateBaseAt(int x, int y)
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

        Instantiate(baseTile, new Vector3(x, 0, y), quat);
    }

    private void CreatePathAt(int x, int y)
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

        Instantiate(pathTileToCreate, new Vector3(x, 0, y), quat);
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
