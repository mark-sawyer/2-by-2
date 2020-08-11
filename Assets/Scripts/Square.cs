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

    void Start() {
        GameEvents.rightPressed.AddListener(rotateClockwise);
        GameEvents.leftPressed.AddListener(rotateCounterClockwise);

        // Get four random colours, at least two unique.
        for (int i = 0; i < 4; i++) {
            colours[i] = (Colour)Random.Range(0, 4);
        }

        while (colours[0] == colours[1] & colours[0] == colours[2] & colours[0] == colours[3]) {
            for (int i = 0; i < 4; i++) {
                colours[i] = (Colour)Random.Range(0, 4);
            }
        }

        setColourSprites();
    }

    void Update() {
        if (beingHeld) {
            if (!Input.GetMouseButton(0)) {
                // Object was released. Check if node was click at location.
                beingHeld = false;
                bool hitNode = false;
                RaycastHit2D[] rays = Physics2D.RaycastAll(transform.position, Vector2.zero);
                for (int i = 0; i < rays.Length; i++) {
                    if (rays[i].collider.tag == "node") {
                        transform.position = rays[i].collider.transform.position;
                        hitNode = true;
                        break;
                    }
                }
                if (!hitNode) {
                    transform.position = GameTracker.POSITION_IN_QUEUE_ONE;
                }
            }
            else {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = mousePos;
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

    public void OnTriggerEnter2D(Collider2D collision) {
        if (positionInQueue == 1) {
            beingHeld = true;
        }
    }

}