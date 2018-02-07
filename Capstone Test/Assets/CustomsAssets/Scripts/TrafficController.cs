using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficController : MonoBehaviour
{
    // Light timers and delay
    public float GreenTimer;
    public float YellowTimer;
    [HideInInspector]public float RedTimer;
    public float ChangeDelay;

    void Awake()
    {
        RedTimer = GreenTimer + YellowTimer + ChangeDelay;
    }
}