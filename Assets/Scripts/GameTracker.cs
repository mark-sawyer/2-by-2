using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTracker : MonoBehaviour {
    public GameObject square;
    public GameObject slot;
    public GameObject node;
    public GameObject[] queuedSquares;
    public GameObject nextBackground;
    public static GameObject scoreText;
    public Sprite greyBlock;
    public static GameObject[,] slots;
    public static GameObject[,] nodes;
    public static int SIDE_LENGTH = 10;
    public static bool playable = true;
    public static bool needToGoAgain;
    public static bool playerIsAlive = true;
    public static Vector2[] QUEUE_POSITIONS = { new Vector2(6f, 3.75f), new Vector2(6f, 1.25f),
                                                new Vector2(6f, -1.25f), new Vector2(6f, -3.75f) };
    private static float TIME_BETWEEN_GAME_OVER_BLOCKS = 0.075f;
    private float gameOverBlocksTimer = TIME_BETWEEN_GAME_OVER_BLOCKS;
    private int gameOverSequenceRowsCompleted;
    public static int score;
    public static int squaresCompleted;
    public static int loopsInTurn;

    void Start() {
        GameEvents.squarePlaced.AddListener(respondToSquareBeingPlaced);
        GameEvents.rWasPressed.AddListener(resetVariables);
        scoreText = GameObject.Find("Text");

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
        print("score: " + score);
        print("squares destroyed: " + squaresCompleted);

        // Check if player quit or restart
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
        if (Input.GetKeyDown("r")) {
            GameEvents.rWasPressed.Invoke();
        }

        if (playerIsAlive) {
            if (playable) {
                if (Input.GetMouseButtonDown(0)) {
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
                    playerIsAlive = !isGameOver();
                    if (playerIsAlive) {
                        nextBackground.GetComponent<NextBackground>().setPlayable();
                        loopsInTurn = 0;
                    }
                }
            }
        }

        // Player lost, game over sequence
        else {
            gameOverBlocksTimer -= Time.deltaTime;
            if (gameOverBlocksTimer <= 0) {
                if (gameOverSequenceRowsCompleted < 20) {
                    gameOverBlocksTimer = TIME_BETWEEN_GAME_OVER_BLOCKS;

                    for (int row = 0; row < 10; row++) {
                        for (int col = 0; col < 20; col++) {
                            if (row + col == gameOverSequenceRowsCompleted && row <= 9 && col <= 9) {
                                slots[row, 9 - col].GetComponent<Slot>().anim.SetTrigger("game over");
                            }
                        }
                    }
                    gameOverSequenceRowsCompleted++;
                }
                else {
                    print("game over");
                }
            }
        }
    }

    public void respondToSquareBeingPlaced() {
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
        loopsInTurn++;
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

    private bool isGameOver() {
        bool canPlaceSquare = false;
        for (int row = 0; row < SIDE_LENGTH - 1; row++) {
            for (int col = 0; col < SIDE_LENGTH - 1; col++) {
                if (nodes[row, col].GetComponent<Node>().neighboursAreEmpty()) {
                    canPlaceSquare = true;
                    goto Over;
                }
            }
        }

    Over: return !canPlaceSquare;
    }

    public static void respondToSingleColourNode() {
        needToGoAgain = true;
        squaresCompleted++;

        if (loopsInTurn == 1) {
            score++;
        }
        else if (loopsInTurn == 2) {
            score += 3;
        }
        else if (loopsInTurn == 3) {
            score += 10;
        }
        else {
            score += 25;
        }

        scoreText.GetComponent<Text>().text = "" + score;
        BarTimer.increaseTime();
    }

    public void resetVariables() {
        playerIsAlive = true;
        playable = true;
        needToGoAgain = false;
        score = 0;
        squaresCompleted = 0;
        loopsInTurn = 0;
        gameOverBlocksTimer = TIME_BETWEEN_GAME_OVER_BLOCKS;
        gameOverSequenceRowsCompleted = 0;

        SceneManager.LoadScene(0);
    }
}


public enum Colour {
    NONE,
    RED,
    GREEN,
    BLUE,
    YELLOW
};