using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject ScrollView;
    [SerializeField] GameObject ScrollBt_Up;
    [SerializeField] Canvas VolumeCanvas;
    [SerializeField] AudioSource ClickingSFX;
    void Start()
    {
        ScrollView.SetActive(false);
        ScrollBt_Up.SetActive(false);
        VolumeCanvas.enabled = false;
    }
    public void CarSelection()
    {
        if (!ClickingSFX.isPlaying)
        {
            ClickingSFX.Play();
        }
        Invoke("CarLoad", 0.2f);
       
    }
    public void HowToPlay()
    {
        if (!ClickingSFX.isPlaying)
        {
            ClickingSFX.Play();
        }
        ScrollView.SetActive(true);
        ScrollBt_Up.SetActive(true);
    }
    public void CancelScroll()
    {
        if (!ClickingSFX.isPlaying)
        {
            ClickingSFX.Play();
        }
        ScrollView.SetActive(false);
        ScrollBt_Up.SetActive(false);
    }
    public void VolumeControl()
    {
        if (!ClickingSFX.isPlaying)
        {
            ClickingSFX.Play();
        }
        Invoke("VolumeLoad", 0.2f);
        
    }
    public void GameQuit()
    {
        if (!ClickingSFX.isPlaying)
        {
            ClickingSFX.Play();
        }
        PlayerPrefs.Save();
        Application.Quit();
    }
    void CarLoad()
    {
        SceneManager.LoadScene(1);
    }
    void VolumeLoad()
    {
        gameObject.SetActive(false);
        VolumeCanvas.enabled = true;
    }
}
