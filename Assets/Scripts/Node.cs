using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {
    private GameObject upRightSlot;
    private GameObject rightDownSlot;
    private GameObject downLeftSlot;
    private GameObject leftUpSlot;

    public void setNeighbours() {
        RaycastHit2D ray;
        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(0.5f, 0.5f), Vector2.zero);
        if (ray.collider != null) {
            upRightSlot = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(0.5f, -0.5f), Vector2.zero);
        if (ray.collider != null) {
            rightDownSlot = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(-0.5f, -0.5f), Vector2.zero);
        if (ray.collider != null) {
            downLeftSlot = ray.collider.gameObject;
        }

        ray = Physics2D.Raycast((Vector2)transform.position + new Vector2(-0.5f, 0.5f), Vector2.zero);
        if (ray.collider != null) {
            leftUpSlot = ray.collider.gameObject;
        }
    }
}
