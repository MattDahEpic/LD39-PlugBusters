using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerNeeder : MonoBehaviour {
    public bool isConnected = false;
    public bool wantsConnection = false;
    public float disconnectTime = 5f;
    public float minTimeBetweenConnections = 12f;
    public float chancePerSecondOfWantingConnection = 0.2f;

    private float time;

    void Start() {
        time = minTimeBetweenConnections;
    }

    void Update() {
        if (!isConnected) {
            time -= Time.deltaTime;
            if (time <= 0) { //if time between connections has elapsed
                if (Time.timeSinceLevelLoad - Mathf.Floor(Time.timeSinceLevelLoad) <= 0.05f) { //and it is about a round second
                    if (Random.value <= chancePerSecondOfWantingConnection) { //and randomness loves you
                        wantsConnection = true;
                        //TODO show indicator
                    }
                }
            }
        }
    }
    public void ConnectToGrid() {
        wantsConnection = false;
        isConnected = true;
        //TODO show plug when player is done inserting it
        StartCoroutine(Disconnect());
    }
    private IEnumerator Disconnect() {
        float time = disconnectTime;
        while (time >= 0) {
            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
        }
        //TODO eject plug
        isConnected = false;
        time = minTimeBetweenConnections;
    }
}
