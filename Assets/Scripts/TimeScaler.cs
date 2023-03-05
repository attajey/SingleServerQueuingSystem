using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaler : MonoBehaviour
{
    public Slider slider;
    public Text currentValue;

    void Start()
    {
        Time.timeScale = 1.0f; 
        slider.value= 1.0f/10;
        Debug.Log(slider.value.ToString());
        currentValue.text = slider.value.ToString();
    }
    void Update()
    {
        Time.timeScale = slider.value*10;
        currentValue.text = (slider.value*10).ToString();

    }
}
