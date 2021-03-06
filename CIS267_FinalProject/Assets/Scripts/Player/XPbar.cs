using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPbar : MonoBehaviour
{
    private Slider slider;
    //public Gradient gradient;
    public Image fill;
    private TextMeshProUGUI levelNumber;

    private void Start()
    {
        levelNumber = GetComponentInChildren<TextMeshProUGUI>();
        slider = GetComponent<Slider>();
        SetXP(PlayerScore.playerScore);
        UpdateLevelNumber();

    }

    private void Update()
    {

    }

    public void UpdateLevelNumber()
    {
        levelNumber.SetText(PlayerScore.levelNumber.ToString());
    }

    public void SetXPToMax()
    {
        slider.value = slider.maxValue;
        //fill.color = gradient.Evaluate(1f);
    }
    public void SetXP(int xp)
    {
        slider.value = xp;
        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }

}
