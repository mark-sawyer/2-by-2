using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTimer : MonoBehaviour {
    public static float timeLeft = 10;
    public static bool doCountDown = true;

    void Update() {
        if (doCountDown) {
            timeLeft -= Time.deltaTime;
            transform.localScale = new Vector3(timeLeft / 6, 1, 0);

            if (timeLeft <= 0) {
                GameTracker.playerIsAlive = false;
                Destroy(GameObject.Find("timer bar left"));
                Destroy(GameObject.Find("timer bar right"));
                Destroy(gameObject);
            }
        }
    }
}
