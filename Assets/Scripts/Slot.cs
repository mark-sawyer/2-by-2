using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {
    public Animator anim;
    public Colour colour;
    private GameObject upNeighbourSlot;
    private GameObject rightNeighbourSlot;
    private GameObject downNeighbourSlot;
    private GameObject leftNeighbourSlot;
    private bool redFlag;
    private bool greenFlag;
    private bool blueFlag;
    private bool yellowFlag;
    private bool toBeDestroyed;

    public void setNeighbours() {
        RaycastHit2D ray;
        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(0, 1), Vector2.zero);
        if (ray.collider != null) {
            upNeighbourSlot = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(1, 0), Vector2.zero);
        if (ray.collider != null) {
            rightNeighbourSlot = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(0, -1), Vector2.zero);
        if (ray.collider != null) {
            downNeighbourSlot = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(-1, 0), Vector2.zero);
        if (ray.collider != null) {
            leftNeighbourSlot = ray.collider.gameObject;
        }
    }

    public void setColour(Colour newColour) {
        colour = newColour;

        switch (colour) {
            case Colour.RED:
                anim.SetTrigger("become red");
                break;
            case Colour.GREEN:
                anim.SetTrigger("become green");
                break;
            case Colour.BLUE:
                anim.SetTrigger("become blue");
                break;
            case Colour.YELLOW:
                anim.SetTrigger("become yellow");
                break;
            case Colour.NONE:
                anim.SetTrigger("disappear");
                break;
        }        
    }

    public void setNeighbourFlags(int pos) {
        toBeDestroyed = true;

        switch (pos) {
            case 0:
                if (upNeighbourSlot != null && upNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE) {
                    upNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                if (leftNeighbourSlot != null && leftNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE) {
                    leftNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                break;
            case 1:
                if (upNeighbourSlot != null && upNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE) {
                    upNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                if (rightNeighbourSlot != null && rightNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE) {
                    rightNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                break;
            case 2:
                if (downNeighbourSlot != null && downNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE) {
                    downNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                if (rightNeighbourSlot != null && rightNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE) {
                    rightNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                break;
            case 3:
                if (downNeighbourSlot != null && downNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE) {
                    downNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                if (leftNeighbourSlot != null && leftNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE) {
                    leftNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                break;
        }
    }

    public void setFlagBasedOnSlotColour(Colour slotToBeRemovedColour) {
        switch(slotToBeRemovedColour) {
            case Colour.RED:
                redFlag = true;
                break;
            case Colour.GREEN:
                greenFlag = true;
                break;
            case Colour.BLUE:
                blueFlag = true;
                break;
            case Colour.YELLOW:
                yellowFlag = true;
                break;
        }
    }

    public void resolveFlags() {
        int flagNumber = 0;
        if (redFlag) {
            setColour(Colour.RED);
            redFlag = false;
            flagNumber++;
        }
        if (greenFlag) {
            setColour(Colour.GREEN);
            greenFlag = false;
            flagNumber++;
        }
        if (blueFlag) {
            setColour(Colour.BLUE);
            blueFlag = false;
            flagNumber++;
        }
        if (yellowFlag) {
            setColour(Colour.YELLOW);
            yellowFlag = false;
            flagNumber++;
        }

        if (flagNumber > 1 | toBeDestroyed) {
            setColour(Colour.NONE);
        }

        toBeDestroyed = false;
    }

    public void invokeResolveLoop() {
        print("we invoking");
        if (!GameTracker.resolveColoursLoopBeenInvoked) {
            GameTracker.resolveColoursLoopBeenInvoked = true;
            GameTracker.doAResolveColoursLoop();
        }
    }
}
