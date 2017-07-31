using FTRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Text score;
    public SwfClipController dissapointment;
	
	void Update () {
        score.text = GameManager.score.ToString();
        dissapointment.GotoAndStop(Mathf.FloorToInt(GameManager.dissatisfaction*100));
    }
}
