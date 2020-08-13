using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarTimer : MonoBehaviour {
    public static float MAX_TIME_LEFT = 60;
    public static float timeLeft = MAX_TIME_LEFT;

    void Update() {
        transform.localScale = new Vector3(timeLeft / 6, 1, 0);
        if (GameTracker.playerIsAlive && GameTracker.playable) {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0) {
                GameTracker.playerIsAlive = false;
                Destroy(GameObject.Find("timer bar left"));
                Destroy(GameObject.Find("timer bar right"));
                Destroy(gameObject);
            }
        }
    }

    public static void increaseTime() {
        timeLeft += 10 * getTimeScalerFromSquaresCleared(GameTracker.squaresCompleted);
        if (timeLeft > MAX_TIME_LEFT) {
            timeLeft = MAX_TIME_LEFT;
        }
    }

    public static float getTimeScalerFromSquaresCleared(int squaresCleared) {
        if (squaresCleared <= 100) {
            return Mathf.Exp(-0.01609437912f * squaresCleared);
        }
        else {
            return Mathf.Exp(-0.01609437912f * 100);
        }
    }
}
