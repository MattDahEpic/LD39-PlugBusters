using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffscreenStuffIndicator : MonoBehaviour {
    public GameObject displayPrefab;
    private Dictionary<PowerNeeder, Image> currentDisplays = new Dictionary<PowerNeeder, Image>();
	
	void Update () {
		foreach (PowerNeeder p in GameManager.instance.powerNeeders) {
            if (p.wantsConnection) {
                Vector3 screenPos = Camera.main.WorldToViewportPoint(p.transform.position);
                if (screenPos.x >= 0 && screenPos.x <= 1 && screenPos.y >= 0 && screenPos.y <= 1) { //is on screen
                    if (currentDisplays.ContainsKey(p)) { //destroy UI if it exists
                        Destroy(currentDisplays[p].gameObject);
                        currentDisplays.Remove(p);
                    }
                } else { //is off the screen
                    float angle;
                    float xDiff = p.transform.position.x - GameManager.instance.player.transform.position.x;
                    float yDiff = p.transform.position.y - GameManager.instance.player.transform.position.y;
                    angle = Mathf.Atan(xDiff / yDiff) * 180f / Mathf.PI;
                    angle = Mathf.Clamp(angle, -135f, 135f);

                    if (!currentDisplays.ContainsKey(p)) {
                        GameObject g = Instantiate(displayPrefab,transform);
                        currentDisplays[p] = g.GetComponent<Image>();
                    }
                    Image arrow = currentDisplays[p];

                    Vector2 targetLook = p.transform.position - currentDisplays[p].transform.position;
                    var rotangle = Mathf.Atan2(targetLook.y, targetLook.x) * Mathf.Rad2Deg;
                    rotangle -= 135f;
                    arrow.transform.rotation = Quaternion.AngleAxis(-rotangle, Vector3.forward);

                    // Get half the Images width and height to adjust it off the screen edge;
                    RectTransform arrowRect = arrow.GetComponent<RectTransform>();
                    float halfImageWidth = arrowRect.sizeDelta.x / 2f;
                    float halfImageHeight = arrowRect.sizeDelta.y / 2f;

                    // Get Half the ScreenHeight and Width to position the image
                    float halfScreenWidth = (float)Screen.width / 2f;
                    float halfScreenHeight = (float)Screen.height / 2f;

                    float xPos = 0f;
                    float yPos = 0f;

                    // Left side of screen;
                    if (angle < -45) {
                        xPos = -halfScreenWidth + halfImageWidth;
                        // Ypos can go between +ScreenHeight/2  down to -ScreenHeight/2
                        // angle goes between -45 and -135
                        // change angle to a value between 0f and 1.0f and Lerp on that
                        float normalizedAngle = (angle + 45f) / -90f;
                        yPos = Mathf.Lerp(halfScreenHeight, -halfScreenHeight, normalizedAngle);
                        // at the top of the screen we need to move the image down half its height
                        // at the bottom of the screen we need to move it up half its height
                        // in the middle we need to do nothing. so we lerp on the angle again
                        float yImageOffset = Mathf.Lerp(-halfImageHeight, halfImageHeight, normalizedAngle);
                        yPos += yImageOffset;

                    }
                    // top of screen
                    else if (angle < 45) {
                        yPos = halfScreenHeight - halfImageHeight;
                        float normalizedAngle = (angle + 45f) / 90f;
                        xPos = Mathf.Lerp(-halfScreenWidth, halfScreenWidth, normalizedAngle);
                        float xImageOffset = Mathf.Lerp(halfImageWidth, -halfImageWidth, normalizedAngle);
                        xPos += xImageOffset;
                    }
                    // right side of screen
                    else {
                        xPos = halfScreenWidth - halfImageWidth;
                        float normalizedAngle = (angle - 45) / 90f;
                        yPos = Mathf.Lerp(halfScreenHeight, -halfScreenHeight, normalizedAngle);
                        float yImageOffset = Mathf.Lerp(-halfImageHeight, halfImageHeight, normalizedAngle);
                        yPos += yImageOffset;
                    }

                    arrowRect.anchoredPosition = new Vector3(xPos, yPos, 0);
                    // UI rotation is backwards from our system.  Positive angles go counterclockwise
                    //arrowRect.Rotate(Vector3.forward, -angle);
                }
            }
        }
	}
}
