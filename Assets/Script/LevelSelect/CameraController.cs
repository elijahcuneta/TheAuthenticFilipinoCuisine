using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    float rangeX, rangeY, speed, drag, startSpeed;
    Vector3 velocity, previousPosition, move;
    bool active;

    void Start() {
        active = false;
        StartCoroutine(goToActiveStage());
    }

    IEnumerator goToActiveStage() {
        if (Stage.current != null) {
            Vector2 target = Stage.current.transform.position;
            if (target.x < -rangeX) {
                target.x = -rangeX;
            } else if (target.x > rangeX) {
                target.x = rangeX;
            }
            if (target.y < -rangeY) {
                target.y = -rangeY;
            } else if (target.y > rangeY) {
                target.y = rangeY;
            }
            Vector2 currentPosition = transform.position;
            do {
                currentPosition = Vector2.MoveTowards(currentPosition, target, startSpeed * Time.deltaTime);
                transform.position = new Vector3(currentPosition.x, currentPosition.y, transform.position.z);
                yield return null;
            } while (currentPosition != target);
        }
        active = true;
    }

    void Update() {
        if (active && LevelSelectOverlayManager.instance.enableStageButtonInput) {
            if (previousPosition != Vector3.zero && (Input.GetMouseButton(0) || Input.touchCount > 0)) {
                move = previousPosition - Input.mousePosition;
                move.Normalize();
            }
            velocity = move * speed;
            Vector3 temp = transform.position + velocity;
            Vector3 valid = transform.position;
            if (temp.x > -rangeX && temp.x < rangeX) {
                valid.x = temp.x;
            }
            if (temp.y > -rangeY && temp.y < rangeY) {
                valid.y = temp.y;
            }
            transform.position = valid;
            velocity = Vector3.Lerp(velocity, Vector3.zero, drag * Time.deltaTime);
            previousPosition = Input.mousePosition;
        }else {
            velocity = Vector3.zero;
        }
    }
}
