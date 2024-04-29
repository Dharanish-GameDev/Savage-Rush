using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour   // FGT , 3,6,7
{

    [SerializeField] AudioSource ClickSound;
    [SerializeField] Button forsakeButton;
    [SerializeField] Button taigaButton;
    [SerializeField] Button greenTitanButton;
    

    void Start()
    {
        forsakeButton.onClick.AddListener(() => ForsakeButton());
        greenTitanButton.onClick.AddListener(() => GreenTitanButton());
        taigaButton.onClick.AddListener(() => TaigaButton());
    }


    private void ForsakeButton()
    {
        if (!ClickSound.isPlaying)
        {
            ClickSound.Play();
        }
        Invoke(nameof(Forsake), 0.2f);
    }
    void Forsake()
    {
        SceneManager.LoadScene(3);
    }
    private void GreenTitanButton()
    {
        if (!ClickSound.isPlaying)
        {
            ClickSound.Play();
        }
        Invoke(nameof(GreenTitan), 0.2f);
    }
    void GreenTitan()
    {
        SceneManager.LoadScene(6);
    }

    private void TaigaButton()
    {
        if (!ClickSound.isPlaying)
        {
            ClickSound.Play();
        }
        Invoke(nameof(Taiga), 0.2f);
    }
    void Taiga()
    {
        SceneManager.LoadScene(7);
    }


    public void BackButton()
    {
        if (!ClickSound.isPlaying)
        {
            ClickSound.Play();
        }
        Invoke("Back", 0.2f);
    }
    void Back()
    {
        SceneManager.LoadScene(1);
    }
}
