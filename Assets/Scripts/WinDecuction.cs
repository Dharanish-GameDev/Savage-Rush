using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinDecuction : MonoBehaviour
{
    [SerializeField] Canvas GameWonCanvas;
    [SerializeField] Timer_Manager timer;
    [SerializeField]GameObject PlayModeCar;
    [SerializeField] GameObject Blaze;
    [SerializeField] GameObject Lilly;
    [SerializeField] GameObject Spooky;
    public bool winned;
    [SerializeField] GameObject Meter;
    [SerializeField] ParticleSystem WinParticles;

    
    void Start()
    {
        winned = false;
        //GameWonCanvas.enabled = false;
    }

   
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(GameObject.FindGameObjectWithTag("Blaze")||GameObject.FindGameObjectWithTag("Lilly")||GameObject.FindGameObjectWithTag("Spooky"))
        {
            if(timer.current_Time >0)
            {
                WinParticles.Play();
                winned = true;
                Invoke(nameof(GameWonCanvasActivator), 0.8f);
                Meter.SetActive(false);
                StartCoroutine(CarDisable());
            }
        }
    }
    private void GameWonCanvasActivator()
    {
        GameWonCanvas.enabled = true;
    }
    IEnumerator CarDisable()
    {
        if (Blaze.gameObject.activeInHierarchy) PlayModeCar = Blaze;
        if (Lilly.gameObject.activeInHierarchy) PlayModeCar = Lilly;
        if (Spooky.gameObject.activeInHierarchy) PlayModeCar = Spooky;
        timer.enabled = false;
        yield return new WaitForSeconds(2f);
        PlayModeCar.gameObject.SetActive(false);
    }
}
