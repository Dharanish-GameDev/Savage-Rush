using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Won : MonoBehaviour
{
    [SerializeField] int nextLevelIndex;
    [SerializeField] AudioSource clickSound;

    
    public void NextLevel()
    {
        if(!clickSound.isPlaying) clickSound.Play();
        Invoke("LevelNext", 0.2f);
    }
    public void ReturnToMainMenu()
    {
        if (!clickSound.isPlaying) clickSound.Play();
        Invoke("returning", 0.2f);
    }
    void returning()
    {
        SceneManager.LoadScene(0);
    }
    void LevelNext()
    {
        SceneManager.LoadScene(nextLevelIndex);
    }
   
}
