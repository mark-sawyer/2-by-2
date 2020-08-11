using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {
    public Sprite redColour;
    public Sprite greenColour;
    public Sprite blueColour;
    public Sprite yellowColour;
    public Colour[] colours = new Colour[4];
    private int positionInQueue = 1;
    private bool beingHeld;
    private bool isTransparent;

    void Start() {
        GameEvents.rightPressed.AddListener(rotateClockwise);
        GameEvents.leftPressed.AddListener(rotateCounterClockwise);

        // Get four random colours, at least two unique.
        for (int i = 0; i < 4; i++) {
            colours[i] = (Colour)Random.Range(1, 5);
        }

        while (colours[0] == colours[1] & colours[0] == colours[2] & colours[0] == colours[3]) {
            for (int i = 0; i < 4; i++) {
                colours[i] = (Colour)Random.Range(1, 5);
            }
        }

        setColourSprites();
    }

    void Update() {
        if (beingHeld) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;

            // Check if valid node is under mouse
            bool overValidNode = false;
            Node validNode = null;
            RaycastHit2D[] rays = Physics2D.RaycastAll(transform.position, Vector2.zero);
            for (int i = 0; i < rays.Length; i++) {
                if (rays[i].collider.tag == "node") {
                    if (rays[i].collider.GetComponent<Node>().neighboursAreEmpty()) {
                        overValidNode = true;
                        validNode = rays[i].collider.GetComponent<Node>();
                    }
                    break;
                }
            }

            // Adjust transparency if required
            if (overValidNode & isTransparent) {
                setAlpha(1);
            }
            else if (!overValidNode & !isTransparent) {
                setAlpha(0.5f);
            }

            // Place square if it was released over a valid node
            if (!Input.GetMouseButton(0)) {
                beingHeld = false;
                GameTracker.holdingSquare = false;
                if (overValidNode) {
                    validNode.setNeighbourColours(colours);
                    Destroy(gameObject);
                }
                else {
                    setAlpha(1);
                    transform.position = GameTracker.POSITION_IN_QUEUE_ONE;
                }
            }
        }
    }

    private void rotateClockwise() {
        if (positionInQueue == 1) {
            Colour holdColour = colours[0];
            colours[0] = colours[3];
            colours[3] = colours[2];
            colours[2] = colours[1];
            colours[1] = holdColour;
        }

        setColourSprites();
    }

    private void rotateCounterClockwise() {
        if (positionInQueue == 1) {
            Colour holdColour = colours[0];
            colours[0] = colours[1];
            colours[1] = colours[2];
            colours[2] = colours[3];
            colours[3] = holdColour;
        }

        setColourSprites();
    }

    private void setColourSprites() {
        for (int i = 0; i < 4; i++) {
            switch (colours[i]) {
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

    public void startBeingHeld() {
        if (positionInQueue == 1) {
            beingHeld = true;
            setAlpha(0.5f);
        }
    }

    public void setAlpha(float alphaVal) {
        Color temp;
        for (int i = 0; i < 4; i++) {
            temp = transform.GetChild(i).GetComponent<SpriteRenderer>().color;
            temp.a = alphaVal;
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = temp;
        }

        isTransparent = alphaVal == 0.5f;
    }
}