using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSetManager : MonoBehaviour {

    [SerializeField]
    private Table[] tables;

    public bool IsAllTableAvailable() {
        for(int i = 0; i < tables.Length; i++) {
            if (tables[i].GetCurrentTableState() == Table.TableState.OCCUPIED) {
                return false;
            }
        }
        return true;
    }
    public void SetAllTableActive(bool active) {
        for(int i = 0; i < tables.Length; i++) {
            tables[i].gameObject.SetActive(active);
        }
    }
    public void SetEachTableActive(int index, bool active) {
        if (index > tables.Length) {
            index = tables.Length - 1;
        } else if (index < 0) {
            index = 0;
        }

        tables[index].gameObject.SetActive(active);
    }
}
