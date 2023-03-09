using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnBoardingManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] baseGO = null;

    [SerializeField]
    private Sprite[] onBoardingImages = null;

    [SerializeField]
    private GameObject baseOnBoardingImage = null;

    [SerializeField]
    private GameObject startButton = null;

    [SerializeField]
    private GameObject skipButton = null;

    [SerializeField]
    private Transform onBoardingTransform = null;

    [SerializeField]
    private GameObject container = null;

    [SerializeField]
    private bool neverAccessOnBoarding = false;

    private float distance = 350f;
    private float baseDistance = 350f;

    private float swipeSpeed = 10f;
    private int totalPages = 0;
    private int currentPage = 0;

    void Awake () {
        onBoardingTransform.GetComponent<RectTransform>().localPosition = new Vector3(0, onBoardingTransform.localPosition.y, 0);
        skipButton.SetActive(false);
        startButton.SetActive(false);
        if (PlayerPrefs.GetString("OnBoardingDone") != "hooray") {
            SetBaseGOActive(false);
        }

        if(neverAccessOnBoarding && PlayerPrefs.GetString("OnBoardingDone") == "hooray") {
            gameObject.SetActive(false);
            return;
        }

        distance = baseDistance;
        distance = 0f;

        totalPages = onBoardingImages.Length - 1;
        currentPage = 0;

        Vector3 position = Vector3.zero;
        RectTransform getRT = onBoardingTransform.GetComponent<RectTransform>();

        for (int i = 0; i < onBoardingImages.Length; i++) {
            GameObject newBaseOnBoardingImage = Instantiate(baseOnBoardingImage, position, Quaternion.identity);
            newBaseOnBoardingImage.transform.parent = onBoardingTransform;
            newBaseOnBoardingImage.transform.GetComponent<RectTransform>().localPosition = position + (Vector3.right * distance);
            newBaseOnBoardingImage.GetComponentInChildren<Image>().sprite = onBoardingImages[i];
            distance += baseDistance;
        }
        if (PlayerPrefs.GetString("OnBoardingDone") == "hooray") {
            skipButton.SetActive(true);
            startButton.SetActive(false);
            return;
        }
        Time.timeScale = 0;
    }

    public void OnNextClick() {
        if(currentPage >= totalPages) {
            if (!startButton.activeSelf) {
                startButton.SetActive(true);
                skipButton.SetActive(false);
            }
            return;
        } 
        onBoardingTransform.localPosition -= (Vector3.right * baseDistance);
        ++currentPage;
    }

    public void OnPreviousClick() {
        if (currentPage <= 0) {
            return;
        }
        onBoardingTransform.localPosition += (Vector3.right * baseDistance);
        --currentPage;
    }

    public void OnClickStart() {
        Time.timeScale = 1;
        SetBaseGOActive(true);
        if (neverAccessOnBoarding) {
            gameObject.SetActive(false);
        }
    }

    public void OnClickOnBoarding() {
        onBoardingTransform.GetComponent<RectTransform>().localPosition = new Vector3(0, onBoardingTransform.localPosition.y, 0);
        if (PlayerPrefs.GetString("OnBoardingDone") == "hooray") {
            skipButton.SetActive(true);
            startButton.SetActive(false);
        }
        container.SetActive(true);
        Time.timeScale = 0;
    }

    private void SetBaseGOActive(bool active) {
        if (baseGO == null)
            return;

        foreach(GameObject g in baseGO) {
            g.SetActive(active);
        }
    }
}
