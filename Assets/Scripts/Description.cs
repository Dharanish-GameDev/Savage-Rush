using UnityEngine;

public class Description : MonoBehaviour
{
    public GameObject blaze_description;
    public GameObject lilly_description;
    public GameObject spooky_description;
    public GameObject blaze;
    public GameObject lilly;
    public GameObject spooky;
    CarSelection car;
    
    void Start()
    {
        blaze_description.SetActive(false);
        lilly_description.SetActive(false);
        spooky_description.SetActive(false);
    }
    void Update()
    {
        if(blaze.activeInHierarchy)
        {
            blaze_description.SetActive(true);
        }
        else
        {
            blaze_description.SetActive(false);
        }
        if (lilly.activeInHierarchy)
        {
            lilly_description.SetActive(true);
        }
        else
        {
            lilly_description.SetActive(false);
        }
        if (spooky.activeInHierarchy)
        {
            spooky_description.SetActive(true);
        }
        else
        {
            spooky_description.SetActive(false);
        }
    }
}
