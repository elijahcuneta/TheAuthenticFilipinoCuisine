using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour {

    [SerializeField]
    private BoxCollider2D boxCollider2D;

    [SerializeField]
    protected string keyName = "";

    [SerializeField]
    protected Transform[] offsets;

    [SerializeField]
    protected Transform origin;

    [SerializeField]
    private GameObject[] check = null;

    [SerializeField]
    private int countLimit = 2;

    private InputInteractableHandler inputInteractableHandler = null;
    private bool isTargetAccepted = false;
    private bool isSecondCheckActive = false;
    private int clickCount = 0;
    protected bool checkObject = true;


    public virtual void Start() {
        inputInteractableHandler = GameLevelManager.Instance.GetInputInteractableManager();
    }

    private void OnDestroy() {
        checkObject = false;
    }

    protected Vector3 GetTargetOffset() {
        if(offsets.Length <= 0) {
            Debug.Log("No Offset has been initialized");
            return Vector3.zero; //PlayerController.Instance.playerMovement.GetPlayerPosition();
        }

        if(offsets.Length == 1) {
            return offsets[0].position;
        }

        Vector3 destination = Vector3.zero;
        int targetsSize = inputInteractableHandler.Targets.Count;
        Vector3 currentPosition = targetsSize != 0 ? inputInteractableHandler.Targets.ToArray()[targetsSize - 1].originalPosition //get tail of queue as reference for computing distance between current target and next
                                                   : GameLevelManager.Instance.GetPlayerController().playerMovement.GetPlayerPosition(); //if there is no target in queue, use the player's position

        destination = offsets[0].position; //assuming the first offset is the shortest
        for (int i = 0; i < offsets.Length; i++) {
             if (Mathf.Abs(offsets[i].position.x - currentPosition.x) < Mathf.Abs(destination.x - currentPosition.x)) {
                 destination = offsets[i].position;
             }
        }

        return destination;
    }

    protected bool SetTargetPositions(InteractableObject interactableObject, Vector3 position, Vector3 originalPosition) {
        return inputInteractableHandler.SetTargets(interactableObject, position, originalPosition);
    }

    private void OnMouseDown() {
        clickCount++;
        if(clickCount > countLimit) {
            clickCount = countLimit;
            return;
        }
        AudioManager.instance.click.Play();
        isTargetAccepted = SetTargetPositions(this, GetTargetOffset(), origin.position);
    }

    public string GetKeyName {
        get { return keyName; }
    }
    public void SetCheckActiveOff() {
        if (check == null) {
            return;
        }
        if (!checkObject) {
            return;
        }

        check[0].SetActive(false);

        if(check.Length <= 1) {
            return;
        }

        if (isSecondCheckActive) {
            isSecondCheckActive = false;
            check[1].SetActive(false);
            return;
        } else if (check[1].activeSelf) {
            isSecondCheckActive = true;
        }
    }
    public void SetCheckActiveOn(int index) {
        if(check == null) {
            return;
        }
        check[index].SetActive(true);
    }

    public void SetClickCount(int newClickCount) {
        clickCount = newClickCount;
    }
    public int GetClickCount() {
        return clickCount;
    }

    public void SetClickable(bool active) {
        boxCollider2D.enabled = active;
    }

}
