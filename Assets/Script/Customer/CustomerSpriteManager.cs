using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpriteManager : MonoBehaviour {

    [HideInInspector]
    public Sprite arm_sprite, spoon_sprite, fork_sprite, legSeated_sprite, legStand_sprite, body_sprite, head_sprite, angry_sprite, openMouth_sprite;
    
    SpriteRenderer leg, body, leftArm, rightArm, head;

    void Awake () {
        leg = transform.Find("Leg").GetComponent<SpriteRenderer>();	
        body = transform.Find("Body").GetComponent<SpriteRenderer>();	
        leftArm = body.transform.Find("Left").GetComponentInChildren<SpriteRenderer>();	
        rightArm = body.transform.Find("Right").GetComponentInChildren<SpriteRenderer>();
        head = body.transform.Find("Head").GetComponent<SpriteRenderer>();
    }

    public void loadNormalArm() {
        leftArm.sprite = arm_sprite;
        rightArm.sprite = arm_sprite;
    }

    public void loadEatingArm() {
        leftArm.sprite = fork_sprite;
        rightArm.sprite = spoon_sprite;
    }

    public void loadSeated() {
        leg.sprite = legSeated_sprite;
    }

    public void loadStanded() {
        leg.sprite = legStand_sprite;
    }
    
    public void loadAngry() {
        head.sprite = angry_sprite;
    }

    public void loadOpenMouth() {
        head.sprite = openMouth_sprite;
    }

    public void loadNormalHead() {
        head.sprite = head_sprite;
    }

    public void loadBody() {
        body.sprite = body_sprite;
    }

    public void loadAllNormalSeated() {
        loadBody();
        loadNormalArm();
        loadNormalHead();
        loadSeated();
    }

    public void loadAllNormalStand() {
        loadBody();
        loadNormalArm();
        loadNormalHead();
        loadStanded();
    }

    public void loadAngryStand() {
        loadAllNormalStand();
        loadAngry();
    }
    public void loadAngrySeated() {
        loadAllNormalSeated();
        loadAngry();
    }

    public void loadEating() {
        loadAllNormalSeated();
        loadEatingArm();
    }

    public void loadEatingAngry() {
        loadAllNormalSeated();
        loadAngry();
    }

}
