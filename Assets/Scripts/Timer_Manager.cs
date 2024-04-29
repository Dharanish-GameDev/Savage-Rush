using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer_Manager : MonoBehaviour
{
    public TextMeshProUGUI timer_text;
    public TextMeshProUGUI TimeAddingText;
    public float current_Time;
    public bool count_Down;
    public bool hasLimit;
    public float time_Limit;
    public bool hasFormats;
    public Time_Formats format;
    public Dictionary<Time_Formats,string> time_formats= new Dictionary<Time_Formats,string>();
    float midTime;
    [SerializeField]
    ParticleSystem Car_Explosion_VFX;
    public AudioSource Car_Explosion_SFX;
    CarController car;
    bool playSFX;
    public GameObject carSFX;
    bool isPlayed;
    public ParticleSystem blaze_Part;
    public ParticleSystem lilly_Part;
    public ParticleSystem spooky_Part;
    public bool IsExhaust;
    public AudioSource TimeBomb;
    bool TimeBombPlayed;
    StartScreen stScreen;
    public bool changeAngle;
    [SerializeField] AudioSource ExhaustSFX;
    [SerializeField] GameObject blaze;
    [SerializeField] GameObject lilly;
    [SerializeField] GameObject spooky;
    [SerializeField] AudioSource Blaze_Ex;
    [SerializeField] AudioSource Lilly_Ex;
    [SerializeField] AudioSource Spooky_Ex;


    private void Awake()
    {
       

    }
    void Start()
    {
        if (blaze.gameObject.activeInHierarchy)
        {
            ExhaustSFX = Blaze_Ex;
        }
        else if (lilly.gameObject.activeInHierarchy)
        {
            ExhaustSFX = Lilly_Ex;
        }
        else if(spooky.gameObject.activeInHierarchy)
        {
            ExhaustSFX = Spooky_Ex;
        }
        changeAngle = false;
        timer_text.enabled = false;
        TimeAddingText.enabled = false;
        VFXselection();
        isPlayed = false;
        IsExhaust= false;
        time_formats.Add(Time_Formats.Whole, "0");
        time_formats.Add(Time_Formats.Tenth, "0.0");
        time_formats.Add(Time_Formats.Hundredth, "0.00");
        car = FindObjectOfType<CarController>();
        stScreen = FindObjectOfType<StartScreen>();
    }

    private void VFXselection()
    {
        if (blaze_Part.gameObject.activeInHierarchy)
        {
            Car_Explosion_VFX = blaze_Part;
        }
        else if (lilly_Part.gameObject.activeInHierarchy)
        {
            Car_Explosion_VFX = lilly_Part;
        }
        else if (spooky_Part.gameObject.activeInHierarchy)
        {
            Car_Explosion_VFX = spooky_Part;
        }
    }

    void Update()
    {
        if(ExhaustSFX==null)
        {
            if (blaze.gameObject.activeInHierarchy)
            {
                ExhaustSFX = Blaze_Ex;
            }
            else if (lilly.gameObject.activeInHierarchy)
            {
                ExhaustSFX = Lilly_Ex;
            }
            else if (spooky.gameObject.activeInHierarchy)
            {
                ExhaustSFX = Spooky_Ex;
            }
        }
        if(stScreen.StartTiming==true)
        {
            timer_text.enabled = true;
            current_Time = count_Down ? current_Time -= Time.deltaTime : current_Time += Time.deltaTime;
            Time_textColor();
            SetTimerText();
            Sfx_Vfx();
        }
    }

    private void Sfx_Vfx()
    {
        if (current_Time == 0)
        {
            Car_Explosion_VFX.Play();
            playSFX = true;
            StartCoroutine(ExplosionVFX());
        }
        if (playSFX && !isPlayed)
        {
          
            Car_Explosion_SFX.Play();
            isPlayed = true;
            StartCoroutine(ExplosionSFX());
        }
    }

    private void Time_textColor()
    {
        if (current_Time <=10 && current_Time != time_Limit)
        {
            timer_text.color = Color.yellow;
        }
       
        if (hasLimit && ((count_Down && current_Time <= time_Limit) || (!count_Down && current_Time >= time_Limit)))
        {
            current_Time = time_Limit;
            SetTimerText();
            
        }
    }
    public void AddingTime( Time_Adder adder)
    {
        if (!adder.TimeAdded)
        {
           
            TimeBombPlayed = false;
            IsExhaust = true;
            adder.TimeAdded = true;
            TimeAddingText.enabled = true;
            TimeAddingText.text = "+ " + adder.addAmount;
            current_Time += adder.addAmount;
            TimeBombPlayed = false;
            if (!ExhaustSFX.isPlaying)
            {
                ExhaustSFX.Play();
            }
            adder.ClockBurst.Play();
            StartCoroutine(clockSFX(adder));
            StartCoroutine(CameraPosition());
            if (current_Time <= midTime)
            {
                timer_text.color = Color.yellow;
            }
            else
            {
                timer_text.color = Color.white;
            }
        }
       
    }

    private void SetTimerText()
    {
        
        if (current_Time > 5)
        {
            timer_text.text = "Time: " + (hasFormats ? current_Time.ToString(time_formats[format]) : current_Time.ToString());
        }
        if(current_Time<=5)
        {
           // hasFormats = false;
            timer_text.text = "Time: " + current_Time.ToString("0.00");
            if (!TimeBomb.isPlaying && !TimeBombPlayed)
            {
                TimeBomb.Play();
                TimeBombPlayed = true;
            }
           // else TimeBomb.Stop();
        }
        if (current_Time <= 3)
        {
            timer_text.color = Color.red;
            TimeBomb.volume = 1f;
        }
        else TimeBomb.volume = 0f;
    }
     IEnumerator clockSFX(Time_Adder adder)
     {
        yield return new WaitForSeconds(0.01f);
        adder.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.2f);
        TimeAddingText.enabled = false;
     }
    IEnumerator ExplosionVFX()
    {
        yield return new WaitForSeconds(0.7f);
        car.gameObject.SetActive(false);
    }
    IEnumerator ExplosionSFX()
    {
        yield return new WaitForSeconds(1f);
        playSFX = false;
    }
    IEnumerator CameraPosition()
    {
        changeAngle = true;
        yield return new WaitForSeconds(3f);
        changeAngle = false;
    }
}
public enum Time_Formats
{
    Whole,
    Tenth,
    Hundredth
}
