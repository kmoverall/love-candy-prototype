using UnityEngine;
using System.Collections;

public class Door : Tile {

    public bool isOpen;
    private bool opening;
    private const double timeToOpen = 0.75;
    private double openTimer;
    public Sprite openSprite;
    public Sprite closedSprite;

    protected Door() {
        gamePosition.x = 0;
        gamePosition.y = 0;
        type = TileType.DOOR;
        openTimer = timeToOpen;
        isOpen = false;
    }
    
    public Door(int xin, int yin, TileType t) {
        gamePosition.x = xin;
        gamePosition.y = yin;
        type = t;
        openTimer = timeToOpen;
        isOpen = (t == TileType.OPENDOOR);
    }


    void Update() {
        if (!opening) {
            openTimer = timeToOpen;
        }
    }

    new public double Interact() { 
        opening = true;
        openTimer -= Time.deltaTime;

        if (opening && openTimer <= 0) {
            opening = false;
            isOpen = !isOpen;
        }
        
        if (isOpen) {
            type = TileType.OPENDOOR;
            GetComponent<SpriteRenderer>().sprite = openSprite;
        } else {
            type = TileType.DOOR;
            GetComponent<SpriteRenderer>().sprite = closedSprite;
        }

        return openTimer;
    }
}
