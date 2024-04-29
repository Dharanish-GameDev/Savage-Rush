using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public TextMeshProUGUI threeScreen;
    public bool StartTiming;
    public  CarController blazecontroller;
    public  CarController lillycontroller;
    public  CarController spookycontroller;
    [SerializeField] AudioSource stBeepSound;
    
    void Start()
    {
        threeScreen.color = Color.red;
        blazecontroller.enabled = false;
        lillycontroller.enabled = false;
        spookycontroller.enabled = false;
        StartTiming = false;
        StartCoroutine(ThreeScreen());
    }

   
    void Update()
    {
       
    }
    IEnumerator ThreeScreen()
    {
        stBeepSound.PlayDelayed(0.6f);
        yield return new WaitForSeconds(1f);
        threeScreen.text = "3";
        yield return new WaitForSeconds(1f);
        threeScreen.text = "2";
        yield return new WaitForSeconds(1f);
        threeScreen.text = "1";
        yield return new WaitForSeconds(1f);
        threeScreen.color = Color.green;
        threeScreen.text = "START";
        yield return new WaitForSeconds(0.5f);
        StartTiming = true;
        if(blazecontroller.gameObject.activeInHierarchy)
        {
            blazecontroller.enabled = true;
        }
        else if(lillycontroller.gameObject.activeInHierarchy)
        {
            lillycontroller.enabled = true;
        }
        else if(spookycontroller.gameObject.activeInHierarchy)
        {
            spookycontroller.enabled = true;
        }
        
      
        threeScreen.enabled = false;


    }
}
