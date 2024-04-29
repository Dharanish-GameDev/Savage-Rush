using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] GameObject On_Button;
    [SerializeField] GameObject Off_Button;
    [SerializeField] Canvas MainMenuCanvas;
    [SerializeField] Canvas VolumeCanvas;
    [SerializeField] AudioSource ClickingSound;
    void Start()
    {
        if(!PlayerPrefs.HasKey("GameVolume"))
        {
            PlayerPrefs.SetFloat("GameVolume", 1f);
            Load();
        }
        else
        {
            Load();
        }
        Off_Button.SetActive(false);
    }

    
    void Update()
    {
        
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Savevalue();
    }
    void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("GameVolume");
    }
    void Savevalue()
    {
        PlayerPrefs.SetFloat("GameVolume",volumeSlider.value);
    }
    public void EnableVolume()
    {
        AudioListener.volume = volumeSlider.value;
        On_Button.SetActive(true);
        Off_Button.SetActive(false);
    }
    public void DisableVolume()
    {
        AudioListener.volume = 0;
        Off_Button.SetActive(true);
        On_Button.SetActive(false);
    }
    public void BackToMainMenu()
    {
        if(!ClickingSound.isPlaying) ClickingSound.Play();
        VolumeCanvas.enabled= false;
        MainMenuCanvas.gameObject.SetActive(true);
    }
}
