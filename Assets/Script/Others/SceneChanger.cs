using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

	public void SceneChange(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}
