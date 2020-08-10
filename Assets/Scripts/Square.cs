﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
    public Sprite redColour;
    public Sprite greenColour;
    public Sprite blueColour;
    public Sprite yellowColour;
    public Colour[] colours = new Colour[4];

    void Start() {
        // Get four random colours, at least two unique.
        for (int i = 0; i < 4; i++) {
            colours[i] = (Colour)Random.Range(0, 4);
        }

        while (colours[0] == colours[1] & colours[0] == colours[2] & colours[0] == colours[3]) {
            for (int i = 0; i < 4; i++) {
                colours[i] = (Colour)Random.Range(0, 4);
            }
        }

        for (int i = 0; i < 4; i++) {
            switch(colours[i]) {
                case Colour.RED:
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = redColour;
                    break;
                case Colour.GREEN:
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = greenColour;
                    break;
                case Colour.BLUE:
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = blueColour;
                    break;
                case Colour.YELLOW:
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = yellowColour;
                    break;
            }
        }
    }

    void Update() {
        
    }
}

