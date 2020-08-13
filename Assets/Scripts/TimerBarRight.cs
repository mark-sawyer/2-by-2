using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBarRight : MonoBehaviour {
    public GameObject timerBar;

    void Update() {
        transform.position = new Vector3(-6 + timerBar.transform.localScale.x, timerBar.transform.position.y, -1);
    }
}
