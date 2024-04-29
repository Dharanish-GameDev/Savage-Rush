using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Time_Adder : MonoBehaviour
{
    public int addAmount = 5;
    public bool TimeAdded;
    public ParticleSystem ClockBurst;

    
    void Update()
    {
        transform.Rotate(0, 1.2f, 0);
    }
   
}
