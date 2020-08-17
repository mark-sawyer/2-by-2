using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public GameObject[] slotNeighbours = new GameObject[4];

    public void setNeighbours() {
        RaycastHit2D ray;
        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(-0.5f, 0.5f), Vector2.zero);
        if (ray.collider != null) {
            slotNeighbours[0] = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(0.5f, 0.5f), Vector2.zero);
        if (ray.collider != null) {
            slotNeighbours[1] = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(0.5f, -0.5f), Vector2.zero);
        if (ray.collider != null) {
            slotNeighbours[2] = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(-0.5f, -0.5f), Vector2.zero);
        if (ray.collider != null) {
            slotNeighbours[3] = ray.collider.gameObject;
        }
    }

    public bool appropriateSlotsAreEmpty(Colour[] squareColours) {
        bool[] validNeighbours = new bool[4] { true, true, true, true };

        for (int i = 0; i < 4; i++) {
            if (slotNeighbours[i] != null) {
                validNeighbours[i] = squareColours[i] == Colour.NONE || slotNeighbours[i].GetComponent<Slot>().colour == Colour.NONE;
            }
            else {
                validNeighbours[i] = squareColours[i] == Colour.NONE;
            }
        }

        return validNeighbours[0] && validNeighbours[1] && validNeighbours[2] && validNeighbours[3];
    }

    public bool neighboursAreEmpty() {
        bool topLeftEmpty = slotNeighbours[0].GetComponent<Slot>().colour == Colour.NONE;
        bool topRightEmpty = slotNeighbours[1].GetComponent<Slot>().colour == Colour.NONE;
        bool bottomRightEmpty = slotNeighbours[2].GetComponent<Slot>().colour == Colour.NONE;
        bool bottomLeftEmpty = slotNeighbours[3].GetComponent<Slot>().colour == Colour.NONE;

        return topLeftEmpty && topRightEmpty && bottomRightEmpty && bottomLeftEmpty;
    }

    public void checkNeighboursHaveSingleColour() {
        Colour[] slotColours = new Colour[4];

        for (int i = 0; i < 4; i++) {
            slotColours[i] = slotNeighbours[i].GetComponent<Slot>().colour;
        }

        bool allSlotsTheSameColour = slotColours[0] != Colour.NONE &&
            slotColours[0] == slotColours[1] &&
            slotColours[0] == slotColours[2] &&
            slotColours[0] == slotColours[3];

        if (allSlotsTheSameColour) {
            GameTracker.respondToSingleColourNode();  // Set needToGoAgain to true, iterate squaresCompleted and score

            for (int i = 0; i < 4; i++) {
                slotNeighbours[i].GetComponent<Slot>().setSlotFlags(i, slotColours[i]);
            }
        }
    }

    public void setNeighbourColours(Colour[] colours) {
        for (int i = 0; i < 4; i++) {
            if (slotNeighbours[i] != null && colours[i] != Colour.NONE) {
                slotNeighbours[i].GetComponent<Slot>().setColour(colours[i]);
            } 
        }
    }
}
