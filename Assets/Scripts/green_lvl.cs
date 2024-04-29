using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class green_lvl : MonoBehaviour // 9,10
{

    [SerializeField] AudioSource clickingSound;
    public void LV1()
    {
        if (!clickingSound.isPlaying) clickingSound.Play();
        Invoke("lv1", 0.2f);
    }
    public void LV2()
    {
        if (!clickingSound.isPlaying) clickingSound.Play();
        Invoke("lv2", 0.2f);
    }
    void lv1()
    {
        SceneManager.LoadScene(8);
    }                                                                              
    void lv2()
    {
        SceneManager.LoadScene(9);
    }
    public void BackButton()
    {
        if (!clickingSound.isPlaying)
        {
            clickingSound.Play();
        }
        Invoke("Back", 0.2f);
    }
    void Back()
    {
        SceneManager.LoadScene(2);
    }
}
