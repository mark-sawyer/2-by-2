using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BarTimer : MonoBehaviour {
    public static float MAX_TIME_LEFT = 60;
    public static float timeLeft = MAX_TIME_LEFT;
    public static float FIRST_BLOCK_TIME_INCREASE = 5;
    public static float MIN_BLOCK_TIME_INCREASE = 1;
    public static int NUMBER_OF_BLOCKS_FOR_MIN_INCREASE = 50;
    public static float yIntercept;
    public static float gradient;

    private void Start() {
        GameEvents.rWasPressed.AddListener(resetTimer);

        gradient = (MIN_BLOCK_TIME_INCREASE - FIRST_BLOCK_TIME_INCREASE) / (NUMBER_OF_BLOCKS_FOR_MIN_INCREASE - 1);
        yIntercept = FIRST_BLOCK_TIME_INCREASE - gradient;
    }

    private void Update() {
        transform.localScale = new Vector3(timeLeft / 6, 1, 0);
        if (GameTracker.playerIsAlive && GameTracker.playable) {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0) {
                GameTracker.playerIsAlive = false;
                GameObject.Find("timer bar left").GetComponent<SpriteRenderer>().enabled = false;
                GameObject.Find("timer bar right").GetComponent<SpriteRenderer>().enabled = false;
                GameObject.Find("timer bar").GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    public static void increaseTime() {
        timeLeft += getTimeIncreaseFromSquaresCleared(GameTracker.squaresCompleted);
        if (timeLeft > MAX_TIME_LEFT) {
            timeLeft = MAX_TIME_LEFT;
        }
    }

    public static float getTimeIncreaseFromSquaresCleared(int squaresCleared) {
        if (squaresCleared < NUMBER_OF_BLOCKS_FOR_MIN_INCREASE) {
            return gradient * squaresCleared + yIntercept;
        }
        else {
            return MIN_BLOCK_TIME_INCREASE;
        }
    }

    private void resetTimer() {
        timeLeft = MAX_TIME_LEFT;
    }
}
