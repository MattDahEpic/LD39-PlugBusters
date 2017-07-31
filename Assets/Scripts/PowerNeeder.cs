using FTRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwfClipController),typeof(AudioSource))]
public class PowerNeeder : MonoBehaviour {
    [HideInInspector] public bool isConnected = false;
    [HideInInspector] public bool wantsConnection = false;
    [HideInInspector] public float dischargeRatePerSecond = 0.05f;
    public float wantsPlugAtChargeLevel = 0.25f;
    public GameObject keyIndicator;
    public SwfClipController chargeIndicator;

    private SwfClipController anim;
    private AudioSource aud;
    public float charge;

    void Start() {
        charge = Random.Range(0.25f,1f);
        dischargeRatePerSecond = Random.Range(0.01f, 0.06f);
        anim = GetComponent<SwfClipController>();
        anim.PlayIfNotAlreadyPlaying("none");
        keyIndicator.SetActive(false);
        aud = GetComponent<AudioSource>();
    }

    void Update() {
        if (!isConnected) {
            if (Time.timeSinceLevelLoad - Mathf.Floor(Time.timeSinceLevelLoad) <= 0.005f) { //and it is about a round second
                charge -= dischargeRatePerSecond;
                if (charge <= 0) {
                    GameManager.dissatisfaction += 0.01f;
                }
            }
            if (charge <= wantsPlugAtChargeLevel) anim.PlayIfNotAlreadyPlaying("indicator");
        }
        chargeIndicator.GotoAndStop(100-Mathf.FloorToInt(charge * 100)); //update charge meter
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerControl p = other.GetComponent<PlayerControl>();
            if (p.hasPlug) {
                keyIndicator.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E)) {
                    p.hasPlug = false;
                    p.anim.loopMode = SwfClipController.LoopModes.Once;
                    p.anim.PlayIfNotAlreadyPlaying("player-putdown");
                    ConnectToGrid();
                }
            } else {
                keyIndicator.SetActive(false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        keyIndicator.SetActive(false);
    }

    public void ConnectToGrid() {
        wantsConnection = false;
        isConnected = true;
        aud.Play();
        if (charge > 0) GameManager.score += 100 - Mathf.FloorToInt(charge*100); //the closer to, but not, empty gets you points
        anim.PlayIfNotAlreadyPlaying("pluggedin");
        StartCoroutine(Disconnect());
    }
    private IEnumerator Disconnect() {
        while (charge < 1f) {
            charge = Mathf.Clamp(charge + 0.1f, 0f, 1f);
            yield return new WaitForSeconds(1);
        }
        anim.PlayIfNotAlreadyPlaying("none");
        isConnected = false;
    }
}
