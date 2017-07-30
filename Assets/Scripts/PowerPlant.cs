using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlant : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerControl p = other.GetComponent<PlayerControl>();
            if (!p.hasPlug) {
                GameManager.instance.currentCords--;
                p.hasPlug = true;
                p.anim.loopMode = FTRuntime.SwfClipController.LoopModes.Once;
                p.anim.PlayIfNotAlreadyPlaying("player-pickup");
            }
        }
    }
}
