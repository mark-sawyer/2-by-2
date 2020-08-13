using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextBackground : MonoBehaviour {
    public Sprite playable;
    public Sprite waiting;

    public void setPlayable() {
        GetComponent<SpriteRenderer>().sprite = playable;
    }

    public void setWaiting() {
        GetComponent<SpriteRenderer>().sprite = waiting;
    }
}
