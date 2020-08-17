using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Square : MonoBehaviour {
    public LayerMask nodeLayer;
    public Sprite redColour;
    public Sprite greenColour;
    public Sprite blueColour;
    public Sprite yellowColour;
    public Colour[] colours = new Colour[4];
    public int positionInQueue;
    public bool beingHeld;
    private bool isTransparent;
    private bool isTwoByOne;
    private Vector3 twoByOnePositionAdjustment;


    void Start() {
        GameEvents.rightPressed.AddListener(rotateClockwise);
        GameEvents.leftPressed.AddListener(rotateCounterClockwise);
        GameEvents.squarePlaced.AddListener(moveUpInQueue);

        // Choose how many little squares will be used in the piece.
        int numberOfSquares = Random.Range(2, 5);
        int numberOfSquaresAssigned = 0;
        while (numberOfSquaresAssigned < numberOfSquares) {
            int randomIndex = Random.Range(0, 4);
            if (colours[randomIndex] == Colour.NONE) {
                colours[randomIndex] = (Colour)Random.Range(1, 5);
                numberOfSquaresAssigned++;
            }

            // Once they're assigned, check they're not all the same colour.
            if (numberOfSquaresAssigned == numberOfSquares) {
                bool multipleColours = false;
                Colour firstColourObserved = Colour.NONE;
                for (int i = 0; i < 4; i++) {
                    if (colours[i] != Colour.NONE) {
                        if (firstColourObserved == Colour.NONE) {
                            firstColourObserved = colours[i];
                        }
                        else if (colours[i] != firstColourObserved) {
                            multipleColours = true;
                            if (numberOfSquares == 2 && 
                                !((colours[0] == Colour.NONE && colours[2] == Colour.NONE) || (colours[0] != Colour.NONE && colours[2] != Colour.NONE))) {

                                isTwoByOne = true;
                                twoByOnePositionAdjustment = getTwoByOnePositionAdjustment();
                            }
                            break;
                        }
                    }
                }
                if (!multipleColours) {
                    numberOfSquaresAssigned = 0;
                    for (int i = 0; i < 4; i++) {
                        colours[i] = Colour.NONE;
                    }
                }
            }
        }

        setColourSprites();
    }

    void Update() {
        if (beingHeld) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!isTwoByOne) {
                transform.position = new Vector3(mousePos.x, mousePos.y, -1);
            }
            else {
                transform.position = new Vector3(mousePos.x, mousePos.y, -1) + twoByOnePositionAdjustment;
            }
            

            // Check if valid node is under mouse
            bool overValidNode = false;
            Node validNode = null;
            RaycastHit2D ray = Physics2D.Raycast(transform.position, Vector2.zero, 0, nodeLayer);
            if (ray.collider != null && ray.collider.GetComponent<Node>().appropriateSlotsAreEmpty(colours)) {
                overValidNode = true;
                validNode = ray.collider.GetComponent<Node>();
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
                if (overValidNode) {
                    validNode.setNeighbourColours(colours);
                    Destroy(gameObject);
                    GameEvents.squarePlaced.Invoke();
                }
                else {
                    setAlpha(1);
                    transform.position = GameTracker.QUEUE_POSITIONS[0];
                }
            }
        }
    }

    private void rotateClockwise() {
        if (positionInQueue == 0) {
            Colour holdColour = colours[0];
            colours[0] = colours[3];
            colours[3] = colours[2];
            colours[2] = colours[1];
            colours[1] = holdColour;
        }

        if (isTwoByOne) {
            twoByOnePositionAdjustment = getTwoByOnePositionAdjustment();
        }

        setColourSprites();
    }

    private void rotateCounterClockwise() {
        if (positionInQueue == 0) {
            Colour holdColour = colours[0];
            colours[0] = colours[1];
            colours[1] = colours[2];
            colours[2] = colours[3];
            colours[3] = holdColour;
        }

        if (isTwoByOne) {
            twoByOnePositionAdjustment = getTwoByOnePositionAdjustment();
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
                case Colour.NONE:
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
                    break;
            }
        }
    }

    public void startBeingHeld() {
        beingHeld = true;
        setAlpha(0.5f);
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

    public void moveUpInQueue() {
        if (positionInQueue != 0) {
            positionInQueue--;
            transform.position = GameTracker.QUEUE_POSITIONS[positionInQueue];
        }
    }

    private Vector3 getTwoByOnePositionAdjustment() {
        Vector3 positionAdjustment;
        if (colours[0] == Colour.NONE && colours[1] == Colour.NONE) {
            positionAdjustment = new Vector3(0, 0.5f, 0);
        }
        else if (colours[1] == Colour.NONE && colours[2] == Colour.NONE) {
            positionAdjustment = new Vector3(0.5f, 0, 0);
        }
        else if (colours[2] == Colour.NONE && colours[3] == Colour.NONE) {
            positionAdjustment = new Vector3(0, -0.5f, 0);
        }
        else {
            positionAdjustment = new Vector3(-0.5f, 0, 0);
        }

        return positionAdjustment;
    }
}