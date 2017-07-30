using FTRuntime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods {
    #region FlashTools
    public static void PlayIfNotAlreadyPlaying(this SwfClipController controller, string sequence) {
        if (controller.clip.sequence != sequence) controller.Play(sequence);
    }
    #endregion
}
