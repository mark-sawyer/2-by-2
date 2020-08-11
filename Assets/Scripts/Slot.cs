using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {
    public Sprite redColour;
    public Sprite greenColour;
    public Sprite blueColour;
    public Sprite yellowColour;
    public Colour colour;
    private GameObject upNeighbourSlot;
    private GameObject rightNeighbourSlot;
    private GameObject downNeighbourSlot;
    private GameObject leftNeighbourSlot;

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
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<SpriteRenderer>().sprite = redColour;
                break;
            case Colour.GREEN:
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<SpriteRenderer>().sprite = greenColour;
                break;
            case Colour.BLUE:
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<SpriteRenderer>().sprite = blueColour;
                break;
            case Colour.YELLOW:
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<SpriteRenderer>().sprite = yellowColour;
                break;
            case Colour.NONE:
                GetComponent<SpriteRenderer>().enabled = false;
                break;
        }        
    }
}
