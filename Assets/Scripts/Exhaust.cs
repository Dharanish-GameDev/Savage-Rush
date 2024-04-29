using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exhaust : MonoBehaviour
{
    Timer_Manager time;
    public ParticleSystem[] Exhausts;
    Time_Adder adder;

    void Start()
    {
        time = FindObjectOfType<Timer_Manager>();
        
    } 
    void Update()
    {
        if(time.IsExhaust)
        {
            foreach (ParticleSystem ex in Exhausts)
            {
                ex.Play();
            }
            time.IsExhaust = false;
        }
    }
}
