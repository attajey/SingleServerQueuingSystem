using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaler : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        Time.timeScale = 1.0f;
        
    }
    void Update()
    {
        Time.timeScale = slider.value*10;
           
    }

}
