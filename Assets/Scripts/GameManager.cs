using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public PlayerControl player;

    public static int score = 0;
    public static float dissatisfaction = 0f;

    public static GameManager instance {
        get {
            return Camera.main.GetComponent<GameManager>();
        }
    }

	void Update () {
		if (dissatisfaction >= 1f) {
            SceneManager.LoadScene("Lose");
        }
	}
}
