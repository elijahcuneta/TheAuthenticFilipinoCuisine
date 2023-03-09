using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawnHandler : MonoBehaviour {

    [SerializeField]
    private List<FoodSpawnSpot> foodSpawnSpots;

    [SerializeField]
    private Food food = null;

    private float delay = 5f;

    public void SetFoodSpawnDelay(float delay) {
        this.delay = delay;
    }

    public void SetFood(int id, CustomerBatch customerBatch) {
        StartCoroutine(CreateFood(id, delay, customerBatch));
    }
    private IEnumerator CreateFood(int id, float delay, CustomerBatch customerBatch) {
        yield return new WaitForSecondsRealtime(delay);
        customerBatch.GetCustomerInfoHandler().SetCheckPatienceActive(true);
        if (SpotAvailable()) {
            for(int i = 0; i < foodSpawnSpots.Count; i++) {
                if (!foodSpawnSpots[i].isSpotTaken) {
                    Food newFood = Instantiate(food, foodSpawnSpots[i].spotTransfom.position, Quaternion.identity);
                    customerBatch.GetCustomerInfoHandler().SetFoodReference(newFood);
                    foodSpawnSpots[i].isSpotTaken = true;
                    newFood.foodSpawnId = i;
                    newFood.SetFoodId(id, customerBatch);
                    break;
                }
            }
        }
    }
    private bool SpotAvailable() {
        foreach (FoodSpawnSpot fss in foodSpawnSpots) {
            if (!fss.isSpotTaken) {
                return true;
            }
        }
        return false;
    }

    public void SetFoodSpawnSpot_Available(int id) {
        if(id >= foodSpawnSpots.Count) {
            id = foodSpawnSpots.Count - 1;
        }
        foodSpawnSpots[id].isSpotTaken = false;
    }
}
