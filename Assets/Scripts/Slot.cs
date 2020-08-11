using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour {
    public Sprite redColour;
    public Sprite greenColour;
    public Sprite blueColour;
    public Sprite yellowColour;
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
}
