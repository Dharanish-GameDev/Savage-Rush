using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] Canvas GameOverCanvas;
   // [SerializeField] TextMeshProUGUI gameOverText;
    public Timer_Manager timer;
    [SerializeField] GameObject Meter;
    bool displayed;
    [SerializeField] WinDecuction win;
    [SerializeField] AudioSource ClickSound;
    void Start()
    {
        displayed = true;
        //GameOverCanvas.enabled = false;
       
    }

   
    void Update()
    {
        CanvasActivator();
    }
    void CanvasActivator()
    {
        if(!win.winned)
        {
            if (timer.current_Time == 0)
            {
                Invoke(nameof(GameOverPanelActivator),0.8f);
                if (displayed)
                {
                    //StartCoroutine(GameOverText());
                    displayed = false;
                }
                Meter.SetActive(false);
            }
        }
        
    }
    private void GameOverPanelActivator()
    {
        GameOverCanvas.enabled = true;
    }
    public void returnToMain()
    {
        if(!ClickSound.isPlaying) ClickSound.Play();
        Invoke("Returning", 0.2f);
    }
    public void Retry()
    {
        if (!ClickSound.isPlaying) ClickSound.Play();
        Invoke("Retrying", 0.2f);
    }
    //IEnumerator GameOverText()
    //{
       
    //    while(GameOverCanvas.enabled)
    //    {
    //        yield return new WaitForSeconds(0.8f);
    //        gameOverText.enabled = false;
    //        yield return new WaitForSeconds(0.8f);
    //        gameOverText.enabled = true;
          
    //    }  
    //}
    void Retrying()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void Returning()
    {
        SceneManager.LoadScene(0);
    }

}
