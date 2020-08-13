using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    public GameObject topLeftSlot;
    public GameObject topRightSlot;
    public GameObject bottomRightSlot;
    public GameObject bottomLeftSlot;

    public void setNeighbours() {
        RaycastHit2D ray;
        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(-0.5f, 0.5f), Vector2.zero);
        if (ray.collider != null) {
            topLeftSlot = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(0.5f, 0.5f), Vector2.zero);
        if (ray.collider != null) {
            topRightSlot = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(0.5f, -0.5f), Vector2.zero);
        if (ray.collider != null) {
            bottomRightSlot = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(-0.5f, -0.5f), Vector2.zero);
        if (ray.collider != null) {
            bottomLeftSlot = ray.collider.gameObject;
        }
    }

    public bool neighboursAreEmpty() {
        bool topLeftEmpty = topLeftSlot.GetComponent<Slot>().colour == Colour.NONE;
        bool topRightEmpty = topRightSlot.GetComponent<Slot>().colour == Colour.NONE;
        bool bottomRightEmpty = bottomRightSlot.GetComponent<Slot>().colour == Colour.NONE;
        bool bottomLeftEmpty = bottomLeftSlot.GetComponent<Slot>().colour == Colour.NONE;

        return topLeftEmpty && topRightEmpty && bottomRightEmpty && bottomLeftEmpty;
    }

    public void checkNeighboursHaveSingleColour() {
        Colour topLeftColour = topLeftSlot.GetComponent<Slot>().colour;
        Colour topRightColour = topRightSlot.GetComponent<Slot>().colour;
        Colour bottomRightColour = bottomRightSlot.GetComponent<Slot>().colour;
        Colour bottomLeftColour = bottomLeftSlot.GetComponent<Slot>().colour;

        bool allSlotsTheSameColour = topLeftColour != Colour.NONE &&
            topLeftColour == topRightColour &&
            topLeftColour == bottomRightColour &&
            topLeftColour == bottomLeftColour;

        if (allSlotsTheSameColour) {
            GameTracker.needToGoAgain = true;
            topLeftSlot.GetComponent<Slot>().setSlotFlags(0, topLeftColour);
            topRightSlot.GetComponent<Slot>().setSlotFlags(1, topLeftColour);
            bottomRightSlot.GetComponent<Slot>().setSlotFlags(2, topLeftColour);
            bottomLeftSlot.GetComponent<Slot>().setSlotFlags(3, topLeftColour);
        }
    }

    public void setNeighbourColours(Colour[] colours) {
        topLeftSlot.GetComponent<Slot>().setColour(colours[0]);
        topRightSlot.GetComponent<Slot>().setColour(colours[1]);
        bottomRightSlot.GetComponent<Slot>().setColour(colours[2]);
        bottomLeftSlot.GetComponent<Slot>().setColour(colours[3]);
    }

    public void removeColours() {
        topLeftSlot.GetComponent<Slot>().setColour(Colour.NONE);
        topRightSlot.GetComponent<Slot>().setColour(Colour.NONE);
        bottomRightSlot.GetComponent<Slot>().setColour(Colour.NONE);
        bottomLeftSlot.GetComponent<Slot>().setColour(Colour.NONE);
    }
}
