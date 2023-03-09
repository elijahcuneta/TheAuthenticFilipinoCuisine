using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Vector3 playerPosition = Vector3.zero; //current Position
    private Vector3 newPlayerPosition = Vector3.zero; //new Position (while moving) ; avoiding new Vector3 per frame

    private PlayerController playerController = null;

    [SerializeField]
    private Transform parent, playerCharacter;

    public Transform[] arm;

    [HideInInspector]
    public bool reachingNewPosition = false;

    [HideInInspector]
    public bool moving = false;

    private float speed = 0;

    void Start() {
        playerController = GameLevelManager.Instance.GetPlayerController();
        SetPlayerSpeedLevel(0);
        playerPosition = parent.position;
        newPlayerPosition = playerPosition;

        reachingNewPosition = moving = false;
    }

    public void Player_GoTo(Vector3 newPosition, bool reachedOriginalTarget = false) {
        StartCoroutine(MovePosition(newPosition, reachedOriginalTarget));
    }
    private IEnumerator MovePosition(Vector3 targetPosition, bool reachedOriginalTarget) {
        if (playerController.playerInfo.GetPlayerState() == PlayerState.WALKING || playerController.playerInfo.GetPlayerState() == PlayerState.IDLE) {
            playerController.playerInfo.SetPlayerState(PlayerState.WALKING, "MovePosition@PlayerMovement.cs");
        }

        reachingNewPosition = moving = true;
        newPlayerPosition = GetPlayerPosition();


        //ensuring player will relocate to position wherein its z = -5;
        targetPosition = new Vector3(targetPosition.x, targetPosition.y, newPlayerPosition.z);
        //If newPos.x < playerX, direction is to the left, otherwise, right. Same in Y
        Vector2 playerDirection = new Vector2(targetPosition.x < parent.position.x ? -1 : 1, targetPosition.y < parent.position.y ? -1 : 1);

        while (newPlayerPosition.x != targetPosition.x) {
            newPlayerPosition += (Vector3.right * (speed * playerDirection.x) * Time.deltaTime);

            if ((playerDirection.x < 0 && newPlayerPosition.x < targetPosition.x) || (playerDirection.x > 0 && newPlayerPosition.x > targetPosition.x)) {
                newPlayerPosition = new Vector3(targetPosition.x, newPlayerPosition.y, newPlayerPosition.z);
            }
            
            if (playerDirection.x < 0) {
                playerCharacter.localScale = new Vector3(-1, 1, 1);
            } else {
                playerCharacter.localScale = new Vector3(1, 1, 1);
            }

            playerController.animator.Play("Side_Run_" + playerController.playerInfo.GetCarryCount());

            parent.position = newPlayerPosition;
            playerPosition = parent.position;
            LayerManager.instance.updateLayer(parent);
            yield return new WaitForEndOfFrame();
        }
        while (newPlayerPosition.y != targetPosition.y) {
            newPlayerPosition += (Vector3.up * (speed * playerDirection.y) * Time.deltaTime);

            if (playerDirection.y < 0) {
                playerController.animator.Play("Front_Run_" + playerController.playerInfo.GetCarryCount());
            } else {
                playerController.animator.Play("Back_Run_" + playerController.playerInfo.GetCarryCount());
            }

            if ((playerDirection.y < 0 && newPlayerPosition.y < targetPosition.y) || (playerDirection.y > 0 && newPlayerPosition.y > targetPosition.y)) {
                newPlayerPosition = targetPosition;
            }

            parent.position = newPlayerPosition;
            playerPosition = parent.position;
            LayerManager.instance.updateLayer(parent);
            yield return new WaitForEndOfFrame();
        }

        playerController.animator.Play("Front_Idle_" + playerController.playerInfo.GetCarryCount());
        if (playerController.playerInfo.GetPlayerState() == PlayerState.WALKING || playerController.playerInfo.GetPlayerState() == PlayerState.IDLE) {
            playerController.playerInfo.SetPlayerState(PlayerState.IDLE, "MovePosition@PlayerMovement.cs");
        }
        reachingNewPosition = reachedOriginalTarget;
        moving = false;
        StopCoroutine(MovePosition(targetPosition, reachedOriginalTarget));
    }

    public Vector3 GetPlayerPosition() {
        //Debug.Log("Player's Position: " + playerPosition);
        return playerPosition;
    }

    public void SetPlayerSpeedLevel(int newSpeedLevel) {
        playerController.playerStats.UpdatePlayerSpeed(newSpeedLevel);
        speed = playerController.playerStats.currentPlayerSpeed;
        playerController.animator.SetFloat("speed", speed * 0.5f);
    }
    public float GetPlayerSpeed() {
        return speed;
    }

    public void updateIdle() {
        playerController.animator.Play("Front_Idle_" + playerController.playerInfo.GetCarryCount());
    }

}
