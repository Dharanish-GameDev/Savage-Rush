using System.Collections;
using UnityEngine;

public class EngineAudio : MonoBehaviour
{
    public AudioSource RunningSound;
    public float runningMaxVolume;
    public float runningMaxPitch;
    public AudioSource ReversingSound;
    public float reversingMaxVolume;
    public float reversingMaxPitch;
    public AudioSource IdleSound;
    public float IdleMaxVolume;
    private CarController carController;
    public float revLimiter;
    public float limiterSound = 1f;
    public float limiterFrequency = 2f;
    public float limiterEngage = 0.8f;
    public AudioSource StartSoundOfEngine;
    public float speedRatio;
    public bool IsEngineRunning;
    public AudioSource HornSound;

    
    void Start()
    {
        carController = GetComponent<CarController>();
        ReversingSound.volume = 0;
        IdleSound.volume = 0;
        RunningSound.volume = 0;
    }
    void Update()
    {
        Car_Audio();
        Horn();
    }

    private void Car_Audio()
    {
        float speedSign = 0;
        if (carController)
        {
            speedSign = Mathf.Sign(carController.GetSpeedratio());
            speedRatio = Mathf.Abs(carController.GetSpeedratio());
        }
        if (speedRatio > limiterEngage)
        {
            revLimiter = (Mathf.Sin(Time.time * limiterFrequency) + 2f) * limiterSound * (speedRatio - limiterEngage);
        }

        if (IsEngineRunning)
        {
            IdleSound.volume = Mathf.Lerp(0.5f, IdleMaxVolume, speedRatio);

            if (speedSign > 0)
            {
                IdleSound.volume = 0f;
                ReversingSound.volume = 0;
                RunningSound.volume = Mathf.Lerp(0.3f, runningMaxVolume, speedRatio);
                RunningSound.pitch = Mathf.Lerp(0.3f, runningMaxPitch, speedRatio);
            }
            else
            {
                IdleSound.volume = 0.5f;
                RunningSound.volume = 0;
                ReversingSound.volume = Mathf.Lerp(0.3f, reversingMaxVolume, speedRatio);
                ReversingSound.pitch = Mathf.Lerp(1f, reversingMaxPitch, speedRatio);
            }

        }
        else
        {
            IdleSound.volume = 0f;
            RunningSound.volume = 0;
        }
    }

    public IEnumerator StartEngine()
    {
        carController.IsEngineRunning = 1;
        StartSoundOfEngine.Play();
        yield return new WaitForSeconds(1f);
        IsEngineRunning = true;
       
        yield return new WaitForSeconds(2f);
        carController.IsEngineRunning = 2;
    }
    void Horn()
    {
        if(Input.GetKeyDown(KeyCode.H)) HornSound.Play();
       
    }
    
}
