using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {
    public Vector2 gamePosition;

    protected Level level;
    protected enum State {STANDING, WALKING, ACTING};
    protected State actorState;
    protected Vector3 walkTarget; //Location in world coordinates that Actor is moving towards
    public Vector2 facing;
    public float moveSpeed; //moveSpeed in Tiles/Second

    protected Dictionary<Vector2, Vector2> walkPath;

	// Use this for initialization
    protected void Start () {
        level = GameObject.Find("LevelManager").GetComponent<Level>();
        actorState = State.STANDING;
        if (facing == Vector2.zero) {
            facing = -1 * Vector2.up;
        }
        walkPath = new Dictionary<Vector2, Vector2>();
    }

	// Update is called once per frame
    protected void Update () {
        switch (actorState) {
            case State.STANDING:
                gameObject.transform.position = level.FindWorldPosition(gamePosition);
                break;
            case State.WALKING:
                Move();
                if (gameObject.transform.position == walkTarget) {
                    actorState = State.STANDING;
                }
                break;
            case State.ACTING:
                break;
        }
    }

    //Moves Actor in direction of walkTarget
    protected void Move() {
        facing = Vector3.Normalize(walkTarget - gameObject.transform.position);
        float step = moveSpeed * Time.deltaTime * level.TileSize;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, walkTarget, step);
        gamePosition = level.FindGamePosition(gameObject.transform.position);
    }

    //Returns true if there is no collision
    protected bool checkCollision() {
        GameObject checkTile = level.GetAdjacentTile(gamePosition, facing);
        if (checkTile != null) {
            return checkTile.GetComponent<Tile>().type == Tile.TileType.FLOOR;
        } else {
            return false;
        }
    }

    protected void OnMouseUpAsButton() {
        level.MapObjectClicked(this);
    }
}
