using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lvl_Selector : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        
    }
    public void Lv1()
    {
        SceneManager.LoadScene(4);
    }
    public void Lv2()
    {
        SceneManager.LoadScene(5);
    }
}
