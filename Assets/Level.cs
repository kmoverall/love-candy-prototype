using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Level : MonoBehaviour {

	private int[,] map;
    private GameObject[,] mapTiles;                     //Array holding all tiles that mae up the map
    private const int mapWidth = 16;
    private const int mapHeight = 10;
	public GameObject[] tiles = new GameObject[3];      //List of tiles used to create level

    private float tileSize;                             //Size of a tile
    public float TileSize {get {return tileSize;} }     //Property allowing other classes to access tileSize

    public enum InputState {PLAYER_SELECTED, ACTOR_SELECTED, TILE_SELECTED};

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

        mapTiles = new GameObject[mapHeight, mapWidth];

        for (int i = 0; i < mapHeight; i++) {
            for (int j = 0; j < mapWidth; j++) {
                mapTiles[i,j] = (GameObject)Instantiate(tiles[map[i, j]], 
                            new Vector3(tileSize*j - mapWidth*tileSize / 2, -1*tileSize*i + mapHeight*tileSize / 2, 0), 
                            Quaternion.identity);
                mapTiles[i,j].GetComponent<Tile>().gamePosition.x = j;
                mapTiles[i,j].GetComponent<Tile>().gamePosition.y = i;
                mapTiles[i,j].GetComponent<Tile>().type = (Tile.TileType)(map[i, j]);
            }
        }
	}

    public void MapObjectClicked (MonoBehaviour obj) {

    }

    //Finds the GamePosition based on its WorldPosition
    public Vector2 FindGamePosition (Vector3 worldPosition) {
        Vector2 gamePosition = new Vector2();
        gamePosition.x = Mathf.Floor(worldPosition.x / TileSize + Level.mapWidth / 2);
        gamePosition.y = Mathf.Floor(-1 * worldPosition.y / TileSize + Level.mapHeight / 2);
        return gamePosition;
    }
    
    //Finds the WorldPosition based on its GamePosition
    public Vector3 FindWorldPosition (Vector2 gamePosition) {
        Vector3 worldPosition = new Vector3();
        worldPosition.x = tileSize*gamePosition.x - Level.mapWidth*tileSize / 2;
        worldPosition.y = -1*tileSize*gamePosition.y + Level.mapHeight*tileSize / 2;
        return worldPosition;
    }

    //Takes in a Game Position and a direction and returns the tile in that direction
    public GameObject GetAdjacentTile(Vector2 gpos, Vector2 dir) {
        Vector2 tilePoint = gpos + dir;
        if (tilePoint.x >= 0 && tilePoint.y >= 0 && tilePoint.x < mapWidth && tilePoint.y < mapHeight) {
            return mapTiles[(int)(tilePoint.y), (int)(tilePoint.x)];
        } else {
            return null;
        }
    }

    //Takes in a World Position and a direction and returns the tile in that direction
    public GameObject GetAdjacentTile(Vector3 wpos, Vector2 dir) {
        Vector2 gpos = FindGamePosition(wpos);
        return GetAdjacentTile(gpos, dir);
    }

    //Returns neighboring tiles
    public List<GameObject> GetNeighbors(Vector2 gpos) {
        List<GameObject> result = new List<GameObject>();
        result.Add(GetAdjacentTile(gpos, Vector2.up));
        result.Add(GetAdjacentTile(gpos, Vector2.up * -1));
        result.Add(GetAdjacentTile(gpos, Vector2.right));
        result.Add(GetAdjacentTile(gpos, Vector2.right * -1));

        return result;
    }

    //Implementation of A* Seach, returns a path from start to goal
    public List<Vector2> AStarSearch(Vector2 start, Vector2 goal) {
        SortedDictionary<int, Vector2> frontier = new SortedDictionary<int, Vector2>();
        Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
        Dictionary<Vector2, int> distance = new Dictionary<Vector2, int>();
        List<Vector2> path = new List<Vector2>();

        cameFrom[start] = new Vector2(-1, -1);
        distance[start] = 0;
        frontier.Add(0, start);

        while (frontier.Count != 0) {
            Vector2 current = frontier[frontier.Keys.Max()];
            if (current == goal) {
                break;
            }

            //Remove tiles from GetNeighbors that are impassable
            foreach (GameObject next in GetNeighbors(current)) {
                int newCost = distance[current] + 1;
                Vector2 nextPos = next.GetComponent<Tile>().gamePosition;
                if (!distance.ContainsKey(nextPos) || newCost < distance[nextPos]) {
                    distance[nextPos] = newCost;
                    int priority = newCost + 1; //Add in manhattan distance heuristic
                    frontier.Add(priority, nextPos);
                    cameFrom[nextPos] = current;
                }
            }

        }

        path.Add(goal);
        Vector2 pathCurrent = goal;
        while(pathCurrent != start) {
            pathCurrent = cameFrom[pathCurrent];
            path.Add(cameFrom[pathCurrent]);
        }

        return path;
    }
}
