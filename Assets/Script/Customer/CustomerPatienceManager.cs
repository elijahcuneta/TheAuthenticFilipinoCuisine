using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerPatienceManager : MonoBehaviour {

    [SerializeField]
    private PatienceInfo[] patienceInfo;

    private void Start() {
      
    }

    public PatienceInfo GetPatienceInfo(int index) {
        if(index > patienceInfo.Length) {
            index = patienceInfo.Length - 1;
        }

        return patienceInfo[index];
    }

    public void ChangePatienceInfoState(int index, PatienceState newPatienceState) {
        if (index > patienceInfo.Length) {
            index = patienceInfo.Length - 1;
        }

        patienceInfo[index].UpdatePatienceState(newPatienceState);
    }

    public bool IsAllHeartNotEmpty() {
        foreach(PatienceInfo pI in patienceInfo) {
            if(pI.patienceState != PatienceState.EMPTY) {
                return true;
            }
        }
        return false;
    }

    public void SetActivePatience(int activePatience) {
        if (activePatience > patienceInfo.Length) {
            activePatience = patienceInfo.Length - 1;
        }

        foreach(PatienceInfo pI in patienceInfo) {
            pI.UpdatePatienceState(PatienceState.EMPTY);
        }

        for(int i = 0; i < activePatience; i++) {
            patienceInfo[i].UpdatePatienceState(PatienceState.FULL);
        }

    }
    
    public int GetPatienceInfoCount() {
        return patienceInfo.Length;
    }
}
