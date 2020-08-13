using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour {
    public GameObject square;
    public GameObject slot;
    public GameObject node;
    public GameObject[] queuedSquares;
    public GameObject nextBackground;
    public static GameObject[,] slots;
    public static GameObject[,] nodes;
    public static int SIDE_LENGTH = 10;
    public static bool holdingSquare;
    public static bool playable = true;
    public static bool needToGoAgain;
    public static Vector2[] QUEUE_POSITIONS = { new Vector2(6f, 3.75f), new Vector2(6f, 1.25f),
                                                new Vector2(6f, -1.25f), new Vector2(6f, -3.75f) };

    void Start() {
        GameEvents.squarePlaced.AddListener(dealWithSquareBeingPlaced);

        // Instantiate the four squares in the queue
        queuedSquares = new GameObject[4];
        for (int i = 0; i < 4; i++) {
            queuedSquares[i] = Instantiate(square, QUEUE_POSITIONS[i], Quaternion.identity);
            queuedSquares[i].GetComponent<Square>().positionInQueue = i;
        }

        // Instantiate the slots
        slots = new GameObject[SIDE_LENGTH, SIDE_LENGTH];
        for (int row = 0; row < SIDE_LENGTH; row++) {
            for (int col = 0; col < SIDE_LENGTH; col++) {
                slots[row, col] = Instantiate(slot, new Vector3(-5.5f + (float)row, -4.5f + (float)col, 0), Quaternion.identity);
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
                nodes[row, col] = Instantiate(node, new Vector3(-5f + row, -4f + col, 0), Quaternion.identity);
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
            if (Input.GetMouseButtonDown(0)) {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                holdingSquare = true;
                queuedSquares[0].GetComponent<Square>().startBeingHeld();
            }

            if (Input.GetKeyDown("right") | Input.GetKeyDown("d")) {
                GameEvents.rightPressed.Invoke();
            }
            else if (Input.GetKeyDown("left") | Input.GetKeyDown("a")) {
                GameEvents.leftPressed.Invoke();
            }
        }

        else if (needToGoAgain) {
            if (slotsAreFinalised()) {
                doAResolveColoursLoop();
            }
        }

        else {
            if (slotsAreFinalised()) {
                playable = true;
                nextBackground.GetComponent<NextBackground>().setPlayable();
            }
        }
    }

    public void dealWithSquareBeingPlaced() {
        // Shift current squares and instantiate new square
        for (int i = 1; i < 4; i++) {
            queuedSquares[i - 1] = queuedSquares[i];
        }
        queuedSquares[3] = Instantiate(square, QUEUE_POSITIONS[3], Quaternion.identity);
        queuedSquares[3].GetComponent<Square>().positionInQueue = 3;

        playable = false;
        nextBackground.GetComponent<NextBackground>().setWaiting();
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