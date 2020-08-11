using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTracker : MonoBehaviour {
    public GameObject slot;
    public GameObject node;
    public static GameObject[,] slots;
    public static GameObject[,] nodes;
    public static int SIDE_LENGTH = 10;
    public static Vector2 POSITION_IN_QUEUE_ONE = new Vector2(6.5f, 3.75f);
    public static bool holdingSquare;

    void Start() {
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
        if (Input.GetMouseButtonDown(0)) {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector2.zero);
            if (ray.collider != null & ray.collider.tag == "square") {
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
}

public enum Colour {
    NONE,
    RED,
    GREEN,
    BLUE,
    YELLOW
};