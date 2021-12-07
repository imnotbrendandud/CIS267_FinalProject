using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    //public Gradient gradient;
    public Image fill;

    private void Start()
    {
        slider = GetComponent<Slider>();
        SetHealth(PlayerHealth.playerHealth);
    }

    private void Update()
    {
       
    }

    public void SetHealthToMax()
    {
        slider.value = slider.maxValue;
        //fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
