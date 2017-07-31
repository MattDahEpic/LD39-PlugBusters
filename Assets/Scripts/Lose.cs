using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lose : MonoBehaviour {
    public Text finalScore;
	void Update () {
        finalScore.text = "Final Score: "+GameManager.score;
        if (Input.anyKeyDown) SceneManager.LoadScene("Menu");
	}
}
