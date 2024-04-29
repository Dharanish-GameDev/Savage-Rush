using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarSelection : MonoBehaviour
{
    public GameObject[] Cars;
    public AudioSource SwitchingSound;
    public int currentcar;
    public bool inGamePlayScene=false;
    Vector3 blazepos;
    Vector3 lillypos; Vector3 spookypos;
    [SerializeField] Material Sky_Forsake;
    [SerializeField] Material Sky_Green;
    [SerializeField] Material Sky_Taiga;
    [SerializeField]  GameObject GreenTitan;
    [SerializeField] GameObject Forsake;
    [SerializeField] GameObject Taiga;
   // [SerializeField] int BackButtonScene;

    private void Awake()
    {
        blazepos = Cars[0].transform.position;
        lillypos= Cars[1].transform.position;
        spookypos= Cars[2].transform.position;

    }
    void Start()
    {
        RenderSettings.skybox = Sky_Green;  
        int selectedCar = PlayerPrefs.GetInt("SelectedcarID");
        if (inGamePlayScene == true)
        {
            Cars[selectedCar].SetActive(true);
            currentcar = selectedCar;
        }
    }

   
    void Update()
    {
        
    }
    public void RightButton()
    {
        if (!SwitchingSound.isPlaying)
        {
            SwitchingSound.Play();
        }

        if (currentcar <= Cars.Length - 1)
        {
           
            currentcar += 1;
           
            if (currentcar>2)
            {
                currentcar = 0;
            }
            if(currentcar >2)
            {
                currentcar = currentcar - 1;
            }
            for (int i = 0; i < Cars.Length; i++)
            {
                Cars[i].gameObject.SetActive(false);
                Cars[0].transform.position= blazepos;
                Cars[1].transform.position = lillypos;
                Cars[2].transform.position = spookypos;
                Cars[currentcar].gameObject.SetActive(true); 
            }
        }
         
       
        
        SkyChanger();
    }

    public void LeftButton()
    {
        if (!SwitchingSound.isPlaying)
        {
            SwitchingSound.Play();
        }
        if (currentcar >= 0)
        {
            
            currentcar -= 1;
           
            if (currentcar<0)
            {
                currentcar = 2;
            }
            for (int i = 0; i < Cars.Length; i++)
            {
                Cars[i].gameObject.SetActive(false);
                Cars[0].transform.position = blazepos;
                Cars[1].transform.position = lillypos;
                Cars[2].transform.position = spookypos;
                Cars[currentcar].gameObject.SetActive(true);
            }
        }
        
        SkyChanger();
    }
    public void SelectButton()
    {
        if (!SwitchingSound.isPlaying)
        {
            SwitchingSound.Play();
        }
        Invoke("ChangeScene", 0.2f);
    }
    void SkyChanger()
    {
        if (currentcar == 0)
        {
            RenderSettings.skybox = Sky_Green;
            GreenTitan.SetActive(true);
            Taiga.SetActive(false);
            Forsake.SetActive(false);
        }
        else if(currentcar == 1)
        {
            RenderSettings.skybox = Sky_Taiga;
            Taiga.SetActive(true);
            Forsake.SetActive(false);
            GreenTitan.SetActive(false);
        }
        else if(currentcar==2)
        {
            RenderSettings.skybox = Sky_Forsake;
            Forsake.SetActive(true);
            GreenTitan.SetActive(false);
            Taiga.SetActive(false);
        }
    }
    void ChangeScene()
    {
        PlayerPrefs.SetInt("SelectedcarID", currentcar);
        SceneManager.LoadScene(2);
    }
    public void BackButton()
    {
        if(!SwitchingSound.isPlaying)
        {
            SwitchingSound.Play();
        }
        Invoke("Back", 0.2f);
    }
    void Back()
    {
        SceneManager.LoadScene(0);
    }
}
