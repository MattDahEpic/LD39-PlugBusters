using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public int maxCords = 8;
    public PlayerControl player;

    public PowerNeeder[] powerNeeders;

    [HideInInspector] public int currentCords;
    [HideInInspector] public int score;

    public static GameManager instance {
        get {
            return Camera.main.GetComponent<GameManager>();
        }
    }

    void Start () {
        currentCords = maxCords;
        score = 0;
	}
	void Update () {
		
	}
}
