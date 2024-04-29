using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Pause_Menu : MonoBehaviour
{
    [SerializeField] Canvas PauseMenu;
    [SerializeField] AudioSource clickSound;
    float volumeFloat;
    [SerializeField] Material skyboxMat;
    [SerializeField] Button PauseButton;
    [SerializeField] AudioSource[] Horns;
    AudioSource Horn;
    [SerializeField] List<Light> headLights;
    [SerializeField] List<Light> ActiveLights;
   
    
   
    void Start()
    {
        foreach( var horn in Horns)
        {
             if(horn.gameObject.activeInHierarchy)
             {
                Horn = horn;
             }
        }
        foreach( var head in headLights)
        {
            if(head.gameObject.activeInHierarchy)
            {
                ActiveLights.Add(head);
            }
        }
        volumeFloat = PlayerPrefs.GetFloat("GameVolume");
    }

    
    void Update()
    {
        if(RenderSettings.skybox != skyboxMat)
        {
            RenderSettings.skybox = skyboxMat;
        }
       
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!clickSound.isPlaying) clickSound.Play();
            if(!PauseMenu.enabled)
            {
                PauseMenu.enabled = true;
                Time.timeScale = 0;
                AudioListener.volume = 0;
            }
            else if(PauseMenu.enabled)
            {
                Resume();
            }
           
        }
        
    }
    public void Resume()
    {
        AudioListener.volume = volumeFloat;
        if (!clickSound.isPlaying) clickSound.Play();
        Time.timeScale = 1;
        PauseMenu .enabled = false;
    }
    public void ReturnToMainMenu()
    {
        
        Time.timeScale = 1;
        AudioListener.volume = volumeFloat;
        if (!clickSound.isPlaying) clickSound.Play();
        Invoke("GotoMain", 0.2f);
    }
    public void Retry()
    {
        Time.timeScale = 1;
        AudioListener.volume = volumeFloat;
        if (!clickSound.isPlaying) clickSound.Play();
        StartCoroutine(retrying());
    }
    IEnumerator retrying()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void GotoMain()
    {
        SceneManager.LoadScene(0);
    }
    public void PauseBut()
    {
        if (!clickSound.isPlaying) clickSound.Play();
        if (!PauseMenu.enabled)
        {
            PauseMenu.enabled = true;
            Time.timeScale = 0;
            AudioListener.volume = 0;
        }
        else if (PauseMenu.enabled)
        {
            Resume();
        }
    }
    public void HornPlay()
    {
        Horn.Play();
    }
    public void LightON()
    {
        foreach (Light light in ActiveLights)
        {
            light.intensity = light.intensity == 0 ? 75f : 0f;
        }
    }
    
}
