using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {
    public Vector2 gamePosition;

    public Sprite sprite1;

    protected Level level;
    protected enum State {STANDING, WALKING, ACTING};
    protected State actorState;
    protected Vector3 walkTarget; //Location in world coordinates that Actor is moving towards

    public Vector2 facing;
    public float moveSpeed; //moveSpeed in Tiles/Second

    public List<Vector2> walkPath; //Stores a list of game locations denoting the actor's current path, with 0 being the end and Count-1 being the beginning

	// Use this for initialization
    protected void Start () {
        level = GameObject.Find("LevelManager").GetComponent<Level>();
        actorState = State.STANDING;
        if (facing == Vector2.zero) {
            facing = -1 * Vector2.up;
        }
        walkPath = new List<Vector2>();
    }

	// Update is called once per frame
    protected void Update () {
        switch (actorState) {
            case State.STANDING:
                gameObject.transform.position = level.FindWorldPosition(gamePosition);
                if(walkPath.Count != 0) {
                    //Removes the last vector in WalkPath, converts it to a World Location, and passes it to walkTarget
                    walkTarget = level.FindWorldPosition(walkPath[walkPath.Count - 1]);
                    walkPath.RemoveAt(walkPath.Count - 1);
                    actorState = State.WALKING;
                }
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
    }
}
