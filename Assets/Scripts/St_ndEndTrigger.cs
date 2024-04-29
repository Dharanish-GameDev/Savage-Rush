using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class St_ndEndTrigger : MonoBehaviour
{
    [SerializeField] bool carCrossed;
    [SerializeField] MeshRenderer stMesh;
    [SerializeField] MeshRenderer finishMesh;
    [SerializeField] BoxCollider WinCollider;
    [SerializeField] BoxCollider BlockBack;
    
    void Start()
    {
        WinCollider.enabled = false;
        finishMesh.enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameObject.FindGameObjectWithTag("Blaze")|| GameObject.FindGameObjectWithTag("Lilly")|| GameObject.FindGameObjectWithTag("Spooky"))
        {
            carCrossed = true;
        }
        else carCrossed = false;

        if(carCrossed)
        {
            stMesh.enabled = false;
            WinCollider.enabled = true;
            finishMesh.enabled = true;
            BlockBack.isTrigger = true;
        }
       
    }
}
