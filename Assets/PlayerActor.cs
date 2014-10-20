using UnityEngine;
using System.Collections;

public class PlayerActor : Actor {
    public bool playerActive;

    public Sprite selectedSprite;

	// Update is called once per frame
	new void Update () {
        base.Update();

        switch(actorState) {
            case State.STANDING:
                if (Input.GetKey("s") && playerActive) {
                    facing = Vector2.up;
                    if (checkCollision()) {
                        walkTarget.x = gameObject.transform.position.x;
                        walkTarget.y = gameObject.transform.position.y - level.TileSize;
                        actorState = State.WALKING;
                    }
                } else if (Input.GetKey("w") && playerActive) {
                    facing = -1 * Vector2.up;
                    if (checkCollision()) {
                        walkTarget.x = gameObject.transform.position.x;
                        walkTarget.y = gameObject.transform.position.y + level.TileSize;
                        actorState = State.WALKING;
                    }
                } else if (Input.GetKey("a") && playerActive) {
                    facing = -1 * Vector2.right;
                    if (checkCollision()) {
                        walkTarget.x = gameObject.transform.position.x - level.TileSize;
                        walkTarget.y = gameObject.transform.position.y;
                        actorState = State.WALKING;
                    }
                } else if (Input.GetKey("d") && playerActive) {
                    facing = Vector2.right;
                    if (checkCollision()) {
                        walkTarget.x = gameObject.transform.position.x + level.TileSize;
                        walkTarget.y = gameObject.transform.position.y;
                        actorState = State.WALKING;
                    }
                }
                break;
        }

        if (Input.GetKeyDown("left shift")) {
            playerActive = !playerActive;
        }

        if (playerActive) {
            GetComponent<SpriteRenderer>().sprite = selectedSprite;
            level.activePlayer = this;
        } else {
            GetComponent<SpriteRenderer>().sprite = sprite1;
        }
    }
}
