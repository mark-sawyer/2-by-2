using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour {
    void Start() {
        
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.zero);
            if (ray.collider != null) {
                ray.collider.GetComponent<Square>().OnTriggerEnter2D(ray.collider);
            }
        }
    }
}

public enum Colour {
    RED,
    GREEN,
    BLUE,
    YELLOW
};