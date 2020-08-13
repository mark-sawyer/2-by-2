using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {
    public Animator anim;
    public Sprite redSprite;
    public Sprite greenSprite;
    public Sprite blueSprite;
    public Sprite yellowSprite;
    public Sprite noneSprite;
    private Animator transitionAnim;
    public Colour colour;
    private GameObject upNeighbourSlot;
    private GameObject rightNeighbourSlot;
    private GameObject downNeighbourSlot;
    private GameObject leftNeighbourSlot;
    public bool noneFlag;
    public bool redFlag;
    public bool greenFlag;
    public bool blueFlag;
    public bool yellowFlag;

    private void Start() {
        GameEvents.slotAnimationTime.AddListener(changeColourIfNecessary);
        transitionAnim = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

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

    public void setSlotFlags(int pos, Colour singleColour) {
        noneFlag = true; 

        switch (pos) {
            case 0:
                if (upNeighbourSlot != null && upNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE && upNeighbourSlot.GetComponent<Slot>().colour != singleColour) {
                    upNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                if (leftNeighbourSlot != null && leftNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE && leftNeighbourSlot.GetComponent<Slot>().colour != singleColour) {
                    leftNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                break;
            case 1:
                if (upNeighbourSlot != null && upNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE && upNeighbourSlot.GetComponent<Slot>().colour != singleColour) {
                    upNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                if (rightNeighbourSlot != null && rightNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE && rightNeighbourSlot.GetComponent<Slot>().colour != singleColour) {
                    rightNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                break;
            case 2:
                if (downNeighbourSlot != null && downNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE && downNeighbourSlot.GetComponent<Slot>().colour != singleColour) {
                    downNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                if (rightNeighbourSlot != null && rightNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE && rightNeighbourSlot.GetComponent<Slot>().colour != singleColour) {
                    rightNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                break;
            case 3:
                if (downNeighbourSlot != null && downNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE && downNeighbourSlot.GetComponent<Slot>().colour != singleColour) {
                    downNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                if (leftNeighbourSlot != null && leftNeighbourSlot.GetComponent<Slot>().colour != Colour.NONE && leftNeighbourSlot.GetComponent<Slot>().colour != singleColour) {
                    leftNeighbourSlot.GetComponent<Slot>().setFlagBasedOnSlotColour(colour);
                }
                break;
        }
    }

    public void setFlagBasedOnSlotColour(Colour slotToBeRemovedColour) {
        switch (slotToBeRemovedColour) {
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
        if (noneFlag) {
            redFlag = false;
            greenFlag = false;
            blueFlag = false;
            yellowFlag = false;
        }
        else {
            int flagNumber = 0;
            if (redFlag) {
                flagNumber++;
            }
            if (greenFlag) {
                flagNumber++;
            }
            if (blueFlag) {
                flagNumber++;
            }
            if (yellowFlag) {
                flagNumber++;
            }

            if (flagNumber > 1) {
                noneFlag = true;
                redFlag = false;
                greenFlag = false;
                blueFlag = false;
                yellowFlag = false;
            }
        }
    }

    private void changeColourIfNecessary() {
        if (noneFlag) {
            anim.SetTrigger("disappear");

            colour = Colour.NONE;
        }
        else if (redFlag) {
            anim.SetTrigger("become red");
            transitionAnim.SetTrigger("become red");
            colour = Colour.RED;
        }
        else if (greenFlag) {
            anim.SetTrigger("become green");
            transitionAnim.SetTrigger("become green");
            colour = Colour.GREEN;
        }
        else if (blueFlag) {
            anim.SetTrigger("become blue");
            transitionAnim.SetTrigger("become blue");
            colour = Colour.BLUE;
        }
        else if (yellowFlag) {
            anim.SetTrigger("become yellow");
            transitionAnim.SetTrigger("become yellow");
            colour = Colour.YELLOW;
        }

        noneFlag = false;
        redFlag = false;
        greenFlag = false;
        blueFlag = false;
        yellowFlag = false;
    }

    public bool slotIsFinalised() {
        bool spriteCorrect = false;
        bool animationStateCorrect = false;
        switch(colour) {
            case Colour.RED:
                spriteCorrect = GetComponent<SpriteRenderer>().sprite == redSprite;
                animationStateCorrect = anim.GetCurrentAnimatorStateInfo(0).IsName("red");
                break;
            case Colour.GREEN:
                spriteCorrect = GetComponent<SpriteRenderer>().sprite == greenSprite;
                animationStateCorrect = anim.GetCurrentAnimatorStateInfo(0).IsName("green");
                break;
            case Colour.BLUE:
                spriteCorrect = GetComponent<SpriteRenderer>().sprite == blueSprite;
                animationStateCorrect = anim.GetCurrentAnimatorStateInfo(0).IsName("blue");
                break;
            case Colour.YELLOW:
                spriteCorrect = GetComponent<SpriteRenderer>().sprite == yellowSprite;
                animationStateCorrect = anim.GetCurrentAnimatorStateInfo(0).IsName("yellow");
                break;
            case Colour.NONE:
                spriteCorrect = GetComponent<SpriteRenderer>().sprite == noneSprite;
                animationStateCorrect = anim.GetCurrentAnimatorStateInfo(0).IsName("none");
                break;
        }

        return spriteCorrect & animationStateCorrect;
    }
}
