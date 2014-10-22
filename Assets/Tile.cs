using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    public Vector2 gamePosition;
    public enum TileType {FLOOR, WALL, DOOR, OPENDOOR, EMPTY};
    public TileType type;
    private Level level;

    Tile() {
        gamePosition.x = 0;
        gamePosition.y = 0;
        type = TileType.EMPTY;
    }

    Tile(int xin, int yin, TileType t) {
        gamePosition.x = xin;
        gamePosition.y = yin;
        type = t;
    }

    void Start() {
        level = GameObject.Find("LevelManager").GetComponent<Level>();
    }

    void OnMouseUpAsButton() {
        level.TileClicked(this);
    }
}
