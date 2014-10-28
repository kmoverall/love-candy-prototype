using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
    public Vector2 gamePosition;
    public enum TileType {FLOOR, WALL, DOOR, OPENDOOR, EMPTY};
    public TileType type;
    private Level level;

    protected Tile() {
        gamePosition.x = 0;
        gamePosition.y = 0;
        type = TileType.EMPTY;
    }

    public Tile(int xin, int yin, TileType t) {
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

    //Returns the amount of time left in the action
    public double Interact() { 
        Debug.Log("Interacting with Tile");
        return 0; }
}
