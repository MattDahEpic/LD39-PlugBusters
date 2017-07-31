using FTRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwfClipController),typeof(AudioSource))]
public class PowerPlant : MonoBehaviour {
    public GameObject keyIndicator;

    private SwfClipController anim;
    private AudioSource aud;
    private void Start() {
        anim = GetComponent<SwfClipController>();
        anim.loopMode = SwfClipController.LoopModes.Once;
        aud = GetComponent<AudioSource>();
        keyIndicator.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerControl p = other.GetComponent<PlayerControl>();
            if (!p.hasPlug) {
                keyIndicator.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E)) {
                    p.hasPlug = true;
                    p.anim.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
                    p.anim.PlayIfNotAlreadyPlaying("player-pickup");
                    anim.Play("chargingstation");
                    aud.Play();
                }
            } else {
                keyIndicator.SetActive(false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        keyIndicator.SetActive(false);
    }
}
