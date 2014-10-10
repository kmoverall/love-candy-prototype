using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	private int[,] map;
    private const int mapWidth = 16;
    private const int mapHeight = 10;
	public GameObject[] tiles = new GameObject[3];      //List of tiles used to create level

    private float tileSize;                             //Size of a tile
    public float TileSize {get {return tileSize;} }     //Property allowing other classes to access tileSize

	// Use this for initialization
	void Start () {
        tileSize = tiles[0].GetComponent<SpriteRenderer>().bounds.max.x - tiles[0].GetComponent<SpriteRenderer>().bounds.min.x;
        map = new int[mapHeight, mapWidth] {{1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1},
                    						  {1, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                    						  {1, 0, 1, 2, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                    						  {1, 0, 1, 0, 0, 0, 0, 0, 1, 2, 1, 1, 1, 1, 1, 1},
                    						  {1, 2, 1, 2, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1},
                    						  {1, 0, 0, 0, 0, 0, 1, 0, 1, 1, 1, 1, 1, 2, 1, 1},
                    						  {1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                    						  {1, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 1},
                    						  {1, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 0, 0, 0, 0, 1},
                    						  {1, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1}};

        for (int i = 0; i < mapHeight; i++) {
            for (int j = 0; j < mapWidth; j++) {
                Instantiate(tiles[map[i, j]], 
                            new Vector3(tileSize*j - mapWidth*tileSize / 2, -1*tileSize*i + mapHeight*tileSize / 2, 0), 
                            Quaternion.identity);
            }
        }
	}

    //Sets the GamePosition based on its WorldPosition
    public Vector2 FindGamePosition (Vector3 worldPosition) {
        Vector2 gamePosition = new Vector2();
        gamePosition.x = Mathf.Floor(worldPosition.x / TileSize + Level.mapWidth / 2);
        gamePosition.y = Mathf.Floor(-1 * worldPosition.y / TileSize + Level.mapHeight / 2);
        return gamePosition;
    }
    
    //Sets the WorldPosition based on its GamePosition
    public Vector3 FindWorldPosition (Vector2 gamePosition) {
        Vector3 worldPosition = new Vector3();
        worldPosition.x = tileSize*gamePosition.x - Level.mapWidth*tileSize / 2;
        worldPosition.y = -1*tileSize*gamePosition.y + Level.mapHeight*tileSize / 2;
        return worldPosition;
    }

    public int GetAdjacentTile(Vector2 gpos, Vector2 dir) {
        Vector2 tilePoint = gpos + dir;
        if (gpos.x >= 0 || gpos.y >= 0) {
            return map[(int)(tilePoint.y), (int)(tilePoint.x)];
        } else {
            return -1;
        }
    }

    public int GetAdjacentTile(Vector3 wpos, Vector2 dir) {
        Vector2 gpos = FindGamePosition(wpos);
        return GetAdjacentTile(gpos, dir);
    }
}
