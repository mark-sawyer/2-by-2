using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    private GameObject topLeftSlot;
    private GameObject topRightSlot;
    private GameObject bottomRightSlot;
    private GameObject bottomLeftSlot;

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

    public bool neighboursHaveSingleColour() {
        Colour topLeftColour = topLeftSlot.GetComponent<Slot>().colour;
        Colour topRightColour = topRightSlot.GetComponent<Slot>().colour;
        Colour bottomRightColour = bottomRightSlot.GetComponent<Slot>().colour;
        Colour bottomLeftColour = bottomLeftSlot.GetComponent<Slot>().colour;

        return topLeftColour != Colour.NONE && topLeftColour == topRightColour && topLeftColour == bottomRightColour && topLeftColour == bottomLeftColour;
    }

    public void setNeighbourColours(Colour[] colours) {
        topLeftSlot.GetComponent<Slot>().setColour(colours[0]);
        topRightSlot.GetComponent<Slot>().setColour(colours[1]);
        bottomRightSlot.GetComponent<Slot>().setColour(colours[2]);
        bottomLeftSlot.GetComponent<Slot>().setColour(colours[3]);
    }
}
