using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Actor : MonoBehaviour {
    public Vector2 gamePosition;

    public Sprite sprite1;

    protected Level level;
    protected enum State {STANDING, WALKING, ACTING};
    protected Stack<State> actorState;
    protected Vector3 walkTarget; //Location in world coordinates that Actor is moving towards

    protected GameObject actionTarget;

    public Vector2 facing;
    public float moveSpeed; //moveSpeed in Tiles/Second

    public List<Vector2> walkPath; //Stores a list of game locations denoting the actor's current path, with 0 being the end and Count-1 being the beginning

	// Use this for initialization
    protected void Start () {
        level = GameObject.Find("LevelManager").GetComponent<Level>();
        actorState = new Stack<State>();
        actorState.Push(State.STANDING);
        if (facing == Vector2.zero) {
            facing = -1 * Vector2.up;
        }
        walkPath = new List<Vector2>();
    }

	// Update is called once per frame
    protected void Update () {
        if (actorState.Peek() == null) {
            actorState.Push(State.STANDING);
        }

        switch (actorState.Peek()) {
            case State.STANDING:
                gameObject.transform.position = level.FindWorldPosition(gamePosition);
                if(walkPath.Count != 0) {
                    //Removes the last vector in WalkPath, converts it to a World Location, and passes it to walkTarget
                    walkTarget = level.FindWorldPosition(walkPath[walkPath.Count - 1]);
                    walkPath.RemoveAt(walkPath.Count - 1);
                    actorState.Push(State.WALKING);
                }
                break;
            case State.WALKING:
                Move();
                break;
            case State.ACTING:
                Act();
                break;
        }
    }

    //Moves Actor in direction of walkTarget
    protected void Move() {
        facing = Vector3.Normalize(walkTarget - gameObject.transform.position);
        float step = moveSpeed * Time.deltaTime * level.TileSize;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, walkTarget, step);
        gamePosition = level.FindGamePosition(gameObject.transform.position);

        if (gameObject.transform.position == walkTarget) {
            actorState.Pop();
        }
    }

    //Returns true if there is no collision
    protected bool checkCollision() {
        GameObject checkTile = level.GetAdjacentTile(gamePosition, facing);
        if (checkTile != null) {
            return checkTile.GetComponent<Tile>().type != Tile.TileType.WALL;
        } else {
            return false;
        }
    }

    protected void Act() {
        //Open Closed Doors
        if (actionTarget.GetComponent<Door>() != null && !actionTarget.GetComponent<Door>().isOpen) {
            double t = actionTarget.GetComponent<Door>().Interact();
            if (t <= 0) {
                actorState.Pop();
            }
        }
    }
}
