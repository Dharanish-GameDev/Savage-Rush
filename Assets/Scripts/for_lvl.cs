using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class for_lvl : MonoBehaviour // 4,5
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
        SceneManager.LoadScene(4);
    }
    void lv2()
    {
        SceneManager.LoadScene(5);
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
