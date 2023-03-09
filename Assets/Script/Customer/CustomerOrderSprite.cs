using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerOrderSprite : MonoBehaviour {

    public SpriteRenderer[] foodOrderSR;

    public void SetFoodSprite(Sprite newSPrite, int index) {
        foodOrderSR[index].sprite = newSPrite;
    }

    public void SetOrderSpriteActive(bool active) {
        gameObject.SetActive(active);
    }
}
