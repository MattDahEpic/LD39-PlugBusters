using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public int maxCords = 8;

    public GameObject[] tenants;

    [HideInInspector] public int currentCords;
    [HideInInspector] public int score;

    void Start () {
        currentCords = maxCords;
        score = 0;
	}
	void Update () {
		
	}
}
