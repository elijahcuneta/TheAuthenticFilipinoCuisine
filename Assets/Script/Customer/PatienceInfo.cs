using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatienceState {
    FULL = 0,
    FADING = 1,
    EMPTY = 2
};
public class PatienceInfo : MonoBehaviour {

    public PatienceState patienceState = PatienceState.FULL;
    public Animator patienceAnimator;

    public void ShowPatience() {
        gameObject.SetActive(true);
    }

    public void HidePatience() {
        gameObject.SetActive(false);
    }

    public void UpdatePatienceState(PatienceState newPatienceState) {
        patienceState = newPatienceState;
        UpdateAnimation();
    }

    private void UpdateAnimation() {
        patienceAnimator.SetBool("Full", patienceState == PatienceState.FULL);
        patienceAnimator.SetBool("Fading", patienceState == PatienceState.FADING);
        patienceAnimator.SetBool("Empty", patienceState == PatienceState.EMPTY);
    }
}
