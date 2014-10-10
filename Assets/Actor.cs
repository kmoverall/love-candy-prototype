using UnityEngine;
using System.Collections;

public class Actor : MonoBehaviour {
    public Vector2 gamePosition;

    protected Level level;
    protected enum State {STANDING, WALKING, ACTING};
    protected State actorState;
    protected Vector3 walkTarget;
    public Vector2 facing;
    public float moveSpeed; //moveSpeed in Tiles/Second

	// Use this for initialization
    protected void Start () {
        level = GameObject.Find("LevelManager").GetComponent<Level>();
        actorState = State.STANDING;
        if (facing == Vector2.zero) {
            facing = -1 * Vector2.up;
        }
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

    protected void Move() {
        facing = Vector3.Normalize(walkTarget - gameObject.transform.position);
        float step = moveSpeed * Time.deltaTime * level.TileSize;
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, walkTarget, step);
        gamePosition = level.FindGamePosition(gameObject.transform.position);
    }

    //Returns true if there is no collision
    protected bool checkCollision() {
        int checkTile = level.GetAdjacentTile(gamePosition, facing);
        return checkTile == 0;
    }


}
