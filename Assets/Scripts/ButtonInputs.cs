using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonInputs : MonoBehaviour
{
    public bool IsPressed;
    public float dampeningPress = 0f;
    public float sensitivity = 2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsPressed)
        {
            dampeningPress += sensitivity * Time.deltaTime;
        }
        else
        {
            dampeningPress -= sensitivity * Time.deltaTime;
        }
        dampeningPress = Mathf.Clamp01(dampeningPress);
    }
    public void OnClickUp()
    {
        IsPressed = false;
    }
    public void OnClickDown()
    {
        IsPressed = true;
    }

}
