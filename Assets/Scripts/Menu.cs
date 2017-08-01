using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {
    public UnityEngine.UI.Text prompt;
    AsyncOperation load;
    private void Start() {
        load = SceneManager.LoadSceneAsync("Game");
        load.allowSceneActivation = false;
    }
    void Update () {
        if (load.progress == 0.9f) {
            prompt.text = "Press any key to begin!";
            if (Input.anyKeyDown) {
                GameManager.score = 0;
                GameManager.dissatisfaction = 0;
                load.allowSceneActivation = true;
            }
        } else {
            prompt.text = "Please wait...";
        }
	}
}
