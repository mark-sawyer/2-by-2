using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour {
    public GameObject square;
    public GameObject slot;
    public GameObject node;
    public static GameObject[,] slots;
    public static GameObject[,] nodes;
    public static int SIDE_LENGTH = 10;
    public static bool holdingSquare;
    public static bool playable = true;
    public static bool needToGoAgain;
    public static Vector2[] QUEUE_POSITIONS = { new Vector2(6.5f, 3.75f), new Vector2(6.5f, 1.25f),
                                                new Vector2(6.5f, -1.25f), new Vector2(6.5f, -3.75f) };

    void Start() {
        GameEvents.squarePlaced.AddListener(dealWithSquareBeingPlaced);

        // Instantiate the four squares in the queue
        for (int i = 0; i < 4; i++) {
            GameObject queueSquare = Instantiate(square, QUEUE_POSITIONS[i], Quaternion.identity);
            queueSquare.GetComponent<Square>().positionInQueue = i;
        }

        // Instantiate the slots
        slots = new GameObject[SIDE_LENGTH, SIDE_LENGTH];
        for (int row = 0; row < SIDE_LENGTH; row++) {
            for (int col = 0; col < SIDE_LENGTH; col++) {
                slots[row, col] = Instantiate(slot, new Vector3(-4.5f + (float)row, -4.5f + (float)col, 0), Quaternion.identity);
            }
        }

        // Set the slot neighbour references
        for (int row = 0; row < SIDE_LENGTH; row++) {
            for (int col = 0; col < SIDE_LENGTH; col++) {
                slots[row, col].GetComponent<Slot>().setNeighbours();
            }
        }

        // Instantiate the nodes
        nodes = new GameObject[SIDE_LENGTH - 1, SIDE_LENGTH - 1];
        for (int row = 0; row < SIDE_LENGTH - 1; row++) {
            for (int col = 0; col < SIDE_LENGTH - 1; col++) {
                nodes[row, col] = Instantiate(node, new Vector3(-4 + row, -4f + col, 0), Quaternion.identity);
            }
        }

        // Set the node neighbour references
        for (int row = 0; row < SIDE_LENGTH - 1; row++) {
            for (int col = 0; col < SIDE_LENGTH - 1; col++) {
                nodes[row, col].GetComponent<Node>().setNeighbours();
            }
        }

    }

    void Update() {
        if (playable) {
            //print("playable");
            if (Input.GetMouseButtonDown(0)) {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.zero);
                if (ray.collider != null && ray.collider.tag == "square" && ray.collider.GetComponent<Square>().positionInQueue == 0) {
                    holdingSquare = true;
                    ray.collider.GetComponent<Square>().startBeingHeld();
                }
            }

            if (Input.GetKeyDown("right") | Input.GetKeyDown("d")) {
                GameEvents.rightPressed.Invoke();
            }
            else if (Input.GetKeyDown("left") | Input.GetKeyDown("a")) {
                GameEvents.leftPressed.Invoke();
            }
        }

        else if (needToGoAgain) {
            //print("need to go again");
            if (slotsAreFinalised()) {
                doAResolveColoursLoop();
            }
        }

        else {
            //print("waiting for slots");
            if (slotsAreFinalised()) {
                playable = true;
            }
        }
    }

    public void dealWithSquareBeingPlaced() {
        // Instantiate new square
        GameObject newSquare = Instantiate(square, QUEUE_POSITIONS[3], Quaternion.identity);
        newSquare.GetComponent<Square>().positionInQueue = 3;
        playable = false;
        needToGoAgain = true;
    }

    public static void doAResolveColoursLoop() {
        needToGoAgain = false;

        // Check nodes for single colour and then set slot flags if they are
        for (int row = 0; row < SIDE_LENGTH - 1; row++) {
            for (int col = 0; col < SIDE_LENGTH - 1; col++) {
                nodes[row, col].GetComponent<Node>().checkNeighboursHaveSingleColour();  // set needToGoAgain true if a single colour node is found
            }
        }

        if (needToGoAgain) {
            // Resolve all slot flags
            for (int row = 0; row < SIDE_LENGTH; row++) {
                for (int col = 0; col < SIDE_LENGTH; col++) {
                    slots[row, col].GetComponent<Slot>().resolveFlags();
                }
            }

            GameEvents.slotAnimationTime.Invoke();
        }
    }

    public static bool slotsAreFinalised() {
        bool slotsDone = true;
        for (int row = 0; row < SIDE_LENGTH; row++) {
            for (int col = 0; col < SIDE_LENGTH; col++) {
                if (!slots[row, col].GetComponent<Slot>().slotIsFinalised()) {
                    slotsDone = false;
                    goto Over;
                }
            }
        }

        Over: return slotsDone;
    }
}


public enum Colour {
    NONE,
    RED,
    GREEN,
    BLUE,
    YELLOW
};