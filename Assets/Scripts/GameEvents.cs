using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents {
    public static UnityEvent rightPressed = new UnityEvent();
    public static UnityEvent leftPressed = new UnityEvent();
    public static UnityEvent squarePlaced = new UnityEvent();
    public static UnityEvent slotAnimationTime = new UnityEvent();
    public static UnityEvent rWasPressed = new UnityEvent();
}
